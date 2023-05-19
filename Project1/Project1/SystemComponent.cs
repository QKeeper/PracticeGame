using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class SystemComponent
    {
        internal virtual void Update(GameTime gameTime) { }
        internal virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
