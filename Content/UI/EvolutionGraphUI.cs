using Terraria.UI;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria;
using System;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria.ModLoader;
using DigiBlock.Content.Systems;
using System.Text.Json;
using Terraria.ModLoader.UI;
using Microsoft.Xna.Framework.Graphics;
using DigiBlock.Content.Items.Digimon;
using DigiBlock.Content.Digimon;

namespace DigiBlock.Content.UI
{
    public class EvolutionGraphUI : UIState
    {
        private HashSet<Keys> previousKeys = new();
        private UIPanel panel;
        private UIText label;
        private UITextBox searchTextBox;
        private bool textActive = false;
        private float panelWidth = 200f;
        private float panelHeight = 300f;
        Dictionary<ModNPC, JsonElement> evolutions;
        List<ModNPC> devolutions;
        ModNPC searchDigimon;
        public Digivice digivice = null;
        public override void OnInitialize()
        {
            panel = new UIDraggablePanel();
            panel.SetPadding(10);
            panel.Left.Set(400f, 0f);
            panel.Top.Set(100f, 0f);
            panel.Width.Set(panelWidth, 0f);
            panel.Height.Set(panelHeight, 0f);

            label = new UIText("Digimon Evolution Graph");
            panel.Append(label);
            panelWidth = Math.Max(label.GetDimensions().Width + panel.PaddingLeft + panel.PaddingRight, panelWidth);
            panel.Width.Set(panelWidth, 0f);


            searchTextBox = new UITextBox("");
            searchTextBox.SetPadding(5);
            searchTextBox.Width.Set(0f, 1f);
            searchTextBox.Top.Set(label.GetDimensions().Height + searchTextBox.PaddingTop, 0f);
            panel.Append(searchTextBox);

            Append(panel);
        }

        public override void Update(GameTime gameTime)
        {

            UpdateTextBox();

            base.Update(gameTime);
        }

        public void SearchGraph(string digimonName)
        {
            Mod mod = ModContent.GetInstance<DigiBlock>();
            string correctName = digimonName.Length > 0 ? char.ToUpper(digimonName[0]) + digimonName.Substring(1).ToLower() : digimonName;
            Console.WriteLine("searching for  " + correctName);
            var allEvolutions = ModContent.GetInstance<EvolutionSystem>().evolutions;
            Dictionary<ModNPC, JsonElement> tempEvolutions = new Dictionary<ModNPC, JsonElement>();
            List<ModNPC> tempDevolutions = new List<ModNPC>();
            if (allEvolutions.RootElement.TryGetProperty(correctName, out JsonElement digivolutions))
            {
                searchDigimon = mod.Find<ModNPC>(correctName);
                foreach (var digiv in digivolutions.EnumerateArray())
                {
                    Console.WriteLine("adding  " + digiv.GetProperty("Evolution").GetString());
                    tempEvolutions.Add(mod.Find<ModNPC>(digiv.GetProperty("Evolution").GetString()), digiv.GetProperty("Conditions"));
                    Console.WriteLine("added");
                }
            }
            foreach (var digimon in allEvolutions.RootElement.EnumerateObject())
            {
                foreach (var digiv in digimon.Value.EnumerateArray())
                {
                    if (digiv.GetProperty("Evolution").GetString() == correctName)
                    {
                        if (searchDigimon == null)
                        {
                            searchDigimon = mod.Find<ModNPC>(correctName);
                        }
                        Console.WriteLine("adding  devolution " + digimon.Name);
                        tempDevolutions.Add(mod.Find<ModNPC>(digimon.Name));
                        Console.WriteLine("added");
                    }
                }
            }

            if (tempEvolutions.Count > 0)
            {
                Console.WriteLine("swapping evolutions");
                evolutions = tempEvolutions;
            }
            else
            {
                evolutions = null;
            }
            if (tempDevolutions.Count > 0)
            {
                Console.WriteLine("swapping devolutions");
                devolutions = tempDevolutions;
            }
            else
            {
                devolutions = null;
            }

            TriggerGraphDraw();
            searchDigimon = null;
        }

        private void TriggerGraphDraw()
        {
            float offsetY = label.GetDimensions().Height + searchTextBox.GetDimensions().Height;
            float digimonHeight = (panelHeight - offsetY) / 3;
            // Clear graph
            if ((evolutions != null && evolutions.Count > 0) || (devolutions != null && devolutions.Count > 0))
            {
                List<UIElement> toRemove = new List<UIElement>();
                foreach (var element in panel.Children)
                {
                    if (element is CroppedUIImageButton e)
                    {
                        toRemove.Add(e);
                    }
                }
                foreach (UIElement element in toRemove)
                {
                    Console.WriteLine("removing");
                    panel.RemoveChild(element);
                }

            }
            if (evolutions != null && evolutions.Count > 0)
            {
                float digimonWidth = panelWidth / evolutions.Count;
                int counter = 0;
                foreach (var evo in evolutions.Keys)
                {
                    var textureBase = ModContent.Request<Texture2D>(evo.Texture);
                    CroppedUIImageButton evoButton = new CroppedUIImageButton(textureBase, evo.NPC.width, evo.NPC.height, evolutions[evo]);

                    // evoButton.Width.Set(digimonWidth, 0f);
                    float textureWidth = textureBase.Width();
                    evoButton.Width.Set(textureWidth, 0f);
                    evoButton.Height.Set(digimonHeight, 0f);
                    evoButton.Top.Set(offsetY, 0f);
                    evoButton.Left.Set(- textureWidth / 2, (1+counter)/(1.0f+evolutions.Count));

                    evoButton.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
                    {
                        if (digivice == null)
                        {
                            SearchGraph(evo.Name);
                        }
                        else
                        {
                            EvolutionSystem evoSystem = ModContent.GetInstance<EvolutionSystem>();
                            DigimonBase digimon = (digivice.card.ModItem as DigimonCard).digimon;
                            if (evoSystem.CanEvolve(digimon, evolutions[evo]))
                            {
                                evoSystem.TriggerEvolution(digimon, evo.Name);
                            }
                            else
                            {
                                Main.NewText("Cant Evolve to " + evo.Name);
                            }
                        }
                    };

                    panel.Append(evoButton);
                    counter++;
                }

            }
            if (searchDigimon != null)
            {
                var digimonTexture = ModContent.Request<Texture2D>(searchDigimon.Texture);
                CroppedUIImageButton digimonButton = new CroppedUIImageButton(digimonTexture, searchDigimon.NPC.width, searchDigimon.NPC.height);
                float dtextureWidth = digimonTexture.Width();
                digimonButton.Width.Set(dtextureWidth, 0f);
                digimonButton.Height.Set(digimonHeight, 0f);
                digimonButton.Top.Set(offsetY + digimonHeight, 0f);
                digimonButton.Left.Set(-dtextureWidth / 2, 0.5f);
                panel.Append(digimonButton);
            }
            

            if (devolutions != null && devolutions.Count > 0)
            {
                float digimonWidth = panelWidth / devolutions.Count;
                int counter = 0;
                foreach (var devo in devolutions)
                {
                    Console.WriteLine("devolution" + devo.Name);
                    var textureBase = ModContent.Request<Texture2D>(devo.Texture);
                    CroppedUIImageButton devoButton = new CroppedUIImageButton(textureBase, devo.NPC.width, devo.NPC.height);
                    float textureWidth = textureBase.Width();

                    // evoButton.Width.Set(digimonWidth, 0f);
                    devoButton.Width.Set(textureWidth, 0f);
                    devoButton.Height.Set(digimonHeight, 0f);
                    devoButton.Top.Set(offsetY + 2 * digimonHeight, 0f);
                    devoButton.Left.Set(- textureWidth / 2, (1+counter)/(1.0f+devolutions.Count));

                    devoButton.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
                    {
                        if (digivice == null)
                        {
                            SearchGraph(devo.Name);
                        }
                        else
                        {
                            EvolutionSystem evoSystem = ModContent.GetInstance<EvolutionSystem>();
                            DigimonBase digimon = (digivice.card.ModItem as DigimonCard).digimon;
                            evoSystem.TriggerEvolution(digimon, devo.Name);
                        }
                    };

                    panel.Append(devoButton);
                    counter++;
                }
            }
        }

        private void UpdateTextBox()
        {
            if (Main.mouseLeft && searchTextBox.IsMouseHovering)
            {
                textActive = true;
            }
            else if (Main.mouseLeft && !searchTextBox.IsMouseHovering)
            {
                Main.blockInput = false;
                textActive = false;
            }

            if (textActive)
            {

                var currentKeys = Main.keyState.GetPressedKeys();

                foreach (Keys key in currentKeys)
                {
                    if (!previousKeys.Contains(key)) // Key was just pressed
                    {
                        string typedChar = GetCharFromKey(key);
                        if (!string.IsNullOrEmpty(typedChar))
                        {
                            searchTextBox.Write(typedChar);
                        }

                        if (key == Keys.Back)
                        {
                            searchTextBox.Backspace();
                        }
                        if (key == Keys.Enter)
                        {
                            SearchGraph(searchTextBox.Text);
                        }
                    }
                }

                // Update previousKeys for next frame
                previousKeys = new HashSet<Keys>(currentKeys);
                Main.blockInput = true;
            }
        }

        private string GetCharFromKey(Keys key)
        {
            // Simple version (expand as needed)
            if (key >= Keys.A && key <= Keys.Z)
            {
                bool shift = Main.keyState.IsKeyDown(Keys.LeftShift) || Main.keyState.IsKeyDown(Keys.RightShift);
                char c = (char)(key - Keys.A + (shift ? 'A' : 'a'));
                return c.ToString();
            }

            if (key >= Keys.D0 && key <= Keys.D9)
            {
                return ((char)('0' + (key - Keys.D0))).ToString();
            }

            return "";
        }


    }
}
