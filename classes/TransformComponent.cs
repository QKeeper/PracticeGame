using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame01
{
    public class TransformComponent : Component
    {
        public Vector2 position = Vector2.Zero;
        public Vector2 scale = Vector2.One;
        public float rotation = 0;
    }
}
