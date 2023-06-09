using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class RatKing : Enemy
    {
        private SpriteEffects Effects = SpriteEffects.None;
        private readonly float StepFrequency = 2f;
        private float StepFrequencyTimer;

        internal RatKing()
        {
            Sprite = Game1.RatKingSprite;
            StepFrequencyTimer = StepFrequency;
            ColliderRadius = Sprite.Width * .5f;
            MovementSpeed = 0;
            MaxHealth = Health = 20;
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Knockback(gameTime);

            StepFrequencyTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (StepFrequencyTimer < 0)
            {
                StepFrequencyTimer += StepFrequency;
                SpawnSystem.SpawnEnemy(new Rat());
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            DrawShadow(spriteBatch);

            spriteBatch.Draw(Sprite, Position, null, Color.White, 0f, Center(Sprite), 1f, Effects, 1f);

            var margin = 4;
            var padding = 2;
            var barWidth = 8;
            spriteBatch.Draw(Game1.BlackSquareSprite, new Rectangle((int)Position.X - Sprite.Width / 2 - padding, (int)Position.Y + Sprite.Height / 2 + margin - padding, Sprite.Width + padding * 2, barWidth + padding * 2), new Color(Color.Black, 170));
            spriteBatch.Draw(Game1.WhiteSquareSprite, new Rectangle((int)Position.X - Sprite.Width / 2, (int)Position.Y + Sprite.Height / 2 + margin, (int)Lerp(0, Sprite.Width, Health / (MaxHealth)), barWidth), new Color(255, 77, 77));
        }

        internal override void Destroy()
        {
            Manager.GotoRoom(0);
            Notification.Add(new Notification("Good job!", "RAT KING was defeated."));
            base.Destroy();
        }
    }
}
