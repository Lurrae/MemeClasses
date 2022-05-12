using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MemeClasses.Items
{
	public class ModGlobalItem : GlobalItem
	{
		public override void SetDefaults(Item item)
		{
			if (MemeClasses.ItemIsRope(item, "coil"))
			{
				item.ammo = 999;
			}
		}

		public override void GrabRange(Item item, Player player, ref int grabRange)
		{
			if (MemeClasses.ItemIsRope(item, "rope") && player.GetModPlayer<PulleyPlayer>().RopeGlove)
			{
				grabRange += 32;
			}
			if (MemeClasses.ItemIsRope(item, "rope") && player.GetModPlayer<PulleyPlayer>().RopeGlove2)
			{
				grabRange += 64;
			}
		}
	}
}