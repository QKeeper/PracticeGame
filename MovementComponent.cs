using Microsoft.Xna.Framework;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame01
{
    public class MovementComponent : Component
    {
        public float speed = 0;
        public Vector2 target = Vector2.Zero;
    }
}
