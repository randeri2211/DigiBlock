using Terraria.UI;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria;
using System;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria.ModLoader;
using DigiBlock.Content.Systems;

namespace DigiBlock.Content.UI
{
    public class EvolutionGraphUI : UIState
    {
        private HashSet<Keys> previousKeys = new();
        private UIPanel panel;
        private UITextBox searchTextBox;
        private bool textActive = false;
        public override void OnInitialize()
        {
            panel = new UIDraggablePanel();
            panel.SetPadding(10);
            panel.Left.Set(400f, 0f);
            panel.Top.Set(100f, 0f);
            panel.Width.Set(300f, 0f);
            panel.Height.Set(200f, 0f);

            UIText label = new UIText("Digimon Evolution Graph");
            panel.Append(label);

            searchTextBox = new UITextBox("");
            searchTextBox.SetPadding(5);
            searchTextBox.Width.Set(0f, 0.7f);
            searchTextBox.Height.Set(30f, 0f);
            searchTextBox.Top.Set(25f, 0f);
            panel.Append(searchTextBox);

            Append(panel);
        }

        public override void Update(GameTime gameTime)
        {

            UpdateTextBox();

            base.Update(gameTime);
        }

        private void SearchGraph(string digimonName)
        {
            digimonName = digimonName.Length > 0 ? char.ToUpper(digimonName[0]) + digimonName.Substring(1).ToLower() : digimonName;
            Console.WriteLine("searching for  " + digimonName);
            var allEvolutions = ModContent.GetInstance<EvolutionSystem>().evolutions;
            foreach (var evolution in allEvolutions.RootElement.EnumerateObject())
            {
                Console.WriteLine("looking at  " + evolution.Name);
                if (evolution.Name == digimonName)
                {
                    Console.WriteLine("found evolution " + evolution.Name);
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
