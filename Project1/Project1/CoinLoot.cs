using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class CoinLoot : Loot
    {
        int Amount = 0;

        internal CoinLoot(Vector2 position, int amount)
        {
            Sprite = Game1.CoinSprite;
            Position = position;
            Amount = amount;
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MovementSpeed = (Vector2.Distance(Position, Game1.Player.Position) < 48) ? (5000f / Vector2.Distance(Position, Game1.Player.Position)) : 0f;
            if (Vector2.Distance(Position, Game1.Player.Position) < 16) Destroy();

            Position += Vector2.Normalize(Game1.Player.Position - Position) * MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        internal override void Destroy()
        {
            Game1.ExperienceSound.Play();
            Game1.Entities.Add(new PopoutText(Position, Game1.ConsolasBigFont, Color.Gold, .8f, "+" + Amount));
            Statistics.Coins += Amount;
            base.Destroy();
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 offset = Vector2.Zero;

            for (var i = 0; i < Amount; i++)
            {
                spriteBatch.Draw(Sprite, Position + offset + new Vector2(2, 2), null, Color.Black, 0f, Center(Sprite), 1.6f, SpriteEffects.None, 1f);
                spriteBatch.Draw(Sprite, Position + offset, null, Color.White, 0f, Center(Sprite), 1.6f, SpriteEffects.None, 1f);
            }
        }   
    }
}
