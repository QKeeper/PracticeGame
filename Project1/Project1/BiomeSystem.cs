using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class BiomeSystem : SystemComponent
    {
        private static Biome CurrentBiome = Biome.Dungeon;

        internal enum Biome
        {
            Dungeon,
        }

        internal static Biome GetBiome => CurrentBiome;

        internal static void SetBiome(Biome biome)
        {
            CurrentBiome = biome;
        }

        internal static Biome RandomizeBiome()
        {
            Array values = Enum.GetValues(typeof(Biome));
            Random random = new();
            return CurrentBiome = (Biome)values.GetValue(random.Next(values.Length));
        }

        internal static Entity[] GetEntities()
        {
            if (GetBiome == Biome.Dungeon)
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
