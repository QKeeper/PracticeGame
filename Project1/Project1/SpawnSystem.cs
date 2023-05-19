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
    internal class SpawnSystem : SystemComponent
    {
        internal float frequency = 1f;
        private float timer = 0f;

        internal SpawnSystem(float frequency)
        {
            this.frequency = frequency;
        }

        internal override void Update(GameTime gameTime)
        {
            if (timer > 0f)
            {
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                return;
            }

            timer += frequency;

            var entity = BiomeSystem.GetEntity();
            entity.position = RandomPoint(Game1.ScreenSize);
            Game1.Entities.Add(entity);
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
