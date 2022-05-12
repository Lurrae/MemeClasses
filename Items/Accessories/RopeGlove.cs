using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace MemeClasses.Items.Accessories
{
	public class RopeGlove : ModItem
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

			pPlr.PulleySpeed += 0.05f; // +5% pulley speed
			pPlr.BonusRopeRange += 1; // +1 rope placement range
			pPlr.RopeGlove = true; // Increased rope grab range
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup("MemeClasses:Ropes", 25)
				.AddTile(TileID.Loom)
				.Register();
		}
	}
}