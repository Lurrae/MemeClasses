using MemeClasses.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace MemeClasses.Items
{
	public class ZiplineShooter : ModItem
	{

		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.shoot = ProjectileType<Zipline>();
			Item.value = Item.sellPrice(silver: 60);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item5;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			player.FindSentryRestingSpot(type, out int xx, out int yy, out _);
			int ai = (player.altFunctionUse == 2) ? 1 : 0;

			if (Collision.CanHitLine(player.Center, 1, 1, new Vector2(xx, yy - 12), 1, 1))
			{
				for (int p = 0; p < Main.maxProjectiles; p++)
				{
					Projectile proj = Main.projectile[p];

					if (proj.type == type && proj.ai[0] == ai)
						proj.Kill();
				}

				Projectile.NewProjectile(source, new Vector2(xx, yy - 12), Vector2.Zero, type, 0, 0, player.whoAmI, ai);
			}
			
			return false; // Don't spawn one from the default shoot code, as we already spawned one
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Harpoon)
				.AddIngredient(ItemID.Hook)
				.AddIngredient(ItemID.Wire, 50)
				.AddRecipeGroup(RecipeGroupID.IronBar, 10)
				.AddIngredient(ItemID.Chain, 10)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}