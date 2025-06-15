using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using DigiBlock.Content.Items.Digimon;

namespace DigiBlock.Content.UI
{
    public class DigiviceUISystem : ModSystem
    {
        private UserInterface digiviceInterface;
        internal DigiviceUI digiviceUI;
        private Digivice openDigivice;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                digiviceUI = new DigiviceUI();
                digiviceUI.Activate();
                digiviceInterface = new UserInterface();
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            digiviceInterface?.Update(gameTime);
            if (openDigivice != null)
            {
                bool accessible = openDigivice.IsItemStillAccessible();
                if (!accessible)
                {
                    CloseUI();
                }
                
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (index != -1)
            {
                layers.Insert(index + 1, new LegacyGameInterfaceLayer(
                    "DigiBlock: Digivice UI",
                    () =>
                    {
                        if (digiviceInterface?.CurrentState != null)
                        {
                            digiviceInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI
                ));
            }
        }

        public void ToggleUI(Digivice digivice)
        {
            if (digiviceInterface.CurrentState == null)
            {
                OpenUI(digivice);
            }
            else
            {
                if (digivice.card != digiviceUI.GetDigiviceItem())
                {
                    OpenUI(digivice);
                }
                else
                {
                    CloseUI();
                }
            }
        }

        public void OpenUI(Digivice digivice)
        {
            digiviceUI.SetDigiviceItem(digivice);
            digiviceInterface?.SetState(digiviceUI);
            openDigivice = digivice;
        }

        public void CloseUI()
        {
            digiviceInterface?.SetState(null);
            openDigivice = null;
        }
    }
}
