using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace Project1
{
    internal class Enemy : Entity
    {
        internal float MaxHealth = 10f;
        internal float Health = 10f;

        internal Enemy()
        {
            Sprite = Game1.GooSprite;

            MovementSpeed = 150f;
        }

        internal virtual void Damage(float amount)
        {
            Health -= amount;

            if (Health <= 0) Destroy();
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Collide(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            DrawShadow(spriteBatch);
            spriteBatch.Draw(Sprite, Position, null, Color.White, 0f, Center(Sprite), 1f, SpriteEffects.None, 0f);
        }

    }
}
