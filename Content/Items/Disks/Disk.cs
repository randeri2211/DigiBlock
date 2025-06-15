using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DigiBlock.Content.Systems;
using System.Collections.Generic;
using DigiBlock.Content.Damage;
using DigiBlock.Content.Digimon;

namespace DigiBlock.Content.Items.Disks
{
    public abstract class Disk : ModItem
    {
        public DigimonBase digimon;
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 31;
            Item.height = 31;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Green;
            Item.maxStack = 99;
        }
    }
}
