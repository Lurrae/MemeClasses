using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
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

	public class ModPulleyDrawLayer : PlayerDrawLayer
	{
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			PulleyPlayer pPlr = drawInfo.drawPlayer.GetModPlayer<PulleyPlayer>();

			return drawInfo.drawPlayer.pulley && pPlr.ActivePulley != null && !ModContent.GetInstance<PulleyAccSlot>().HideVisuals;
		}

		public override Position GetDefaultPosition() => new Between(PlayerDrawLayers.Pulley, PlayerDrawLayers.Shield);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			PulleyPlayer pPlr = drawInfo.drawPlayer.GetModPlayer<PulleyPlayer>();
			Item activePulley = pPlr.ActivePulley;
			string textureLoc = activePulley.ModItem.Texture + "_Pulley";
			Asset<Texture2D> texture = ModContent.Request<Texture2D>(textureLoc);
			
			int frame = drawInfo.drawPlayer.pulleyFrame;
			int pulleyDir = drawInfo.drawPlayer.pulleyDir;
			float gravDir = drawInfo.drawPlayer.gravDir;
			int dir = drawInfo.drawPlayer.direction;

			int num = (pulleyDir == 2) ? 0 : 10;
			int num2 = (pulleyDir == 2) ? -25 : -26;
			float rot = (pulleyDir == 2) ? 0f : -0.35f * dir;
			Vector2 vector = new(drawInfo.drawPlayer.width / 2, drawInfo.drawPlayer.height / 2);
			Vector2 vector2 = new((int)(drawInfo.Position.X - Main.screenPosition.X + vector.X - (9 * dir)) + num * dir, (int)(drawInfo.Position.Y - Main.screenPosition.Y + vector.Y + 2f * gravDir + num2 * gravDir));

			drawInfo.DrawDataCache.Add(new DrawData(
				texture.Value,
				vector2,
				new Rectangle?(new Rectangle(0, texture.Height() / 2 * frame, texture.Width(), texture.Height() / 2)),
				drawInfo.colorArmorHead,
				rot,
				new Vector2(texture.Width() / 2, texture.Height() / 4),
				1f,
				drawInfo.playerEffect,
				0
			));
		}
	}
}