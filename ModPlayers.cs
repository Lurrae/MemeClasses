using MemeClasses.Items.Pulleys;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
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
					else if (ActivePulley.type == ItemType<MechPulley>() && Player.velocity != Vector2.Zero)
					{
						MechPulley_BuildUpCharge();
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

		private void MechPulley_BuildUpCharge()
		{
			if (MechPulleyCharge < 3f)
			{
				MechPulleyCharge += 0.01f;
				if (MechPulleyCharge == 1f || MechPulleyCharge == 2f || MechPulleyCharge == 3f)
				{
					Main.NewText("Reached charge level " + (int)MechPulleyCharge + "!");
				}
			}
		}

		private readonly List<int> targets = new();

		private void MechPulley_SummonBolts()
		{
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
					
					if (targets.Count >= i)
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
			targets.Clear();
		}
	}
}