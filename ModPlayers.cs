using MemeClasses.Items.Pulleys;
using MemeClasses.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace MemeClasses
{
	public class PulleyPlayer : ModPlayer
	{
		public float PulleySpeed;
		public int BonusRopeRange;
		public bool RopeGlove; // Magnetism effect
		public bool RopeGlove2; // Magnetism effect, but stronger
		public Item ActivePulley; // What pulley item is in the player's pulley slot?
		public float MechPulleyCharge;
		public bool JustLeftRope;
		public bool MovementOverride;

		public override void ResetEffects()
		{
			PulleySpeed = 1f;
			BonusRopeRange = 0;
			RopeGlove = false;
			RopeGlove2 = false;
			ActivePulley = null;
		}

		public override void UpdateEquips()
		{
			foreach (Item item in Player.inventory)
			{
				if (MemeClasses.ItemIsRope(item, "rope"))
				{
					item.tileBoost = 3 + BonusRopeRange; // We can't do += here because tileBoost does not reset every frame, so it would just increase infinitely
				}
			}
		}

		// Prevent the vanilla pulley sprite from drawing if a custom one needs to be drawn instead
		public override void HideDrawLayers(PlayerDrawSet drawInfo)
		{
			if (Player.pulley && ActivePulley != null && !GetInstance<PulleyAccSlot>().HideVisuals)
			{
				PlayerDrawLayers.Pulley.Hide();
			}
		}

		private int timer = 0;
		public override void PostUpdateMiscEffects()
		{
			if (Player.pulley)
			{
				Player.velocity *= PulleySpeed;
				Player.maxFallSpeed *= PulleySpeed;

				// Handle all custom pulleys' behaviors
				if (ActivePulley != null)
				{
					if (ActivePulley.type == ItemType<SlimePulley>())
					{
						SlimePulley_DownwardMovement();
					}
					else if (ActivePulley.type == ItemType<MechPulley>() && MechPulleyCharge >= 3f)
					{
						MechPulley_MaxCharge();
					}

					if (Player.velocity != Vector2.Zero || MovementOverride)
					{
						timer++;

						if (ActivePulley.type == ItemType<MechPulley>())
						{
							MechPulley_BuildUpCharge();
						}
						else if (ActivePulley.type == ItemType<HellstonePulley>() && timer >= 5)
						{
							HellstonePulley_LingeringFlames();
							timer = 0;
						}
					}

					if (ActivePulley.type != ItemType<MechPulley>() && MechPulleyCharge >= 1f)
					{
						MechPulley_SummonBolts();
					}
				}
			}
			else if (ActivePulley != null)
			{
				if (JustLeftRope && ActivePulley.type == ItemType<MechPulley>())
				{
					MechPulley_SummonBolts();
				}
			}

			JustLeftRope = Player.pulley;
		}

		private void SlimePulley_DownwardMovement()
		{
			Player.noKnockback = true;

			if (Player.velocity.Y > 0 && Player.controlDown) // Moving downwards
			{
				// Double downwards speed
				Player.velocity *= 1.2f;
				Player.maxFallSpeed *= 2f;

				// Afterimage effect + damage enemies
				Player.armorEffectDrawShadow = true;
				Player.CollideWithNPCs(Player.getRect(), ActivePulley.damage, ActivePulley.knockBack, 10, 10);
			}
		}

		private int soundsPlayed = 0;
		private void MechPulley_BuildUpCharge()
		{
			if (MechPulleyCharge < 3f)
			{
				MechPulleyCharge += 0.01f * (Player.position.Distance(Player.oldPosition) / 4);
				MechPulleyCharge = (float)Math.Round(MechPulleyCharge, 2);

				if ((MechPulleyCharge >= 1f && soundsPlayed < 1) || (MechPulleyCharge >= 2f && soundsPlayed < 2) || (MechPulleyCharge >= 3f && soundsPlayed < 3))
				{
					SoundEngine.PlaySound(SoundID.MaxMana, Player.Center);
					for (int i = 0; i < 10; i++)
					{
						Dust.NewDust(Player.position, Player.width, Player.height, DustID.Electric, Main.rand.Next(-5, 5), Main.rand.Next(-5, -3));
					}
					soundsPlayed++;
				}
			}
		}

		private void MechPulley_MaxCharge()
		{
			float x = Main.rand.NextFloat(-0.2f, 0.2f);
			float y = Main.rand.NextFloat(-0.2f, 0.2f);

			Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.CosmicCarKeys, x, y);
			dust.noGravity = true;

			foreach (NPC npc in Main.npc)
			{
				// Only release charge if there's an enemy in range
				if (npc.CanBeChasedBy() && Player.Distance(npc.Center) <= 512f)
				{
					MechPulley_SummonBolts();
					break;
				}
			}
		}

		private readonly List<int> targets = new();
		private void MechPulley_SummonBolts()
		{
			int charge = (int)Math.Floor(MechPulleyCharge);
			
			if (charge > 0)
			{
				SoundEngine.PlaySound(SoundID.Item94, Player.Center);

				for (int n = 0; n < Main.maxNPCs; n++)
				{
					NPC npc = Main.npc[n];

					if (!npc.CanBeChasedBy() || Player.Distance(npc.Center) > 512f)
					{
						continue; // Skip any NPCs we can't target
					}

					targets.Add(npc.whoAmI);
					if (targets.Count == 3)
						break;
				}

				for (int i = 0; i < charge; i++)
				{
					var source = Player.GetSource_ItemUse(ActivePulley, "MechPulley_Bolt");
					Vector2 newVel;

					if (targets.Count > 0)
					{
						Vector2 targetPos;

						if (targets.Count < i + 1)
						{
							int j = Main.rand.Next(targets.Count);
							targetPos = Main.npc[targets[j]].Center;
						}
						else
						{
							targetPos = Main.npc[targets[i]].Center;
						}

						newVel = targetPos - Player.Center;
						newVel.Normalize();
					}
					else
					{
						newVel = Vector2.One.RotatedByRandom(MathHelper.ToRadians(360f));
					}

					Projectile bolt = Projectile.NewProjectileDirect(source, Player.Center, newVel * 5f, ProjectileID.MagnetSphereBolt, ActivePulley.damage, ActivePulley.knockBack, Player.whoAmI);
					bolt.DamageType = GetInstance<PulleyDamageClass>();
					bolt.maxPenetrate = bolt.penetrate = charge; // Hits one enemy per charge
				}
			}
			MechPulleyCharge = 0f;
			soundsPlayed = 0;
			targets.Clear();
		}

		private void HellstonePulley_LingeringFlames()
		{
			Vector2 vector = new(Player.width / 2, Player.height / 2);
			Vector2 vector2 = new((int)(Player.position.X + vector.X - 9) + 10, (int)(Player.position.Y + vector.Y + 2f * Player.gravDir + -26 * Player.gravDir));

			Projectile.NewProjectile(Player.GetSource_ItemUse(ActivePulley), vector2, Vector2.Zero, ProjectileType<HellstoneFlame>(), ActivePulley.damage, ActivePulley.knockBack, Player.whoAmI);
		}
	}
}