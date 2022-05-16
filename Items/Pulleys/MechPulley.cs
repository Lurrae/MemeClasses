using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MemeClasses.Items.Pulleys
{
	public class MechPulley : BasePulley
	{
		public override void StaticPulleyDefaults()
		{
		}

		public override void SetPulleyDefaults()
		{
			Item.damage = 25;
			Item.knockBack = 8f;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(gold: 4);
		}
	}
}