using Terraria;
using Terraria.ID;

namespace MemeClasses.Items.Pulleys
{
	public class RopeEater : BasePulley
	{
		public override void StaticPulleyDefaults()
		{
		}

		public override void SetPulleyDefaults()
		{
			Item.damage = 10;
			Item.knockBack = 1f;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(gold: 1);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.RichMahogany, 8)
				.AddIngredient(ItemID.JungleSpores, 12)
				.AddIngredient(ItemID.Vine, 2)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}