using Terraria.UI;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using DigiBlock.Content.Items;
using System;
using Terraria;


namespace DigiBlock.Content.UI
{
    public class DigiviceUI : UIState
    {
        private UIPanel panel;
        private MYUIItemSlot itemSlot;
        private UIItemSlot realItemSlot;
        public override void OnInitialize()
        {
            panel = new UIDraggablePanel();

            panel.SetPadding(10);
            panel.Left.Set(400f, 0f);
            panel.Top.Set(200f, 0f);
            panel.Width.Set(100f, 0f);
            panel.Height.Set(100f, 0f);
            Append(panel);
        }

        public void SetDigiviceItem(Digivice digivice)
        {
            panel.RemoveAllChildren();
            itemSlot = new MYUIItemSlot(digivice);
            itemSlot.Left.Set(25f, 0f);
            itemSlot.Top.Set(25f, 0f);
            panel.Append(itemSlot);
        }

        public Item GetDigiviceItem()
        {
            return itemSlot.digivice.item;
        }
    }
}
