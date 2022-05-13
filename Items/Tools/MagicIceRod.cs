using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MemeClasses.Items
{
    internal class MagicIceRod : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.IceRod);
        }
    }
}
