using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class Loot : Entity
    {
        internal Loot(Type type)
        {
            movementSpeed = 500f;

            Random random = new();
            var expSpriteList = new List<Texture2D> { Game1.ExperienceOrb1, Game1.ExperienceOrb2, Game1.ExperienceOrb3};
            if (type == Type.exp) sprite = expSpriteList[random.Next(expSpriteList.Count)];
        }

        internal override void Update(GameTime gameTime)
        {
            var collidedEntities = Collide(gameTime);

            if (collidedEntities.Contains(Game1.Player))
            {
                Game1.Player.experience++;
                Destroy();
            }

            position += Vector2.Normalize(Game1.Player.position - position) * movementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        internal enum Type
        {
            exp,
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            DrawShadow(spriteBatch);
            spriteBatch.Draw(sprite, position, null, Color.White, 0f, Center(sprite), 2f, SpriteEffects.None, 0f);
        }
    }
}
