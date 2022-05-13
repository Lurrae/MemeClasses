using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MangoTech.Content.Items
{
    internal class MagicIceRod : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.IceRod);
        }
    }
}
