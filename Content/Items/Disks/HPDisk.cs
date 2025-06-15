using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DigiBlock.Content.Items.Disks
{
    public class HPDisk : Disk
    {
        public int healAmount = 0;
        public override void SetStaticDefaults()
        {
            
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            healAmount = 20;
            Item.SetNameOverride("HPDisk");
            Item.width = 31;
            Item.height = 31;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Green;
        }

        public override void Use()
        {
            digimon.NPC.life = Math.Min(digimon.NPC.lifeMax, digimon.NPC.life + healAmount);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "HPDisk", "Heals the digimon by "+healAmount+" HP if lacking atleast that amount"));
        }
    }
}
