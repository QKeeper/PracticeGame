using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class Entity
    {
        internal Texture2D sprite;
        internal Texture2D shadow;
        internal Vector2 position;
        internal float colliderRadius = 16;
        internal float time = 0f;

        internal (Vector2 direction, float force, float time) knockback;
        internal Color DefineColor() => knockback.time <= 0 ? Color.White : new Color(Color.MediumVioletRed, 200);
        internal static Vector2 Center(Texture2D texture) => new(texture.Width / 2, texture.Height / 2);
        internal virtual void Update(GameTime gameTime) { }
        internal virtual void Draw(SpriteBatch spriteBatch) { }

        internal void Destroy()
        {
            Game1.Entities.Remove(this);
        }

        internal void Knockback(Vector2 direction, float force, float time)
        {
            knockback.time = time;
            knockback.direction = Vector2.Normalize(direction);
            knockback.force = force;
        }

        internal void Knockback(GameTime gameTime)
        {
            if (knockback.time <= 0) return;

            knockback.time -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += knockback.direction * knockback.force * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        internal void DrawShadow(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(shadow, new Rectangle((int)position.X, (int)position.Y + sprite.Height / 2, (int)(sprite.Width * .85f), sprite.Height / 3), null, new Color(Color.Black, 128), 0f, Center(shadow), SpriteEffects.None, 0f);
        }

    }
}
