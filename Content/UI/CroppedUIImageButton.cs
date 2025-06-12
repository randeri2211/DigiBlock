using Terraria.UI;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Text.Json; 
using Terraria;
using System;
using Terraria.GameContent.ItemDropRules;
using System.Linq;

namespace DigiBlock.Content.UI
{
    public class CroppedUIImageButton : UIImageButton
    {
        private readonly int frameHeight;
        private readonly int frameWidth;
        private readonly Texture2D myTexture;
        private readonly JsonElement conditions;

        public CroppedUIImageButton(Asset<Texture2D> texture, int frameWidth, int frameHeight, JsonElement conditions = new JsonElement())
            : base(texture)
        {
            this.conditions = conditions;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            myTexture = texture.Value;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            // base.DrawSelf(spriteBatch); 
            CalculatedStyle dimensions = GetDimensions();
            Vector2 position = dimensions.Position();
            Vector2 size = new Vector2(dimensions.Width, dimensions.Height);

            Vector2 imageSize = new Vector2(frameWidth, frameHeight);
            Vector2 drawPosition = position + (size - imageSize) / 2f;
            if (IsMouseHovering && conditions.ValueKind == JsonValueKind.Object && conditions.EnumerateObject().Any())
            {
                string textConditions = "";
                foreach (var condition in conditions.EnumerateObject())
                {
                    textConditions += condition.Name + ":" + condition.Value.ToString() + "\n";
                }
                Console.WriteLine(textConditions);
                Vector2 pos = new Vector2(Main.mouseX + 16, Main.mouseY + 16);
                Utils.DrawBorderString(Main.spriteBatch, textConditions, pos, Color.White);
            }
            // Console.WriteLine(size + "" + dimensions.Width + "," + dimensions.Height);
            spriteBatch.Draw(
                myTexture, // Stored Texture2D from constructor
                drawPosition,
                new Rectangle(0, 0, frameWidth, frameHeight),
                Color.White
            );
        }
    }

}
