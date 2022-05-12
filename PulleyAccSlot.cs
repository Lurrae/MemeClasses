using MemeClasses.Items.Pulleys;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace MemeClasses
{
	public class PulleyAccSlot : ModAccessorySlot
	{
		public override string FunctionalTexture => "MemeClasses/Pulleys_Icon";
		public override bool DrawVanitySlot => false; // Vanilla equipment doesn't have vanity slots, so neither should pulleys
		
		// Use background images #3 and #12 for all equipment slots we add! They're the same as the vanilla ones afaik
		public override string FunctionalBackgroundTexture => "Terraria/Images/Inventory_Back3";
		public override string DyeBackgroundTexture => "Terraria/Images/Inventory_Back12";

		// Hides the slot when not in the equipment tab of the inventory
		public override bool IsHidden()
		{
			return Main.EquipPageSelected != 2;
		}

		public override bool IsVisibleWhenNotEnabled()
		{
			return false;
		}

		public override Vector2? CustomLocation => new Vector2(Main.screenWidth - 92, Main.screenHeight - 600 + 340);

		public override bool CanAcceptItem(Item checkItem, AccessorySlotType context)
		{
			if ((context == AccessorySlotType.DyeSlot && checkItem.dye > 0) ||
				(context == AccessorySlotType.FunctionalSlot && checkItem.CountsAsClass(GetInstance<PulleyDamageClass>())))
				return true;

			return false;
		}

		public override bool ModifyDefaultSwapSlot(Item item, int accSlotToSwapTo)
		{
			if (item.CountsAsClass(GetInstance<PulleyDamageClass>()))
				return true;

			return false;
		}

		public override void OnMouseHover(AccessorySlotType context)
		{
			switch (context)
			{
				case AccessorySlotType.DyeSlot:
					Main.hoverItemName = "Dye";
					break;
				case AccessorySlotType.FunctionalSlot:
					Main.hoverItemName = "Pulley";
					break;
			}
		}
	}
}