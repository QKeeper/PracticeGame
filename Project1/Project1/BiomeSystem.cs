using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal static class Biome
    {
        private static BiomeType CurrentBiome = BiomeType.Dungeon;

        internal enum BiomeType
        {
            Dungeon,
        }

        internal static BiomeType GetBiome => CurrentBiome;

        internal static void Set(BiomeType biome)
        {
            CurrentBiome = biome;
        }

        internal static BiomeType Randomize()
        {
            Array values = Enum.GetValues(typeof(BiomeType));
            Random random = new();
            return CurrentBiome = (BiomeType)values.GetValue(random.Next(values.Length));
        }

        internal static Texture2D GetBackground()
        {
            if (GetBiome == BiomeType.Dungeon)
                return Game1.BackgroundImage;

            return default;
        }

        internal static Entity[] GetEntities()
        {
            if (GetBiome == BiomeType.Dungeon)
                return new Entity[] { new Gnoll(), new Rat() };

            return Array.Empty<Entity>();
        }

        internal static Entity GetEntity()
        {
            Random random = new();
            Entity[] entities = GetEntities();

            return entities?[random.Next(entities.Length)];
        }

    }
}
