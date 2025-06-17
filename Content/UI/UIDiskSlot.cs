using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using DigiBlock.Content.Items.Digimon;
using Terraria.ID;
using DigiBlock.Content.Digimon;
using DigiBlock.Content.Items.Disks;

namespace DigiBlock.Content.UI
{
    public class UIDiskSlot : UIElement
    {
        public Digivice digivice;
        private Item lastItem = new Item();

        public UIDiskSlot(Digivice digivice)
        {
            this.digivice = digivice;
            Width.Set(50f, 0f);
            Height.Set(50f, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            CalculatedStyle dimensions = GetInnerDimensions();
            Vector2 slotPosition = dimensions.Position();

            Rectangle slotRect = new((int)slotPosition.X, (int)slotPosition.Y, 50, 50);
            bool hovering = slotRect.Contains(Main.mouseX, Main.mouseY);

            if (hovering && (Main.mouseItem.type == ItemID.None || Main.mouseItem.ModItem is Disk disk))
            {
                Main.LocalPlayer.mouseInterface = true;
                ItemSlot.MouseHover(ref digivice.disk, ItemSlot.Context.InventoryItem);
                ItemSlot.LeftClick(ref digivice.disk, ItemSlot.Context.InventoryItem);
                ItemSlot.RightClick(ref digivice.disk, ItemSlot.Context.InventoryItem);

                bool wasAir = lastItem.IsAir;
                bool isAir = digivice.disk.IsAir;

                // Now compare with previous state
                if (!Item.Equals(digivice.disk, lastItem))
                {
                    // Case: Removed
                    if (!wasAir && isAir)
                    {
                        removeDisk(lastItem);
                    }
                    // Case: Inserted
                    else if (wasAir && !isAir)
                    {
                        addDisk();
                    }
                    // Case: Swapped
                    else if (!wasAir && !isAir)
                    {
                        removeDisk(lastItem);
                        addDisk();
                    }
                }
                lastItem = digivice.disk;
            }

            ItemSlot.Draw(spriteBatch, ref digivice.disk, ItemSlot.Context.HotbarItem, dimensions.Position());
        }

        public void removeDisk(Item item)
        {
            Disk disk = item.ModItem as Disk;
            disk.digimon = null;
        }

        public void addDisk()
        {
            if(digivice.disk.ModItem is Disk disk)
            {
                DigimonBase digimon = (digivice.card.ModItem as DigimonCard).digimon;
                disk.digimon = digimon;
            }
        }
    }
}
