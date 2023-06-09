using Microsoft.Xna.Framework;
using SharpDX.MediaFoundation.DirectX;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project1
{
    internal static class SpawnSystem
    {
        internal static bool Active = false;
        
        private static float Frequency = 1f;
        internal static void SetFrequency(float frequency)
        {
            if (frequency <= 0) throw new ArgumentOutOfRangeException("Частота спавна не может быть меньше или равна нулю");
            Frequency = frequency;
        }

        private static float Timer = 0f;

        internal static void Update(GameTime gameTime)
        {
            if (!Active) return;
            if (Game1.Player == null || Game1.Player.Health <= 0) return;
            if (Timer > 0f) { Timer -= (float)gameTime.ElapsedGameTime.TotalSeconds; return; }

            Timer += Frequency;

            SpawnEnemy();
        }

        internal static void SpawnEnemy()
        {
            var entity = Biome.GetEntity();
            entity.Position = Camera.Zero + RandomPoint(Game1.ScreenSize);
            Game1.Entities.Add(entity);
        }

        internal static void SpawnEnemy(Enemy enemy)
        {
            enemy.Position = Camera.Zero + RandomPoint(Game1.ScreenSize);
            Game1.Entities.Add(enemy);
        }

        private static Vector2 RandomPoint(Vector2 area)
        {
            Random random = new();
            Vector2 point = new(0, 0);

            switch (random.Next(4))
            {
                // Верхняя граница
                case 0:
                    point = new(random.Next((int)area.X), 0);
                    break;
                // Правая граница
                case 1:
                    point = new(area.X, random.Next((int)area.Y));
                    break;
                // Нижняя граница
                case 2:
                    point = new(random.Next((int)area.X), area.Y);
                    break;
                // Левая граница
                case 3:
                    point = new(0, random.Next((int)area.Y));
                    break;
            }

            return point;
        }

    }
}
