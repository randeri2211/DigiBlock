using Terraria.UI;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using System;
using Terraria;
using DigiBlock.Content.Items.Digimon;
using System.Collections.Generic;
using Terraria.ModLoader.UI;
using DigiBlock.Content.Digimon.Ability;
using Terraria.ModLoader;
using DigiBlock.Content.Systems;


namespace DigiBlock.Content.UI
{
    public class DigiviceUI : UIState
    {
        private UIPanel panel;
        private MYUIItemSlot cardSlot;
        private float defaultPanelWidth = 100f;
        private float defaultPanelHeight = 100f;
        private List<UIButton<string>> buttonList = new List<UIButton<string>>();
        private float buttonHeight = 0;
        UIButton<string> evoButton;
        private UIDiskSlot diskSlot;
        UIText dataUI;
        internal UserInterface EvolutionGraphUIInterface;

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

            EvolutionGraphUIInterface = ModContent.GetInstance<EvolutionGraphUISystem>().EvolutionGraphUIInterface;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (dataUI != null)
            {
                panel.RemoveChild(dataUI);
            }
            if (!cardSlot.digivice.card.IsAir && cardSlot.digivice.card.ModItem is DigimonCard digimonCard)
            {
                // Has DigimonCard
                if (diskSlot == null) // Only on the first digimon insertion
                {
                    initDiskSlot();
                }

                // Data on the digimon
                string text = initText(digimonCard);

                Vector2 textSize = Terraria.GameContent.FontAssets.MouseText.Value.MeasureString(text);
                float padding = panel.PaddingLeft + panel.PaddingRight;

                // Special ability buttons
                if (buttonList.Count == 0)
                {
                    buttonHeight = 0;
                    for (int i = 0; i < digimonCard.digimon.specialAbilities.Count; i++)
                    {
                        buttonHeight = initSpecialAbilityButton(digimonCard, i, buttonHeight, textSize.Y);
                    }
                }

                // Evolution Graph button
                if (evoButton == null)
                {
                    initEvoButton(digimonCard);
                }

                panel.Width.Set(Math.Max(textSize.X, 2 * evoButton.Width.Pixels + diskSlot.Width.Pixels) + padding, 0f);
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
                if (evoButton != null)
                {
                    panel.RemoveChild(evoButton);
                    evoButton = null;
                }
                buttonHeight = 0;
                foreach (var button in buttonList)
                {
                    panel.RemoveChild(button);
                    buttonList = new List<UIButton<string>>();
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
            evoButton = null;
            buttonList = new List<UIButton<string>>();
            buttonHeight = 0;

            initDiskSlot();
        }

        private void initDiskSlot()
        {
            diskSlot = new UIDiskSlot(cardSlot.digivice);
            diskSlot.Left.Set(-diskSlot.Width.Pixels / 2 + panel.PaddingLeft, 0.5f);
            diskSlot.Top.Set(0f, 0f);
            panel.Append(diskSlot);
        }

        private string initText(DigimonCard digimonCard)
        {
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
            return text;
        }

        private float initSpecialAbilityButton(DigimonCard digimonCard, int i, float buttonHeight, float textHeight)
        {
            DigiAbility ability = digimonCard.digimon.specialAbilities[i];
            UIButton<string> button = new UIButton<string>(ability.name);
            button.TooltipText = true;
            button.HoverText = ability.tooltip;
            button.Top.Set(buttonHeight + textHeight + dataUI.Top.Pixels - panel.PaddingBottom - panel.PaddingTop, 0f);
            Vector2 bDim = Terraria.GameContent.FontAssets.MouseText.Value.MeasureString(button.HoverText);
            button.Width.Set(bDim.X, 0f);
            button.Height.Set(bDim.Y, 0f);
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
            };

            buttonList.Add(button);
            panel.Append(button);
            button.Recalculate();
            return buttonHeight;
        }

        public void initEvoButton(DigimonCard digimonCard)
        {
            evoButton = new UIButton<string>("Digivolutions");
            Vector2 evoTextSize = Terraria.GameContent.FontAssets.MouseText.Value.MeasureString(evoButton.Text);
            evoButton.TooltipText = true;
            evoButton.HoverText = "Digivolutions";
            evoButton.Width.Set(evoTextSize.X, 0f);
            evoButton.Height.Set(evoTextSize.Y, 0f);
            evoButton.Left.Set(-evoTextSize.X, 1.0f);
            evoButton.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
            {

                if (EvolutionGraphUIInterface.CurrentState != null)
                {
                    EvolutionGraphUIInterface.SetState(null);
                }
                EvolutionGraphUI evoGraphUI = new EvolutionGraphUI();
                evoGraphUI.digivice = cardSlot.digivice;
                EvolutionGraphUIInterface.SetState(evoGraphUI);
                ModContent.GetInstance<EvolutionGraphUISystem>().setVisible(true);
                evoGraphUI.SearchGraph(digimonCard.digimon.name);
            };
            panel.Append(evoButton);
        }

        public Item GetDigiviceItem()
        {
            return cardSlot.digivice.card;
        }
    }
}
