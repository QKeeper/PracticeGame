using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class Ghost : Entity
    {
        internal static void Create(Vector2 position) => Game1.Entities.Add(new Ghost(position));

        internal static void Create(Vector2 position, Texture2D sprite) => Game1.Entities.Add(new Ghost(position) { Sprite = sprite });

        private float Opacity = 170f;

        internal Ghost(Vector2 position)
        {
            Position = position;
            Sprite = Game1.GhostSprite;
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Position += new Vector2(0, -25f * (float)gameTime.ElapsedGameTime.TotalSeconds);
            Opacity -= 170f / 1.7f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (Opacity < 0) Destroy();
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(Sprite, Position, null, new Color(Color.Black, (int)Opacity), 0f, Center(Sprite), .9f, SpriteEffects.None, 1f);
        }
    }
}
