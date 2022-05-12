using Terraria;
using Terraria.GameContent.Creative;
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
			Item.damage = 18;
			Item.knockBack = 8f;
		}
	}
}