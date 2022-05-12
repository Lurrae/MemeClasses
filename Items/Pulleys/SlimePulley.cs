using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

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
		}
	}
}