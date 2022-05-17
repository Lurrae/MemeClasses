using Terraria;
using Terraria.ID;

namespace MemeClasses.Items.Pulleys
{
	public class WoodenPulley : BasePulley
	{
		public override void StaticPulleyDefaults()
		{
		}

		public override void SetPulleyDefaults()
		{
			Item.damage = 7;
			Item.knockBack = 2f;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(copper: 80);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup(RecipeGroupID.Wood, 7)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}