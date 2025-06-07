using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using DigiBlock.Content.Items;
using System;
using Terraria.GameContent.UI.Elements;
using System.Collections.Generic;

namespace DigiBlock.Content.UI
{
    public class MYUIItemSlot : UIElement
    {
        public Digivice digivice;

        public MYUIItemSlot(Digivice digivice)
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

            if (hovering)
            {
                Main.LocalPlayer.mouseInterface = true;
                ItemSlot.MouseHover(ref digivice.item, ItemSlot.Context.InventoryItem);
                ItemSlot.LeftClick(ref digivice.item, ItemSlot.Context.InventoryItem);
                ItemSlot.RightClick(ref digivice.item, ItemSlot.Context.InventoryItem); 
            }
            
            ItemSlot.Draw(spriteBatch, ref digivice.item, ItemSlot.Context.ShopItem, dimensions.Position());
        }
    }
}
