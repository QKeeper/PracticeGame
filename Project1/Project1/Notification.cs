using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project1
{
    internal class Notification : Entity
    {
        internal string title;
        internal List<(string, Vector2)> descriptionStrings = new();
        internal Vector2 notificationSize;
        internal Vector2 titleSize = Vector2.Zero;

        internal bool flagPlaySound = true;
        internal bool flagReady = false;
        internal float opacity = 0;

        internal readonly SoundEffect notificationSound = Game1.NotifcationSound;
        internal readonly int fadeTime = 1;
        internal readonly int stringSpacing = 4;
        internal readonly int padding = 16;
        internal readonly int stringMaxLength = 60;
        internal readonly Color textColor = Color.White;
        internal readonly Color shadowColor = Color.OrangeRed;
        internal readonly Vector2 shadowOffset = new(1, 1);

        internal Notification(float time, string title, string description)
        {
            this.time = time;
            this.title = title;
            titleSize = Game1.ConsolasMediumFont.MeasureString(title);
            if (titleSize.X > notificationSize.X) notificationSize.X = titleSize.X;
            notificationSize.Y += titleSize.Y;

            Vector2 measureString;
            string currentString = "";
            foreach (var word in description.Split(' '))
            {
                if (currentString.Length + word.Length< stringMaxLength)
                    currentString += word + " ";
                else
                {
                    measureString = Game1.ConsolasSmallFont.MeasureString(currentString.Trim());
                    if (measureString.X > notificationSize.X) notificationSize.X = measureString.X;
                    notificationSize.Y += measureString.Y + stringSpacing;
                    descriptionStrings.Add((currentString, new(measureString.X, measureString.Y)));
                    currentString = word + " ";
                }
            }
            measureString = Game1.ConsolasSmallFont.MeasureString(currentString.Trim());
            if (measureString.X > notificationSize.X) notificationSize.X = measureString.X;
            notificationSize.Y += measureString.Y + stringSpacing;
            descriptionStrings.Add((currentString, new(measureString.X, measureString.Y)));
        }

        internal override void Update(GameTime gameTime)
        {
            if (flagPlaySound)
            {
                notificationSound.Play();
                flagPlaySound = false;
            }

            if (!flagReady)
            {
                opacity += 255 / fadeTime * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (opacity > 255) flagReady = true;
            }
            else
            {
                time -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (time < 0) opacity -= 255 / fadeTime * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (opacity < 0) Game1.Notifications.Remove(this);
            }

            opacity = Math.Clamp(opacity, 0, 255);

        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.BlackSquareSprite, new Rectangle((int)(Game1.ScreenSize.X - notificationSize.X) / 2 - padding, 109 - padding, (int)notificationSize.X + padding * 2, (int)notificationSize.Y + stringSpacing + 32), new Color(Color.White, (int)(opacity / 255 * 100)));

            spriteBatch.DrawString(Game1.ConsolasMediumFont, title, new Vector2(Game1.ScreenSize.X / 2, 128) + shadowOffset - titleSize / 2, shadowColor);
            spriteBatch.DrawString(Game1.ConsolasMediumFont, title, new Vector2(Game1.ScreenSize.X / 2, 128) - titleSize / 2, textColor);

            for (var i = 0; i < descriptionStrings.Count; i++)
            {
                spriteBatch.DrawString(Game1.ConsolasSmallFont, descriptionStrings[i].Item1, new Vector2(Game1.ScreenSize.X / 2 - descriptionStrings[i].Item2.X / 2, 128 + (descriptionStrings[i].Item2.Y + stringSpacing) * (i + 1)) + shadowOffset, shadowColor);
                spriteBatch.DrawString(Game1.ConsolasSmallFont, descriptionStrings[i].Item1, new Vector2(Game1.ScreenSize.X / 2 - descriptionStrings[i].Item2.X / 2, 128 + (descriptionStrings[i].Item2.Y + stringSpacing) * (i + 1)), textColor);
            }
        }
    }
}
