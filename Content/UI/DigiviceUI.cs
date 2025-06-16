using Terraria.UI;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using System;
using Terraria;
using DigiBlock.Content.Items.Digimon;
using System.Collections.Generic;
using Terraria.ModLoader.UI;
using DigiBlock.Content.Digimon.Ability;


namespace DigiBlock.Content.UI
{
    public class DigiviceUI : UIState
    {
        private UIPanel panel;
        private MYUIItemSlot cardSlot;
        private float defaultPanelWidth = 100f;
        private float defaultPanelHeight = 100f;
        private List<UIButton<string>> buttonList = new List<UIButton<string>>();
        private UIDiskSlot diskSlot;
        UIText dataUI;
        public override void OnInitialize()
        {
            panel = new UIDraggablePanel();
            diskSlot = null;
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
            if (dataUI != null)
            {
                panel.RemoveChild(dataUI);
            }
            foreach (UIButton<string> button in buttonList)
            {
                panel.RemoveChild(button);
            }
            if (!cardSlot.digivice.card.IsAir && cardSlot.digivice.card.ModItem is DigimonCard digimonCard)
            {
                // Has DigimonCard
                if (diskSlot == null) // Only on the first digimon insertion
                {
                    initDiskSlot();
                }

                string text = "";
                text += "Name: " + digimonCard.digimon.name + "\n";
                text += "Level: " + digimonCard.digimon.level + "\n";
                text += "HP: " + digimonCard.digimon.NPC.life + "/" + digimonCard.digimon.NPC.lifeMax + "\n";
                text += "Exp: " + digimonCard.digimon.getEXP() + "/" + digimonCard.digimon.maxEXP + "\n";
                text += "Contact Digital Damage: " + digimonCard.digimon.CalculateDamage(digimonCard.digimon.physicalDamage) + "\n";
                text += "Special Digital Damage: " + digimonCard.digimon.CalculateDamage(digimonCard.digimon.specialDamage) + "\n";
                text += "Agility: " + digimonCard.digimon.agility + "\n";
                text += "Defense: " + digimonCard.digimon.NPC.defense + "\n";
                text += "Digimon Type: " + digimonCard.digimon.Name + "\n";
                text += "Digimon Attribute: " + digimonCard.digimon.attribute + "\n";
                foreach (var biome in digimonCard.digimon.biomeKills.Keys)
                {
                    text += biome + ": " + digimonCard.digimon.biomeKills[biome] + "\n";
                }
                dataUI = new UIText(text);
                dataUI.Top.Set(cardSlot.GetDimensions().Height + cardSlot.Top.Pixels, 0f);
                panel.Append(dataUI);

                Vector2 textSize = Terraria.GameContent.FontAssets.MouseText.Value.MeasureString(text);
                float padding = panel.PaddingLeft + panel.PaddingRight;

                float buttonHeight = 0;
                for (int i = 0; i < digimonCard.digimon.specialAbilities.Count; i++)
                {
                    DigiAbility ability = digimonCard.digimon.specialAbilities[i];
                    UIButton<string> button = new UIButton<string>(ability.GetType().Name);
                    // button.TooltipText = true;
                    button.HoverText = ability.tooltip;
                    button.AltHoverText = ability.tooltip;
                    button.Top.Set(buttonHeight + textSize.Y + dataUI.Top.Pixels - panel.PaddingBottom - panel.PaddingTop, 0f);
                    // button.Left.Set(panel.PaddingLeft, 0f);
                    Vector2 bDim = Terraria.GameContent.FontAssets.MouseText.Value.MeasureString(button.HoverText);
                    button.Width.Set(bDim.X,0f);
                    button.Height.Set(bDim.Y,0f);
                    buttonHeight += bDim.Y;
                    if (digimonCard.digimon.specialAbilityIndex == i)
                    {
                        button.BackgroundColor = Color.CornflowerBlue * 0.8f;
                        button.BorderColor = Color.LightBlue;
                    }
                    else
                    {
                        button.BackgroundColor = Color.DarkSlateGray * 0.6f;
                        button.BorderColor = Color.Gray;
                    }

                    int index = i;
                    button.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
                    {
                        digimonCard.digimon.specialAbilityIndex = index;
                        // Console.WriteLine("" + digimonCard.digimon.specialAbilityIndex);

                    };

                    buttonList.Add(button);
                    panel.Append(button);
                    button.Recalculate();
                }

                // Console.WriteLine("button height: " + buttonHeight);
                

                panel.Width.Set(Math.Max(textSize.X + padding, defaultPanelWidth), 0f);
                panel.Height.Set(cardSlot.Top.Pixels + cardSlot.GetInnerDimensions().Height + textSize.Y + buttonHeight, 0f);
                dataUI.Recalculate();
                panel.Recalculate();
            }
            else
            {
                if (diskSlot != null)
                {
                    panel.RemoveChild(diskSlot);
                    diskSlot = null;
                }
                // No DigimonCard
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
            cardSlot = new MYUIItemSlot(digivice);
            cardSlot.Left.Set(0f, 0f);
            cardSlot.Top.Set(0f, 0f);
            panel.Append(cardSlot);


            initDiskSlot();
        }

        private void initDiskSlot()
        {
            diskSlot = new UIDiskSlot(cardSlot.digivice);
            diskSlot.Left.Set(-diskSlot.Width.Pixels / 2 + panel.PaddingLeft, 0.5f);
            diskSlot.Top.Set(0f, 0f);
            panel.Append(diskSlot);
        }

        public Item GetDigiviceItem()
        {
            return cardSlot.digivice.card;
        }
    }
}
