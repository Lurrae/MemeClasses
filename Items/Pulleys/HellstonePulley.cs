using Terraria;
using Terraria.ID;

namespace MemeClasses.Items.Pulleys
{
	public class HellstonePulley : BasePulley
	{
		public override void StaticPulleyDefaults()
		{
		}

		public override void SetPulleyDefaults()
		{
			Item.damage = 30;
			Item.knockBack = 4f;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(gold: 5);
		}
	}
}