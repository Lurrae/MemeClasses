using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MemeClasses
{
	public class MemeClasses : Mod
	{
		public override void AddRecipeGroups()
		{
			// "Any Rope"
			RecipeGroup group = new(() => Language.GetTextValue("LegacyMisc.37") + " " + Language.GetTextValue("ItemName.Rope"), new int[]
			{
				ItemID.Rope,
				ItemID.SilkRope,
				ItemID.VineRope,
				ItemID.WebRope
			});
			RecipeGroup.RegisterGroup("MemeClasses:Ropes", group);

			// "Any Rope Coil"
			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Language.GetTextValue("ItemName.RopeCoil"), new int[]
			{
				ItemID.RopeCoil,
				ItemID.SilkRopeCoil,
				ItemID.VineRopeCoil,
				ItemID.WebRopeCoil
			});
			RecipeGroup.RegisterGroup("MemeClasses:RopeCoils", group);
		}

		public static bool ItemIsRope(Item item, string type)
		{
			return (type == "coil" && (item.type == ItemID.RopeCoil || item.type == ItemID.SilkRopeCoil || item.type == ItemID.VineRopeCoil || item.type == ItemID.WebRopeCoil)) ||
				(type == "rope" && (item.type == ItemID.Rope || item.type == ItemID.SilkRope || item.type == ItemID.VineRope || item.type == ItemID.WebRope));
		}
	}
}