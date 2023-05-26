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
    internal class Gnoll : Enemy
    {
        private Vector2 direction = Vector2.Zero;
        private SpriteEffects spriteEffects;
        private float rotation;

        private float hitDelayTimer = 0f;
        private float hitDelay = .45f;

        internal Gnoll()
        {
            sprite = Game1.GnollSprite;
            shadow = Game1.ShadowR8;
            colliderRadius = sprite.Width / 2.5f;

            maxHealth = health = 5;
            movementSpeed = 250;
        }

        internal override void Update(GameTime gameTime)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            Collide(gameTime);
            Knockback(gameTime);
            Movement(gameTime);
            HitPlayer(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            DrawShadow(spriteBatch);

            spriteEffects = direction.X > 0 ? SpriteEffects.None : (direction.X < 0 ? SpriteEffects.FlipHorizontally : spriteEffects);
            spriteBatch.Draw(sprite, position, null, DefineColor(), rotation, Center(sprite), 1f, spriteEffects, 0f);

            // spriteBatch.DrawString(Game1.Berlin32Font, "Health: " + health + "/" + maxHealth, new Vector2(position.X - sprite.Width / 2, position.Y - sprite.Height / 2 - 16), Color.White);
        }

        internal void Movement(GameTime gameTime)
        {
            if (knockback.time > 0) return;

            if (Game1.Player.health > 0)
            {
                direction = Vector2.Normalize(Game1.Player.position - position);
                rotation = MathF.Sin(time * 14) / 9;
            }
            else
            {
                direction = Vector2.Zero;
                rotation = 0;
            }

            position += direction * movementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        }

        internal void HitPlayer(GameTime gameTime)
        {
            if (hitDelayTimer > 0)
            {
                hitDelayTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                return;
            }

            if (knockback.time > 0) return;

            foreach (Entity entity in Game1.Entities.ToList())
            {
                if (entity is not Player) continue;
                if (Vector2.Distance(position, entity.position) > sprite.Width) continue;
                if (entity.knockback.time > 0) continue;

                (entity as Player).Damage(1);
                hitDelayTimer = hitDelay;
                entity.Knockback(entity.position - position, 175f, 0.2f);
                Knockback(position - entity.position, 175f, 0.25f);
                Game1.HitSound.Play();
            }
        }

        internal override void Destroy()
        {
            DropLoot(position, new Random().Next(3) + 1);
            base.Destroy();
        }

    }
}
