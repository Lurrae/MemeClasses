using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace MemeClasses.Items.Pulleys
{
	public abstract class BasePulley : ModItem
	{
		// NOTE: To anyone creating a pulley, inherit this class INSTEAD of ModItem!
		//		 You'll need to use StaticPulleyDefaults and SetPulleyDefaults, and you do not need to set the research count or damage type of anything! That's all handled here
		//		 - Tepig

		public virtual void StaticPulleyDefaults()
		{
			// NOTE: Items' names and tooltips are automatically set from localization files! Do not set the names in code!
			//		 If you do, any translations we do in the future will break. - Tepig
		}

		public sealed override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; // How many items needed to research?

			StaticPulleyDefaults(); // Because this runs after setting the research count, it's possible to modify that value if needed
		}

		public virtual void SetPulleyDefaults()
		{
		}

		public sealed override void SetDefaults()
		{
			SetPulleyDefaults(); // This runs before setting the damage type, so any different damage type will be overwritten
			
			Item.accessory = true;
			Item.DamageType = ModContent.GetInstance<PulleyDamageClass>();
		}

		public override bool CanEquipAccessory(Player player, int slot, bool modded)
		{
			return slot == ModContent.GetInstance<PulleyAccSlot>().Type;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<PulleyPlayer>().ActivePulley = Item;
		}
	}
}