using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class Rat : Enemy
    {
        internal Rat()
        {
            sprite = Game1.RatSprite;
            shadow = Game1.ShadowE7;
            colliderRadius = sprite.Width / 2.5f;
        }
    }
}
