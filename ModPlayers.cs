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
		public int MaxCharges;
		public int ChargesToRetain;
		public bool JustLeftRope;
		public bool OnZipline;
		public bool MovingOnZipline;
		public bool MovingHorizontallyOnVanillaRope;
		public bool Rotor;

		public override void ResetEffects()
		{
			PulleySpeed = 1f;
			BonusRopeRange = 0;
			MaxCharges = 3;
			ChargesToRetain = 1;
			RopeGlove = false;
			RopeGlove2 = false;
			ActivePulley = null;
			Rotor = false;
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

		public override void PostUpdate()
		{
			bool movingDown = Player.velocity.Y > 0 || (MovingOnZipline && Player.controlDown);

			if (OnZipline)
			{
				Player.pulley = true;
			}
			
			if (Player.pulley && ActivePulley != null && ActivePulley.type == ItemType<SlimePulley>() && movingDown)
			{
				Player.armorEffectDrawShadow = true;
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

		public override void PreUpdateMovement()
		{
			if (Player.pulley)
			{
				Player.instantMovementAccumulatedThisFrame.X *= PulleySpeed;
				Player.velocity *= PulleySpeed;
				Player.maxFallSpeed *= PulleySpeed;

				if (Player.instantMovementAccumulatedThisFrame.X != 0)
				{
					MovingHorizontallyOnVanillaRope = true;
				}
				else
				{
					MovingHorizontallyOnVanillaRope = false;
				}
			}
		}

		private int timer = 0;
		private int moveTimer = 0;
		public override void PostUpdateMiscEffects()
		{
			if (Player.pulley)
			{
				// Handle all custom pulleys' behaviors
				if (ActivePulley != null)
				{
					if (ActivePulley.type == ItemType<SlimePulley>())
					{
						SlimePulley_DownwardMovement();
					}
					else if (ActivePulley.type == ItemType<MechPulley>() && MechPulleyCharge >= MaxCharges)
					{
						MechPulley_MaxCharge();
					}
					else if (ActivePulley.type == ItemType<RopeEater>())
					{
						float range = 196 * PulleySpeed;

						for (int i = 0; i < 10; i++)
						{
							Dust dust = Dust.NewDustDirect(Player.Center, 1, 1, DustID.JungleGrass);
							dust.position += Main.rand.NextVector2CircularEdge(range, range);
							dust.noGravity = true;
						}

						if (Player.ownedProjectileCounts[ProjectileType<RopeEaterMinion>()] < 2)
						{
							RopeEater_ManEaters();
						}
					}
					else if (ActivePulley.type == ItemType<ShroomPulley>())
					{
						float range = 196 * PulleySpeed;

						for (int i = 0; i < 10; i++)
						{
							Dust dust = Dust.NewDustDirect(Player.Center, 1, 1, DustID.GlowingMushroom);
							dust.position += Main.rand.NextVector2CircularEdge(range, range);
							dust.noGravity = true;
						}

						timer++;
						if (timer >= 15 && Player.ownedProjectileCounts[ProjectileID.TruffleSpore] < 40)
						{
							ShroomPulley_Spores();
							timer = 0;
						}
					}

					if (Player.velocity != Vector2.Zero || MovingHorizontallyOnVanillaRope || MovingOnZipline)
					{
						moveTimer++;

						if (ActivePulley.type == ItemType<MechPulley>())
						{
							MechPulley_BuildUpCharge();
						}
						else if (ActivePulley.type == ItemType<HellstonePulley>() && moveTimer >= Math.Max(16 - (2 * PulleySpeed), 3))
						{
							HellstonePulley_LingeringFlames();
							moveTimer = 0;
						}
						else if (ActivePulley.type == ItemType<WoodenPulley>() && moveTimer >= Math.Max(15 - (0.05f * Player.velocity.Length()), 3))
						{
							WoodPulley_Projectiles();
							moveTimer = 0;
						}
						else if (ActivePulley.type == ItemType<FancyPulley>() && moveTimer >= Math.Max(12 - (0.1f * Player.velocity.Length()), 3))
						{
							FancyPulley_BloodGlobs();
							moveTimer = 0;
						}
						else if (ActivePulley.type == ItemType<MistPulley>() && moveTimer >= 10) // This one does not scale with velocity/pulley speed
						{
							MistPulley_MistClouds();
							moveTimer = 0;
						}
					}

					if (ActivePulley.type != ItemType<MechPulley>() && MechPulleyCharge >= 1f)
					{
						MechPulley_SummonBolts();
					}
				}
			}
			else if (JustLeftRope && ActivePulley != null)
			{
				if (ActivePulley.type == ItemType<MechPulley>())
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
				// Player moves downwards faster
				Player.velocity *= 1.2f;
				Player.maxFallSpeed *= 1.6f;

				// Damage enemies
				int collisions = Player.CollideWithNPCs(Player.getRect(), ActivePulley.damage + Player.velocity.Y, ActivePulley.knockBack, 20, 6);
				if (collisions > 0)
				{
					Player.velocity.Y = -16;
				}
			}
		}

		private int soundsPlayed = 0;
		private void MechPulley_BuildUpCharge()
		{
			if (MechPulleyCharge < MaxCharges)
			{
				MechPulleyCharge += 0.01f + (0.01f * (Player.position.Distance(Player.oldPosition) / 4));
				MechPulleyCharge = (float)Math.Round(MechPulleyCharge, 2);

				for (int i = 1; i < MaxCharges + 1; i++)
				{
					if (MechPulleyCharge >= i && soundsPlayed < i)
					{
						SoundEngine.PlaySound(SoundID.MaxMana, Player.Center);
						for (int j = 0; j < 10; j++)
						{
							Dust.NewDust(Player.position, Player.width, Player.height, DustID.Electric, Main.rand.Next(-5, 5), Main.rand.Next(-5, -3));
						}
						soundsPlayed++;
					}
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
					bolt.ArmorPenetration = 10; // Ignores 10 enemy defense
				}
			}
			if (Rotor && charge > ChargesToRetain)
			{
				MechPulleyCharge = ChargesToRetain;
				soundsPlayed = ChargesToRetain;
			}
			else
			{
				MechPulleyCharge = 0f;
				soundsPlayed = 0;
			}
			targets.Clear();
		}

		private void HellstonePulley_LingeringFlames()
		{
			Vector2 vector = new(Player.width / 2, Player.height / 2);
			Vector2 vector2 = new((int)(Player.position.X + vector.X - 9) + 10, (int)(Player.position.Y + vector.Y + 2f * Player.gravDir + -26 * Player.gravDir));

			Projectile.NewProjectile(Player.GetSource_ItemUse(ActivePulley), vector2, Vector2.Zero, ProjectileType<HellstoneFlame>(), ActivePulley.damage, ActivePulley.knockBack, Player.whoAmI);
		}

		private void WoodPulley_Projectiles()
		{
			Vector2 velocity = Vector2.UnitY;
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(45f));
			if (Player.velocity.Y < 0)
				velocity.Y *= 6f;

			int type;
			int damageMod;
			if (Main.rand.NextBool(3)) // 1/3 chance to shoot a splinter
			{
				type = ProjectileType<Splinter>();
				damageMod = 2;
				velocity.X *= 8;
				velocity.Y *= -4; // Multiply by a negative number to make it shoot upwards
			}
			else // 2/3 chance to shoot a rope shred
			{
				type = ProjectileType<RopeShred>();
				damageMod = 1;
				velocity.X *= 4;
				velocity.Y *= -3;
			}

			Projectile proj = Projectile.NewProjectileDirect(Player.GetSource_ItemUse(ActivePulley), Player.Center, velocity, type, ActivePulley.damage * damageMod, ActivePulley.knockBack * damageMod, Player.whoAmI);
			proj.friendly = true;
			proj.hostile = false;
			proj.DamageType = GetInstance<PulleyDamageClass>();
		}

		private void FancyPulley_BloodGlobs()
		{
			Vector2 velocity = Vector2.UnitY;
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(45f));
			if (Player.velocity.Y < 0)
				velocity.Y *= 5f;

			Projectile proj = Projectile.NewProjectileDirect(Player.GetSource_ItemUse(ActivePulley), Player.Center, velocity * -3.5f, ProjectileType<BloodGlob>(), ActivePulley.damage, ActivePulley.knockBack, Player.whoAmI, 3);
			proj.DamageType = GetInstance<PulleyDamageClass>();
		}

		private void RopeEater_ManEaters()
		{
			Projectile.NewProjectile(Player.GetSource_ItemUse(ActivePulley), Player.Center, Vector2.Zero, ProjectileType<RopeEaterMinion>(), ActivePulley.damage, ActivePulley.knockBack, Player.whoAmI, Player.ownedProjectileCounts[ProjectileType<RopeEaterMinion>()]);
		}

		private void ShroomPulley_Spores()
		{
			float range = 198 * PulleySpeed;
			Vector2 spawnPos = Player.Center + Main.rand.NextVector2Circular(range, range);

			Projectile.NewProjectile(Player.GetSource_ItemUse(ActivePulley, "ShroomPulley_Spore"), spawnPos, Vector2.Zero, ProjectileID.TruffleSpore, ActivePulley.damage, ActivePulley.knockBack, Player.whoAmI);
		}

		private void MistPulley_MistClouds()
		{
			var source = Player.GetSource_ItemUse(ActivePulley, "MistPulley_Mist");

			Projectile proj = Projectile.NewProjectileDirect(source, Player.Center, Vector2.Zero, ProjectileID.SporeCloud, ActivePulley.damage, ActivePulley.knockBack, Player.whoAmI);
			proj.scale = Player.velocity.Length() * 0.3f; // Projectiles are larger when the player is moving faster
		}
	}
}