using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame01
{
    public class WeaponComponent : Component
    {
        public class Weapon
        {
            public Type type = Type.None;
            public float Damage = 1f;
            public float Penetration = 0f;
            public float AttackRate = 1f;
            public float MovementSpeed = 1f;

            public enum Type
            {
                None,
                Sword,
                Spear,
                Hammer
            }
        }

        public Weapon Current;

        public WeaponComponent(Weapon weapon)
        {
            if (weapon == null)
                Current = new();
            else
                Current = weapon;
        }


    }
}
