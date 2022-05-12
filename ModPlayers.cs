using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MemeClasses
{
	public class PulleyPlayer : ModPlayer
	{
		public float PulleySpeed;
		public int BonusRopeRange;
		public bool RopeGlove; // Magnetism effect
		public bool RopeGlove2; // Magnetism effect, but stronger

		public override void ResetEffects()
		{
			PulleySpeed = 1f;
			BonusRopeRange = 0;
			RopeGlove = false;
			RopeGlove2 = false;
		}

		public override void UpdateEquips()
		{
			foreach (Item item in Player.inventory)
			{
				if (MemeClasses.ItemIsRope(item, "rope"))
				{
					item.tileBoost = 3 + BonusRopeRange; // We can't do += here because tileBoost does not reset every frame, so it would just increase infinitely
				}
			}
		}
	}
}