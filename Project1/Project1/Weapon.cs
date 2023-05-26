using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class Weapon : Entity
    {
        internal float damage = 1f;
        internal float attackRate = 2f;
        internal float movementSlowdown = 0f;
        internal float attackRange = 196f;
        internal float knockbackForce = 1f;
    }
}
