using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace MemeClasses.Items.Accessories
{
	public class RopeOil : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.accessory = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(gold: 3);
		}

		public override void UpdateEquip(Player player)
		{
			PulleyPlayer pPlr = player.GetModPlayer<PulleyPlayer>();

			pPlr.PulleySpeed += 0.1f;
		}
	}
}