using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal static class WeaponSystem
    {
        static readonly Random random = new();
        static List<Weapon> weapons = new();

        static WeaponSystem()
        {
            // weapons.Add(new Weapon() { });
        }

        internal static void SetLevel(ref this (float value, int level, float minValue, float step) weaponAttribute, int level)
        {
            var result = weaponAttribute.minValue;
            for (var i = 1; i < level; i++)
                result += weaponAttribute.step;
            weaponAttribute = (result, level, weaponAttribute.minValue, weaponAttribute.step);
        }

        internal static void AddLevel(ref this (float value, int level, float minValue, float step) weaponAttribute, int level = 1)
        {
            for (var i = 0; i < level; i++)
            {
                weaponAttribute.value += weaponAttribute.step;
                weaponAttribute.level++;
            }
        }

        private static readonly List<Weapon> List = new()
        {
            // new Weapon(null)
        };
    }
}
