using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace MemeClasses.Items.Accessories
{
	public class Rotor : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.accessory = true;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(gold: 1, silver: 50);
		}

		public override void UpdateEquip(Player player)
		{
			PulleyPlayer pPlr = player.GetModPlayer<PulleyPlayer>();

			pPlr.Rotor = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.HallowedBar, 10)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}