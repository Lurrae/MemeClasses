using Terraria;
using Terraria.ID;

namespace MemeClasses.Items.Pulleys
{
	public class FancyPulley : BasePulley
	{
		public override void StaticPulleyDefaults()
		{
		}

		public override void SetPulleyDefaults()
		{
			Item.damage = 14;
			Item.knockBack = 7f;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(silver: 90);
		}
	}
}