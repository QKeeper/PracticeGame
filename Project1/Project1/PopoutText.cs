using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Project1
{
    internal class PopoutText : Entity
    {
        private Color color;
        private SpriteFont font;
        internal string text;
        private Vector2 textSize;
        private float scale;
        private float rotation;
        private float lifetime = .8f;

        private readonly Random random = new();
        private readonly float measurement = 16f;
        private Vector2 offset;

        internal PopoutText(Vector2 position, SpriteFont font, Color color, float scale, string text)
        {
            this.Position = position;
            this.font = font;
            this.color = color;
            this.scale = random.NextFloat(scale * .9f, scale * 1.05f);
            rotation = random.NextFloat(-.15f, .15f);
            lifetime *= random.NextFloat(.85f, 1.15f);
            MovementSpeed = 15f * random.NextFloat(.8f, 1.2f);
            this.text = text;
            textSize = font.MeasureString(text);

            offset = new Vector2(random.NextFloat(-measurement, measurement), random.NextFloat(-measurement, measurement));
        }

        internal override void Update(GameTime gameTime)
        {
            lifetime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (lifetime <= 0) Destroy();

            MovementSpeed = 15f;
            Position += new Vector2(0, -MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text, Position + offset + new Vector2(1, 1), Color.Black, rotation, textSize / 2, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, text, Position + offset + new Vector2(-1, 1), Color.Black, rotation, textSize / 2, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, text, Position + offset + new Vector2(1, -1), Color.Black, rotation, textSize / 2, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, text, Position + offset + new Vector2(-1, -1), Color.Black, rotation, textSize / 2, scale, SpriteEffects.None, 0f);

            spriteBatch.DrawString(font, text, Position + offset, color, rotation, textSize / 2, scale, SpriteEffects.None, 0f);
        }
    }
}
