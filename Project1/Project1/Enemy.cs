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
        internal float movementSpeed = 250;

        internal Enemy()
        {
            sprite = Game1.GooSprite;
            shadow = Game1.ShadowE8;
            colliderRadius = sprite.Width / 2;
        }

        internal virtual void Damage(float amount) { }
        internal override void Update(GameTime gameTime)
        {
            Collide(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            DrawShadow(spriteBatch);
            spriteBatch.Draw(sprite, position, null, Color.White, 0f, Center(sprite), 1f, SpriteEffects.None, 0f);
        }

        internal void Collide(GameTime gameTime)
        {
            var entities = Game1.Entities.ToList();
            entities.Remove(this);
            foreach (Entity entity in entities)
            {
                if (Vector2.Distance(position, entity.position) > colliderRadius + entity.colliderRadius) continue;
                while (Vector2.Distance(position, entity.position) < colliderRadius + entity.colliderRadius)
                {
                    position += Vector2.Normalize(position - entity.position);
                    entity.position += Vector2.Normalize(entity.position - position) * movementSpeed * .5f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }

    }
}
