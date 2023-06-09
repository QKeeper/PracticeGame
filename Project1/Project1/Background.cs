using Microsoft.VisualBasic.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;

namespace Project1
{
    internal static class Background
    {
        internal static Texture2D Image { get; private set; }
        internal static float scale { get; private set; }

        static Background()
        {
            Image = DefineBackground();
        }

        internal static void Draw(SpriteBatch spriteBatch)
        {
            if (Image == null) { spriteBatch.DrawString(Game1.ConsolasBigFont, "Background Texture Error", Camera.Zero + new Vector2(64, 64), Color.Red); return; }

            for (var i = -1; i < Game1.ScreenSize.X / Image.Width + 1; i++)
                for (var j = -1; j < Game1.ScreenSize.Y / Image.Height + 1; j++)
                    spriteBatch.Draw(Image, Game1.Player.SnapPoint + new Vector2(i * Image.Width, j * Image.Height), Color.White);
        }

        internal static void SetBackground(Texture2D texture) { Image = texture; }

        internal static Texture2D DefineBackground()
        {
            switch (Biome.GetBiome)
            {
                case Biome.BiomeType.Dungeon:
                    return Game1.BackgroundImage;
            }

            return default;
        }

    }
}
