using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using DigiBlock.Content.Items;
using System;
using Terraria.GameContent.UI.Elements;
using DigiBlock.Content.Items.Digimon;
using Terraria.ID;
using DigiBlock.Content.Digimon;
using DigiBlock.Common;

namespace DigiBlock.Content.UI
{
    public class MYUIItemSlot : UIElement
    {
        public Digivice digivice;
        private Item previousItem = new Item(); // starts as "air"

        public MYUIItemSlot(Digivice digivice)
        {
            this.digivice = digivice;
            Width.Set(50f, 0f);
            Height.Set(50f, 0f);
            this.previousItem = digivice.item;
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

                bool wasAir = previousItem.IsAir;
                bool isAir = digivice.item.IsAir;

                // Now compare with previous state
                if (!Item.Equals(previousItem, digivice.item))
                {
                    // Case: Removed
                    if (!wasAir && isAir)
                    {
                        DigimonCard c = previousItem.ModItem as DigimonCard;
                        c.digimon.NPC.active = false;
                        c.digivice = null;
                    }
                    // Case: Inserted
                    else if (wasAir && !isAir)
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
                        }
                    }
                    // Case: Swapped
                    else if (!wasAir && !isAir)
                    {
                        DigimonCard c1 = previousItem.ModItem as DigimonCard;
                        c1.digimon.NPC.active = false;
                        c1.digivice = null;
                        DigimonCard c2 = digivice.item.ModItem as DigimonCard;
                        Player player = Main.LocalPlayer;//TODO:Change to check the player
                        int newNpcId = NPC.NewNPC(null, (int)player.position.X, (int)player.position.Y, c2.digimon.Type);
                        if (Main.npc[newNpcId].ModNPC is DigimonBase digiNPC)
                        {
                            digiNPC.copyData(c2.digimon);
                            digiNPC.NPC.active = true;
                            c2.digimon = digiNPC;
                        }
                        c2.digivice = digivice;
                    }
                    previousItem = digivice.item;
                }
            }

            ItemSlot.Draw(spriteBatch, ref digivice.item, ItemSlot.Context.ShopItem, dimensions.Position());
        }
    }
}
