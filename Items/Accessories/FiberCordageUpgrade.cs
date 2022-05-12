using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace MemeClasses.Items.Accessories
{
	public class FiberCordageUpgrade : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.accessory = true;
		}

		public override void UpdateEquip(Player player)
		{
			PulleyPlayer pPlr = player.GetModPlayer<PulleyPlayer>();

			player.cordage = true;

			pPlr.PulleySpeed += 0.05f;
			pPlr.BonusRopeRange += 2;
			pPlr.RopeGlove2 = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.CordageGuide)
				.AddIngredient(ItemType<RopeGlove>())
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}