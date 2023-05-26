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
        internal float maxHealth = 10f;
        internal float health = 10f;

        internal Enemy()
        {
            sprite = Game1.GooSprite;
            colliderRadius = sprite.Width / 2;

            movementSpeed = 150f;
        }

        internal virtual void Damage(float amount)
        {
            health -= amount;

            if (health <= 0) Destroy();
        }

        internal override void Update(GameTime gameTime)
        {
            Collide(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            DrawShadow(spriteBatch);
            spriteBatch.Draw(sprite, position, null, Color.White, 0f, Center(sprite), 1f, SpriteEffects.None, 0f);
        }

        internal void DropLoot(Vector2 position, int experience)
        {
            for (var i = 0; i < experience; i++)
                Game1.Entities.Add(new Loot(Loot.Type.exp) { position = position + new Vector2(i + 1, i + 1) * 8 });
        }

    }
}
