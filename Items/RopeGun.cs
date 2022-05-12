using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace MemeClasses.Items
{
	public class RopeGun : ModItem
	{

		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.useAmmo = 999;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.RopeCoil;
			Item.shootSpeed = 20f;
			Item.value = Item.sellPrice(silver: 20);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item5;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup(RecipeGroupID.IronBar, 5)
				.AddRecipeGroup(RecipeGroupID.Wood, 5)
				.AddRecipeGroup("MemeClasses:RopeCoils")
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}