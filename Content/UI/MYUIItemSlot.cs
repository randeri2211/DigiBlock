using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using DigiBlock.Content.Items.Digimon;
using Terraria.ID;
using DigiBlock.Content.Digimon;

namespace DigiBlock.Content.UI
{
    public class MYUIItemSlot : UIElement
    {
        public Digivice digivice;
        public Item item = new Item(); // starts as "air"

        public MYUIItemSlot(Digivice digivice)
        {
            this.digivice = digivice;
            Width.Set(50f, 0f);
            Height.Set(50f, 0f);
            this.item = digivice.item;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            CalculatedStyle dimensions = GetInnerDimensions();
            Vector2 slotPosition = dimensions.Position();

            Rectangle slotRect = new((int)slotPosition.X, (int)slotPosition.Y, 50, 50);
            bool hovering = slotRect.Contains(Main.mouseX, Main.mouseY);

            if (hovering && (Main.mouseItem.type == ItemID.None || Main.mouseItem.ModItem is DigimonCard digimonCard))
            {
                Main.LocalPlayer.mouseInterface = true;
                ItemSlot.MouseHover(ref digivice.item, ItemSlot.Context.InventoryItem);
                ItemSlot.LeftClick(ref digivice.item, ItemSlot.Context.InventoryItem);
                ItemSlot.RightClick(ref digivice.item, ItemSlot.Context.InventoryItem);

                bool wasAir = item.IsAir;
                bool isAir = digivice.item.IsAir;

                // Now compare with previous state
                if (!Item.Equals(item, digivice.item))
                {
                    // Case: Removed
                    if (!wasAir && isAir)
                    {
                        removeCard(item);
                    }
                    // Case: Inserted
                    else if (wasAir && !isAir)
                    {
                        addCard();
                    }
                    // Case: Swapped
                    else if (!wasAir && !isAir)
                    {
                        removeCard(item);
                        addCard().digivice = digivice;
                    }
                    item = digivice.item;
                }
            }

            ItemSlot.Draw(spriteBatch, ref digivice.item, ItemSlot.Context.HotbarItem, dimensions.Position());
        }

        public void removeCard(Item item)
        {
            DigimonCard c = item.ModItem as DigimonCard;
            c.digimon.NPC.active = false;
            c.digivice = null;
        }

        public DigimonCard addCard()
        {
            DigimonCard c = digivice.item.ModItem as DigimonCard;
            Player player = Main.LocalPlayer;
            int newNpcId = NPC.NewNPC(null, (int)player.position.X, (int)player.position.Y, c.digimon.Type);
            if (Main.npc[newNpcId].ModNPC is DigimonBase digiNPC)
            {
                digiNPC.copyData(c.digimon);
                digiNPC.NPC.active = true;
                c.digimon = digiNPC;
                c.digivice = digivice;
                c.digimon.CalculateStats();
            }
            return c;
        }
    }
}
