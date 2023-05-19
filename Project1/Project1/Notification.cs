using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class Notification : Entity
    {
        internal string text;

        internal Notification(float time, string text)
        {
            this.time = time;
            this.text = text;
        }

        internal override void Update(GameTime gameTime)
        {
            time--;
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
