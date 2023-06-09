using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class Component
    {
        internal float Time;

        internal virtual void Update(GameTime gameTime) { Time += (float)gameTime.ElapsedGameTime.TotalSeconds; }

        internal virtual void Draw(SpriteBatch spriteBatch) { }

        internal static float Lerp(float firstFloat, float secondFloat, float by) => firstFloat * (1 - by) + secondFloat * by;
    }
}
