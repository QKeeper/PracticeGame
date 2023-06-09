using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class Dummy : Enemy
    {
        private string Name = "Dummy";
        private Vector2 NameSize;

        internal Dummy()
        {
            Sprite = Game1.DummySprite;
            NameSize = Game1.ConsolasSmallFont.MeasureString(Name);
            MovementSpeed = 0;
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            knockback.time -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        internal override void Damage(float amount) { }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            DrawShadow(spriteBatch);

            spriteBatch.Draw(Sprite, Position, null, DefineColor(), 0f, Center(Sprite), 1f, SpriteEffects.None, 1f);

            if (Vector2.Distance(Game1.Player.Position, Position) < 176)
            {
                var padding = 4;
                var textPosition = Position - new Vector2(NameSize.X / 2, Sprite.Height / 2 + NameSize.Y) - new Vector2(0, 4);
                spriteBatch.Draw(Game1.BlackSquareSprite, new Rectangle((int)textPosition.X - padding, (int)textPosition.Y - padding, (int)NameSize.X + padding * 2, (int)NameSize.Y + padding * 2 - 4), new Color(Color.White, 90));
                spriteBatch.DrawString(Game1.ConsolasSmallFont, Name, textPosition, Color.White);
            }
        }
    }
}
