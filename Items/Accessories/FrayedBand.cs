using MemeClasses.Items.Pulleys;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace MemeClasses.Items.Accessories
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class FrayedBand : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.accessory = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(copper: 85);
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(GetInstance<PulleyDamageClass>()).Flat += 2; // +2 damage
			player.GetCritChance(GetInstance<PulleyDamageClass>()) += 2f; // +2% crit chance
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup("MemeClasses:Ropes", 5)
				.AddTile(TileID.Loom)
				.Register();
		}
	}
}