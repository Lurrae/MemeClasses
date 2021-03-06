using Terraria;
using Terraria.ID;

namespace MemeClasses.Items.Pulleys
{
	public class SlimePulley : BasePulley
	{
		public override void StaticPulleyDefaults()
		{
		}

		public override void SetPulleyDefaults()
		{
			Item.damage = 10;
			Item.knockBack = 8f;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(gold: 2);
		}
	}
}