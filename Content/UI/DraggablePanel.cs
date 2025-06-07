using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria.GameContent.UI.Elements;

namespace DigiBlock.Content.UI
{
    public class UIDraggablePanel : UIPanel
    {
        private Vector2 offset;
        private bool dragging;
        public override void LeftMouseDown(UIMouseEvent evt)
        {
            base.LeftMouseDown(evt);
            // Only start dragging if mouse is NOT over any child elements
            if (!IsMouseOverChild(evt.MousePosition) && ContainsPoint(evt.MousePosition))
            {
                dragging = true;
                offset = evt.MousePosition - new Vector2(Left.Pixels, Top.Pixels);
            }
        }

        public override void LeftMouseUp(UIMouseEvent evt)
        {
            base.LeftMouseUp(evt);
            dragging = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (dragging)
            {
                Vector2 mouse = UserInterface.ActiveInstance.MousePosition;
                Left.Set(mouse.X - offset.X, 0f);
                Top.Set(mouse.Y - offset.Y, 0f);
                Recalculate();
            }
        }

        private bool IsMouseOverChild(Vector2 mousePos)
        {
            foreach (var child in Elements)
            {
                if (child.ContainsPoint(mousePos))
                    return true;
            }
            return false;
        }
    }
}