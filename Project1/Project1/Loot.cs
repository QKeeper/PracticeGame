using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class Loot : Entity
    {
        internal static void Drop(Vector2 position, int xp = 0, int coins = 0)
        {
            for (var i = 0; i < xp; i++)
                Game1.Entities.Add(new ExperienceLoot(position + new Vector2(MathF.Cos(i * (6.2831f / xp)), MathF.Sin(i * (6.2831f / xp))) * xp * 1.15f));

            if (coins > 0) Game1.Entities.Add(new CoinLoot(position, coins));
        }
    }
}
