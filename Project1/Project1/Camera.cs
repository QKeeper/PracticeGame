using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal static class Camera
    {
        internal static Matrix Transform { get; private set; }
        internal static Matrix MatrixPosition = new();
        internal static Matrix MatrixOffset = new();

        internal static Vector2 Target = new();

        internal static Vector2 Position { get; private set; }
        internal static Vector2 Offset { get; private set; }
        internal static Vector2 Zero { get; private set; }
        internal static Rectangle Rectangle { get; private set; }
        internal static Vector2 MousePosition { get; private set; }

        internal static void Update(Vector2 target, GameTime gameTime)
        {
            Target = target;
            Follow(Target);
            Position = new Vector2(-MatrixPosition.M41, -MatrixPosition.M42);
            Offset = new Vector2(MatrixOffset.M41, MatrixOffset.M42);
            Zero = Position - Offset;
            Rectangle = new Rectangle((int)(Position.X - Offset.X), (int)(Position.Y - Offset.Y), (int)Offset.X * 2, (int)Offset.Y * 2);
            MousePosition = Zero + Mouse.GetState().Position.ToVector2();
        }

        internal static void Follow(Vector2 target)
        {
            MatrixPosition = Matrix.CreateTranslation(-target.X, -target.Y, 0);
            MatrixOffset = Matrix.CreateTranslation(Game1.ScreenSize.X / 2, Game1.ScreenSize.Y / 2, 0);
            Transform = MatrixPosition * MatrixOffset;
        }
    }
}
