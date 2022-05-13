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
					if (Player.velocity != Vector2.Zero)
					{
						timer++;

						if (ActivePulley.type == ItemType<MechPulley>())
						{
							MechPulley_BuildUpCharge();
						}
						else if (ActivePulley.type == ItemType<HellstonePulley>() && timer == 10)
						{
							HellstonePulley_LingeringFlames();
							timer = 0;
						}
					}
				}
			}
			else if (JustLeftRope && ActivePulley.type == ItemType<MechPulley>())
			{
				MechPulley_SummonBolts();
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
			else
			{
				MechPulley_SummonBolts();
			}
		}

		private readonly List<int> targets = new();
		private void MechPulley_SummonBolts()
		{
			SoundEngine.PlaySound(SoundID.Item94, Player.Center);
			
			int charge = (int)Math.Round(MechPulleyCharge);
			foreach (NPC npc in Main.npc)
			{
				if (!npc.CanBeChasedBy() || Player.Distance(npc.Center) > 512f)
				{
					continue; // Skip any NPCs we can't target
				}

				targets.Add(npc.whoAmI);
				if (targets.Count == 3)
					break;
			}

			if (charge > 0)
			{
				for (int i = 0; i < charge; i++)
				{
					var source = Player.GetSource_ItemUse(ActivePulley);
					float damage = ActivePulley.damage * MechPulleyCharge;
					Projectile bolt = Projectile.NewProjectileDirect(source, Player.Center, Vector2.One * 5f, ProjectileID.MagnetSphereBolt, (int)damage, ActivePulley.knockBack, Player.whoAmI);
					bolt.DamageType = GetInstance<PulleyDamageClass>();
					
					if (targets.Count > i)
					{
						Vector2 targetPos = Main.npc[targets[i]].Center;
						Vector2 newVel = targetPos - Player.Center;
						newVel.Normalize();
						bolt.velocity = newVel * 5f;
					}
					else
					{
						bolt.velocity = bolt.velocity.RotatedByRandom(MathHelper.ToRadians(360f));
					}
				}
			}
			MechPulleyCharge = 0f;
			soundsPlayed = 0;
			targets.Clear();
		}

		private void HellstonePulley_LingeringFlames()
		{
			int pulleyDir = Player.pulleyDir;
			float gravDir = Player.gravDir;
			int dir = Player.direction;

			int num = (pulleyDir == 2) ? 0 : 10;
			int num2 = (pulleyDir == 2) ? -25 : -26;
			Vector2 vector = new(Player.width / 2, Player.height / 2);
			Vector2 vector2 = new((int)(Player.position.X + vector.X - (9 * dir)) + num * dir, (int)(Player.position.Y + vector.Y + 2f * gravDir + num2 * gravDir));

			Projectile.NewProjectile(Player.GetSource_ItemUse(ActivePulley), vector2, Vector2.Zero, ProjectileType<HellstoneFlame>(), ActivePulley.damage, ActivePulley.knockBack, Player.whoAmI);
		}
	}
}