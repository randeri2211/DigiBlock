using Terraria.UI;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using DigiBlock.Content.Items;
using System;
using Terraria;
using DigiBlock.Content.Items.Digimon;
using System.Collections.Generic;


namespace DigiBlock.Content.UI
{
    public class DigiviceUI : UIState
    {
        private UIPanel panel;
        private MYUIItemSlot itemSlot;
        private float defaultPanelWidth = 100f;
        private float defaultPanelHeight = 100f;
        public override void OnInitialize()
        {
            panel = new UIDraggablePanel();

            panel.SetPadding(10);
            panel.Left.Set(0f, 0.5f);
            panel.Top.Set(0f, 0.2f);
            panel.Width.Set(defaultPanelWidth, 0f);
            panel.Height.Set(defaultPanelHeight, 0f);
            Append(panel);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            List<UIElement> toRemove = new List<UIElement>();
            foreach (var element in panel.Children)
            {
                if (element is UIText)
                {
                    toRemove.Add(element);
                }
            }
            foreach (var element in toRemove)
            {
                panel.RemoveChild(element);
            }
            if (!itemSlot.item.IsAir && itemSlot.item.ModItem is DigimonCard digimonCard)
            {

                string text = "";
                text += "Name: " + digimonCard.digimon.name + "\n";
                text += "Level: " + digimonCard.digimon.level + "\n";
                text += "HP: " + digimonCard.digimon.NPC.life + "/" + digimonCard.digimon.NPC.lifeMax + "\n";
                text += "Exp: " + digimonCard.digimon.getEXP() + "/" + digimonCard.digimon.maxEXP + "\n";
                text += "Contact Digital Damage: " + digimonCard.digimon.CalculateDamage(digimonCard.digimon.contactDamage) + "\n";
                text += "Special Digital Damage: " + digimonCard.digimon.CalculateDamage(digimonCard.digimon.specialDamage) + "\n";
                text += "Agility: " + digimonCard.digimon.agility + "\n";
                text += "Digimon Type: " + digimonCard.digimon.Name + "\n";
                text += "Digimon Attribute: " + digimonCard.digimon.attribute + "\n";
                UIText dataUI = new UIText(text);
                dataUI.Top.Set(itemSlot.GetDimensions().Height + itemSlot.Top.Pixels, 0f);
                // Console.WriteLine("height " + dataUI.GetInnerDimensions().Height + "percent" + dataUI.Height.Percent);
                // Console.WriteLine("width " + dataUI.GetInnerDimensions().Width + "percent" + dataUI.Width.Pixels);
                // panel.Height.Set(itemSlot.GetInnerDimensions().Height + itemSlot.Top.Pixels + dataUI.GetInnerDimensions().Height,0f);
                // panel.Width.Set(Math.Max(dataUI.GetInnerDimensions().Width + panel.PaddingLeft + panel.PaddingRight, defaultPanelWidth),0f);
                panel.Append(dataUI);
                dataUI.Recalculate();
                panel.Recalculate();


                Vector2 textSize = Terraria.GameContent.FontAssets.MouseText.Value.MeasureString(text);
                float padding = panel.PaddingLeft + panel.PaddingRight;

                panel.Width.Set(Math.Max(textSize.X + padding, defaultPanelWidth), 0f);
                panel.Height.Set(itemSlot.Top.Pixels + itemSlot.GetInnerDimensions().Height + textSize.Y, 0f);
            }
            else
            {
                if (panel.GetDimensions().Width != defaultPanelWidth || panel.GetDimensions().Height != defaultPanelHeight)
                {
                    panel.Width.Set(defaultPanelWidth, 0f);
                    panel.Height.Set(defaultPanelHeight, 0f);
                }
            }
        }

        public void SetDigiviceItem(Digivice digivice)
        {
            panel.RemoveAllChildren();
            itemSlot = new MYUIItemSlot(digivice);
            itemSlot.Left.Set(-itemSlot.Width.Pixels / 2 + panel.PaddingLeft, 0.5f);
            itemSlot.Top.Set(25f, 0f);
            panel.Append(itemSlot);
        }

        public Item GetDigiviceItem()
        {
            return itemSlot.digivice.item;
        }
    }
}
