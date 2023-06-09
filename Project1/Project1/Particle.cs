using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Project1
{
    internal class Particle : Entity
    {
        internal static List<Particle> List = new();

        internal static void Create(Texture2D sprite, Vector2 position, int amount, Color color, float force = 10f, float scale = 1f)
        {
            for (var i = 0; i < amount; i++)
                List.Add(new Particle(sprite, position, color, force, scale));
        }

        Color Color;
        float Force;
        float Scale;
        float ScaleMultiplier;
        float Rotation;
        Vector2 Direction;

        internal Particle(Texture2D sprite, Vector2 position, Color color, float force, float scale)
        {
            Random random = new();
            Sprite = sprite;
            Position = position;
            Color = color;
            Force = random.NextFloat(force * .1f, force * 1f);
            Scale = scale;
            ScaleMultiplier = random.NextFloat(1f, 5f);

            Direction = Vector2.Normalize(new Vector2(new Random().Next(-100, 100), new Random().Next(-100, 100)));
        }

        internal static void UpdateAll(GameTime gameTime)
        {
            foreach (var particle in List.ToList()) particle.Update(gameTime);
        }

        internal static void DrawAll(SpriteBatch spriteBatch)
        {
            foreach (var particle in List) particle.Draw(spriteBatch);
        }

        internal override void Update(GameTime gameTime)
        {
            foreach (var particle in List)
            base.Update(gameTime);

            Position += Direction * Force * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Rotation += 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Scale = Lerp(Scale, 0, 3f * ScaleMultiplier * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (Scale < 0.02) Destroy();
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position + new Vector2(1, 1), null, Color.Black, Rotation, Center(Sprite), Scale, SpriteEffects.None, 1f);
            spriteBatch.Draw(Sprite, Position + new Vector2(-1, 1), null, Color.Black, Rotation, Center(Sprite), Scale, SpriteEffects.None, 1f);
            spriteBatch.Draw(Sprite, Position + new Vector2(-1, -1), null, Color.Black, Rotation, Center(Sprite), Scale, SpriteEffects.None, 1f);
            spriteBatch.Draw(Sprite, Position + new Vector2(1, -1), null, Color.Black, Rotation, Center(Sprite), Scale, SpriteEffects.None, 1f);
            spriteBatch.Draw(Sprite, Position, null, Color, Rotation, Center(Sprite), Scale, SpriteEffects.None, 1f);
        }

        internal override void Destroy()
        {
            List.Remove(this);
        }

    }
}
