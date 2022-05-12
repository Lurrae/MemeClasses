using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace MemeClasses.Items.Pulleys
{
	public class PulleyItem : BasePulley
	{
		public override void StaticPulleyDefaults()
		{
		}

		public override void SetPulleyDefaults()
		{
			Item.damage = 5;
		}
	}
}