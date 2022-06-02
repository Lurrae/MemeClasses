using MemeClasses.Items.Pulleys;
using MemeClasses.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace MemeClasses
{
	public class ModDebuffNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		public float SlowPercent;

		public override void SetDefaults(NPC npc)
		{
			npc.buffImmune[BuffID.Stoned] = npc.buffImmune[BuffID.Slow] = npc.buffImmune[BuffID.OgreSpit] = IsStopImmune(npc);
			npc.buffImmune[BuffID.Chilled] = npc.buffImmune[BuffID.Frozen] = IsImmuneToCold(npc);

			if (npc.type == NPCID.Medusa)
				npc.buffImmune[BuffID.Stoned] = true;

			if (npc.type == NPCID.DD2OgreT2 || npc.type == NPCID.DD2OgreT3)
				npc.buffImmune[BuffID.OgreSpit] = true;

			if (IsNotOrganic(npc))
				npc.buffImmune[BuffID.Bleeding] = true;
		}

		private static bool IsImmuneToCold(NPC npc) // NPCs which can't be slowed/stopped, or deal cold damage (or are Undead/Armored Vikings) are immune to cold debuffs
		{
			return IsStopImmune(npc) || npc.coldDamage || npc.type == NPCID.UndeadViking || npc.type == NPCID.ArmoredViking;
		}

		private static bool IsStopImmune(NPC npc) // Checks if the NPC is a boss or a worm
		{
			return npc.boss || npc.realLife > 0;
		}

		// Why can't there be an easier way to do this...
		// Thankfully we can ignore enemies with negative IDs (like the big/small variants of skeletons), since they are counted as the main one
		private static bool IsNotOrganic(NPC npc)
		{
			bool skeleton = false;
			bool mechanical = false;
			bool plant = false;
			bool ghastly = false;
			bool rocky = false;

			switch (npc.type)
			{
				case NPCID.AngryBones:
				case NPCID.AngryBonesBig:
				case NPCID.AngryBonesBigHelmet:
				case NPCID.AngryBonesBigMuscle:
				case NPCID.ArmoredSkeleton:
				case NPCID.ArmoredViking:
				// Yay, enemies with a million variants for no reason!
				case NPCID.BlueArmoredBones:
				case NPCID.BlueArmoredBonesMace:
				case NPCID.BlueArmoredBonesNoPants:
				case NPCID.BlueArmoredBonesSword:
				case NPCID.RustyArmoredBonesAxe:
				case NPCID.RustyArmoredBonesFlail:
				case NPCID.RustyArmoredBonesSword:
				case NPCID.RustyArmoredBonesSwordNoArmor:
				case NPCID.HellArmoredBones:
				case NPCID.HellArmoredBonesMace:
				case NPCID.HellArmoredBonesSpikeShield:
				case NPCID.HellArmoredBonesSword:
				// Okay now back to normal enemies again...
				case NPCID.BoneLee:
				case NPCID.BoneSerpentHead:
				case NPCID.BoneSerpentBody:
				case NPCID.BoneSerpentTail:
				case NPCID.CursedSkull:
				case NPCID.DarkCaster:
				case NPCID.DD2SkeletonT1:
				case NPCID.DD2SkeletonT3:
				case NPCID.DiabolistRed:
				case NPCID.DiabolistWhite:
				case NPCID.DoctorBones:
				case NPCID.DungeonGuardian:
				case NPCID.GiantCursedSkull:
				case NPCID.GreekSkeleton:
				case NPCID.Necromancer:
				case NPCID.NecromancerArmored:
				case NPCID.RuneWizard:
				// I hate skeletons I hate skeletons I hate skeletons I hate-
				// WHY do they have SO MANY variants????
				case NPCID.BoneThrowingSkeleton: // WHY ARE THERE FOUR OF THESE-
				case NPCID.BoneThrowingSkeleton2:
				case NPCID.BoneThrowingSkeleton3:
				case NPCID.BoneThrowingSkeleton4:
				case NPCID.HeadacheSkeleton:
				case NPCID.MisassembledSkeleton:
				case NPCID.PantlessSkeleton:
				case NPCID.Skeleton:
				case NPCID.SkeletonAlien:
				case NPCID.SkeletonArcher:
				case NPCID.SkeletonAstonaut: // Why is this misspelled in vanilla code???
				// Now back to our regularly scheduled skeletons that aren't variants of the "skeleton" enemy
				case NPCID.SkeletonCommando:
				case NPCID.SkeletonSniper:
				case NPCID.SkeletonTopHat:
				case NPCID.SkeletronHand:
				case NPCID.SkeletronHead:
				case NPCID.SporeSkeleton:
				case NPCID.TacticalSkeleton:
				case NPCID.Tim:
				case NPCID.UndeadMiner:
				case NPCID.UndeadViking:
					skeleton = true;
					break;
				case NPCID.BigMimicCorruption:
				case NPCID.BigMimicCrimson:
				case NPCID.BigMimicHallow:
				case NPCID.BigMimicJungle:
				case NPCID.CrimsonAxe:
				case NPCID.CursedHammer:
				case NPCID.DeadlySphere:
				case NPCID.TheDestroyer:
				case NPCID.TheDestroyerBody:
				case NPCID.TheDestroyerTail:
				case NPCID.EnchantedSword:
				case NPCID.IceMimic:
				case NPCID.PirateShipCannon:
				case NPCID.MartianProbe:
				case NPCID.MartianSaucer:
				case NPCID.MartianSaucerCannon:
				case NPCID.MartianSaucerCore:
				case NPCID.MartianSaucerTurret:
				case NPCID.Mimic:
				case NPCID.PossessedArmor:
				case NPCID.PrimeCannon:
				case NPCID.PrimeLaser:
				case NPCID.PrimeSaw:
				case NPCID.PrimeVice:
				case NPCID.Probe:
				case NPCID.Retinazer:
				case NPCID.SkeletronPrime:
				case NPCID.Spazmatism:
					mechanical = true;
					break;
				case NPCID.Tumbleweed:
				case NPCID.Dandelion:
				case NPCID.Everscream:
				case NPCID.FungiBulb:
				case NPCID.GiantFungiBulb:
				case NPCID.ManEater:
				case NPCID.MourningWood:
				case NPCID.Plantera:
				case NPCID.PlanterasTentacle:
					plant = true;
					break;
				case NPCID.AncientCultistSquidhead:
				case NPCID.CultistDragonBody1:
				case NPCID.CultistDragonBody2:
				case NPCID.CultistDragonBody3:
				case NPCID.CultistDragonBody4:
				case NPCID.DesertDjinn:
				case NPCID.Flocko:
				case NPCID.Ghost:
				case NPCID.IceQueen:
				case NPCID.PirateGhost:
				case NPCID.Poltergeist:
				case NPCID.Reaper:
				case NPCID.SandElemental:
				case NPCID.ShadowFlameApparition:
				case NPCID.Wraith:
					ghastly = true;
					break;
				case NPCID.GraniteFlyer:
				case NPCID.GraniteGolem:
				case NPCID.IceGolem:
				case NPCID.MeteorHead:
				case NPCID.RockGolem:
					rocky = true;
					break;
			}

			return skeleton || mechanical || plant || ghastly || rocky;
		}

		public override void ResetEffects(NPC npc)
		{
			SlowPercent = 0f;
		}

		// Modifies movement speed where applicable
		public override bool PreAI(NPC npc)
		{
			// We first have to check for any debuffs that should stop movement
			// We can return false to skip running any further ai code
			if (npc.HasBuff(BuffID.Stoned) || npc.HasBuff(BuffID.Frozen))
			{
				// Prevent NPCs from moving or animating
				npc.position = npc.oldPosition;
				npc.frameCounter = 0;

				if (npc.HasBuff(BuffID.Stoned))
					npc.position.Y -= 10; // Force NPCs to fall very fast when Stoned

				return false;
			}

			// All of this code down here runs only if enemies are not frozen/stoned

			// Find all of the active slowing debuffs and apply their effects
			// I'm not using if/else here or an "or" statement because these effects should stack
			if (npc.HasBuff(BuffID.Chilled))
			{
				SlowPercent += 0.25f; // 25% reduced movement speed
			}
			if (npc.HasBuff(BuffID.Slow))
			{
				SlowPercent += 0.5f; // 50% reduced movement speed
			}
			if (npc.HasBuff(BuffID.OgreSpit))
			{
				SlowPercent += 0.66f; // 66% reduced movement speed
			}

			// Once all of their effects have been applied, move them backwards by a certain amount so they appear to move slower
			if (SlowPercent > 0f)
			{
				if (SlowPercent >= 1f) // 100% slowness means the enemy cannot move
				{
					npc.position = npc.oldPosition; // Send them back to their previous position so they visually don't move
					npc.frameCounter = 0; // Prevent animations from happening
					return false; // Don't run ai at all
				}
				npc.position -= npc.velocity * SlowPercent;
			}

			return base.PreAI(npc);
		}

		// Damage over time effects
		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			if (npc.HasBuff(BuffID.Bleeding))
			{
				if (npc.lifeRegen > 0)
					npc.lifeRegen = 0;

				npc.lifeRegen -= 4;
			}
		}

		// Modify the color of NPCs when under the effects of certain debuffs
		// Also add some particle effects with certain debuffs (like Bleeding)
		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (npc.HasBuff(BuffID.Frozen) || npc.HasBuff(BuffID.Chilled))
				drawColor = Color.Aqua;

			if (npc.HasBuff(BuffID.Stoned))
				drawColor = Color.Gray;

			if (npc.HasBuff(BuffID.OgreSpit))
				drawColor = Color.DarkOliveGreen;

			if (npc.HasBuff(BuffID.Bleeding) && Main.rand.NextBool(30))
			{
				Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Blood);
				Dust dust2 = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Blood);
				dust2.velocity.Y += 0.5f;
				dust.velocity *= 0.25f;
			}
		}
	}

	// Got loot modifications? Drop them in here!
	public class ModLootNPC : GlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			if (npc.type == NPCID.KingSlime)
			{
				npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ItemType<SlimePulley>(), 10)); // 1/10 drop chance
			}

			if (npc.type == NPCID.TheGroom)
			{
				npcLoot.Add(ItemDropRule.Common(ItemType<FancyPulley>())); // Guaranteed drop since The Groom is pretty rare
			}

			if (npc.type == NPCID.SporeSkeleton)
			{
				npcLoot.Add(ItemDropRule.Common(ItemType<ShroomPulley>(), 50)); // 1/50 drop chance
			}
		}
	}

	// Anything that modifies the shops of town NPCs (and the Traveling Merchant) goes here
	public class ModShopNPC : GlobalNPC
	{
		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			if (type == NPCID.Mechanic)
			{
				shop.item[nextSlot].SetDefaults(ItemType<MechPulley>());
				nextSlot++;
			}
		}

		public override void SetupTravelShop(int[] shop, ref int nextSlot)
		{
			if (Main.rand.NextBool(7))
			{
				shop[nextSlot] = ItemType<RopeOil>();
				nextSlot++;
			}
		}
	}
}