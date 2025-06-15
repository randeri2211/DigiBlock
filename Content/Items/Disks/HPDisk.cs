using Terraria;
using Terraria.ID;

namespace DigiBlock.Content.Items.Disks
{
    public class HPDisk : Disk
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 31;
            Item.height = 31;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Green;
        }
    }
}
