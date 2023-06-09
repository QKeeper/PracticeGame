using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class Anvil : Entity
    {
        private readonly string InteractText = "[E]Anvil";
        private readonly Vector2 InteractTextSize;
        private bool KeyState;

        internal float Price = 3f;

        internal Anvil()
        {
            Sprite = Game1.AnvilSprite;

            InteractTextSize = Game1.ConsolasSmallFont.MeasureString(InteractText);
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Collide(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.E) && !KeyState)
            {
                if (Statistics.Coins >= Price)
                {
                    Statistics.Coins -= Price;
                    Game1.Player.weapon.Upgrade();
                    Game1.ExperienceSound.Play();
                    Particle.Create(Game1.HitParticle, Position - new Vector2(0, Sprite.Height * .35f), new Random().Next(10, 15), Color.Gray, 250, 2);
                    Price *= 1.3f;
                }
            }

            KeyState = Keyboard.GetState().IsKeyDown(Keys.E);

        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            DrawShadow(spriteBatch);

            spriteBatch.Draw(Sprite, Position + Vector2.UnitX * 2, null, new Color(Color.Black, 170), 0f, Center(Sprite), 1f, SpriteEffects.None, 1f);
            spriteBatch.Draw(Sprite, Position - Vector2.UnitX * 2, null, new Color(Color.Black, 170), 0f, Center(Sprite), 1f, SpriteEffects.None, 1f);
            spriteBatch.Draw(Sprite, Position + Vector2.UnitY * 2, null, new Color(Color.Black, 170), 0f, Center(Sprite), 1f, SpriteEffects.None, 1f);
            spriteBatch.Draw(Sprite, Position - Vector2.UnitY * 2, null, new Color(Color.Black, 170), 0f, Center(Sprite), 1f, SpriteEffects.None, 1f);

            spriteBatch.Draw(Sprite, Position, null, Color.White, 0f, Center(Sprite), 1f, SpriteEffects.None, 1f);

            if (Vector2.Distance(Position, Game1.Player.Position) < 176)
            {
                Vector2 textPosition = Position - new Vector2(0, Sprite.Height / 2) - new Vector2(InteractTextSize.X / 2, InteractTextSize.Y) - new Vector2(0, 4);
                int padding = 4;

                spriteBatch.Draw(Game1.BlackSquareSprite, new Rectangle((int)textPosition.X - padding, (int)textPosition.Y - padding, (int)InteractTextSize.X + padding * 2, (int)InteractTextSize.Y + padding * 2 - 4), new Color(Color.White, 90));
                spriteBatch.DrawString(Game1.ConsolasSmallFont, InteractText, textPosition + new Vector2(1), Color.Black);
                spriteBatch.DrawString(Game1.ConsolasSmallFont, InteractText, textPosition, Color.White);

                spriteBatch.Draw(Game1.CoinSprite, textPosition + InteractTextSize - new Vector2(0, InteractTextSize.Y / 2), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                var color = Statistics.Coins > Price ? Color.Gold : new Color(255, 77, 77);
                spriteBatch.DrawString(Game1.ConsolasSmallFont, "" + MathF.Ceiling(Price), textPosition + InteractTextSize + new Vector2(Game1.CoinSprite.Width + 2, - InteractTextSize.Y / 2) + new Vector2(1), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                spriteBatch.DrawString(Game1.ConsolasSmallFont, "" + MathF.Ceiling(Price), textPosition + InteractTextSize + new Vector2(Game1.CoinSprite.Width + 2, - InteractTextSize.Y / 2), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }
        }
    }
}
