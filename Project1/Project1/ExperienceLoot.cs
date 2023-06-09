using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class ExperienceLoot : Loot
    {
        internal ExperienceLoot(Vector2 position)
        {
            Position = position;

            Random random = new();
            var expSpriteList = new List<Texture2D> { Game1.ExperienceOrb1, Game1.ExperienceOrb2, Game1.ExperienceOrb3 };
            Sprite = expSpriteList[random.Next(expSpriteList.Count)];
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MovementSpeed = Game1.Player.Health <= 0 ? 0 : 7000f / Vector2.Distance(Position, Game1.Player.Position);
             
            if (Vector2.Distance(Position, Game1.Player.Position) < Sprite.Width)
            {
                Statistics.Experience++;
                Game1.Entities.Add(new PopoutText(Game1.Player.Position, Game1.ConsolasMediumFont, new Color(50, 255, 126), .65f, "+exp"));
                Game1.ExperienceSound.Play();
                Destroy();
            }

            Position += Vector2.Normalize(Game1.Player.Position - Position) * MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            DrawShadow(spriteBatch);
            spriteBatch.Draw(Sprite, Position, null, Color.White, 0f, Center(Sprite), 2f, SpriteEffects.None, 0f);
        }
    }
}
