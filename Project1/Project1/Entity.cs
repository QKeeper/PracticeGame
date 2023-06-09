using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class Entity : Component
    {
        internal Texture2D Sprite;
        internal Texture2D Shadow = Game1.ShadowR8;
        internal Vector2 Position;
        internal float ColliderRadius = 16;
        internal float MovementSpeed = 0;

        internal (Vector2 direction, float force, float time) knockback;
        internal Color DefineColor() => knockback.time <= 0 ? Color.White : new Color(Color.MediumVioletRed, 200);
        internal static Vector2 Center(Texture2D texture) => (texture is null) ? Vector2.Zero : new(texture.Width / 2, texture.Height / 2);

        internal Entity()
        {
            ColliderRadius = Sprite is null ? 32 : Sprite.Width * .4f;
        }

        internal virtual void Destroy()
        {
            Game1.Entities.Remove(this);
        }

        internal void Knockback(Vector2 direction, float force, float time)
        {
            knockback.time = time;
            knockback.direction = Vector2.Normalize(direction);
            knockback.force = force;
        }

        internal virtual void Knockback(GameTime gameTime)
        {
            if (knockback.time <= 0) return;

            knockback.time -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += knockback.direction * knockback.force * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        internal void DrawShadow(SpriteBatch spriteBatch)
        {
            if (Sprite is not null && Shadow is not null)
                spriteBatch.Draw(Shadow, new Rectangle((int)Position.X, (int)Position.Y + Sprite.Height / 2, (int)(Sprite.Width * .85f), Sprite.Height / 3), null, new Color(Color.Black, 128), 0f, Center(Shadow), SpriteEffects.None, 0f);
        }

        internal List<Entity> Collide(GameTime gameTime)
        {
            var collidedEntities = new List<Entity>();

            var entities = Game1.Entities.ToList();
            entities.Remove(this);
            foreach (Entity entity in entities)
            {
                if (entity is Loot or PopoutText or Weapon or Particle or Ghost) continue;
                if (Vector2.Distance(Position, entity.Position) > ColliderRadius + entity.ColliderRadius) continue;
                collidedEntities.Add(entity);
                while (Vector2.Distance(Position, entity.Position) < ColliderRadius + entity.ColliderRadius)
                {
                    if (Position == entity.Position) Position += Vector2.UnitX;
                    var direction = Vector2.Normalize(Position - entity.Position).X is Single.NaN ? Vector2.UnitX : Vector2.Normalize(Position - entity.Position);
                    Position += direction * MovementSpeed * .05f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    entity.Position += -direction * entity.MovementSpeed * .05f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            return collidedEntities;
        }

    }
}
