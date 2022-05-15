using Terraria;
using Terraria.ID;

namespace MemeClasses.Items.Pulleys
{
	public class ShroomPulley : BasePulley
	{
		public override void StaticPulleyDefaults()
		{
		}

		public override void SetPulleyDefaults()
		{
			Item.damage = 11;
			Item.knockBack = 6f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(gold: 2);
		}
	}
}