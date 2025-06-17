using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using DigiBlock.Content.UI;

namespace DigiBlock.Content.Systems
{
    public class EvolutionGraphUISystem : ModSystem
    {
        internal UserInterface EvolutionGraphUIInterface;
        internal EvolutionGraphUI evolutionGraphUI;
        private bool visible = false;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                evolutionGraphUI = new EvolutionGraphUI();
                evolutionGraphUI.Activate();
                EvolutionGraphUIInterface = new UserInterface();
                EvolutionGraphUIInterface.SetState(evolutionGraphUI);
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (visible)
            {
                EvolutionGraphUIInterface?.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int inventoryLayerIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (inventoryLayerIndex != -1)
            {
                layers.Insert(inventoryLayerIndex, new LegacyGameInterfaceLayer(
                    "DigiBlock: Digimon UI",
                    () =>
                    {
                        if (visible)
                            EvolutionGraphUIInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public override void PostUpdateInput()
        {
            if (DigiBlock.OpenEvolutionGraphUIKeybind.JustPressed)
            {
                visible = !visible;
                if (visible)
                    EvolutionGraphUIInterface?.SetState(evolutionGraphUI);
                else
                    EvolutionGraphUIInterface?.SetState(null);
            }
        }

        public void setVisible(bool visible)
        {
            this.visible = visible;
        }
    }
}
