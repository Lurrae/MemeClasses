using MemeClasses.Items.Pulleys;
using MemeClasses.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace MemeClasses
{
	public class ModGlobalNPC : GlobalNPC
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
		}

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