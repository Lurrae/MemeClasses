using Terraria;
using Terraria.ID;

namespace MemeClasses.Items.Pulleys
{
	public class MistPulley : BasePulley
	{
		public override void StaticPulleyDefaults()
		{
		}

		public override void SetPulleyDefaults()
		{
			Item.damage = 10;
			Item.knockBack = 1f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 45);
		}
	}
}