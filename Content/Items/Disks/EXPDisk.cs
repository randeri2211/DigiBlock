using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DigiBlock.Content.Items.Disks
{
    public class EXPDisk : Disk
    {
        public int expAmount = 0;
        public override void SetStaticDefaults()
        {
            
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            expAmount = 20;
            Item.SetNameOverride("EXP Disk");
            Item.width = 31;
            Item.height = 31;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Green;
        }

        public override void Use()
        {
            digimon.GiveEXP(expAmount);
            base.Use();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "HPDisk", "Gives the digimon "+expAmount+" EXP"));
        }
    }
}
