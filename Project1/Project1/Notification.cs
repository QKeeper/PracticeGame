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
    internal class Notification
    {
        private static readonly List<Notification> List = new();

        internal string title;
        internal List<(string, Vector2)> descriptionStrings = new();
        internal Vector2 notificationSize;
        internal Vector2 titleSize = Vector2.Zero;

        internal bool flagPlaySound = true;
        internal bool flagReady = false;
        internal float opacity = 0;
        internal float time = 0;

        internal readonly SoundEffect notificationSound = Game1.NotifcationSound;
        internal readonly int fadeTime = 1;
        internal readonly int stringSpacing = 4;
        internal readonly int padding = 16;
        internal readonly int stringMaxLength = 60;
        internal readonly Color textColor = Color.White;
        internal readonly Color shadowColor = Color.OrangeRed;
        internal readonly Vector2 shadowOffset = new(1, 1);

        internal static void Add(Notification notification) => List.Add(notification);

        internal static void Clear() => List.Clear();
        
        internal static void Update(GameTime gameTime) { if (List.Count > 0) List[0].UpdateElement(gameTime); }

        internal static void Draw(SpriteBatch spriteBatch) { if (List.Count > 0) List[0].DrawElement(spriteBatch); }

        internal Notification(string title, string description, float time = 2)
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
                if (currentString.Length + word.Length < stringMaxLength)
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

        internal void UpdateElement(GameTime gameTime)
        {
            if (flagPlaySound) { notificationSound.Play(); flagPlaySound = false; }

            if (!flagReady)
            {
                opacity += 255 / fadeTime * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (opacity > 255) flagReady = true;
            }
            else
            {
                time -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (time < 0) opacity -= 255 / fadeTime * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (opacity < 0) List.Remove(this);
            }

            opacity = Math.Clamp(opacity, 0, 255);

        }

        internal void DrawElement(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.BlackSquareSprite, 
                new Rectangle(
                    (int)(Camera.Zero.X + (Game1.ScreenSize.X - notificationSize.X) / 2 - padding), 
                    (int)Camera.Zero.Y + 109 - padding, 
                    (int)notificationSize.X + padding * 2, 
                    (int)notificationSize.Y + stringSpacing + padding * 2),
                new Color(Color.White, (int)(opacity / 255 * 100)));

            spriteBatch.DrawString(Game1.ConsolasMediumFont, title, Camera.Zero + new Vector2(Game1.ScreenSize.X / 2, 128) + shadowOffset - titleSize / 2, shadowColor);
            spriteBatch.DrawString(Game1.ConsolasMediumFont, title, Camera.Zero + new Vector2(Game1.ScreenSize.X / 2, 128) - titleSize / 2, textColor);

            for (var i = 0; i < descriptionStrings.Count; i++)
            {
                spriteBatch.DrawString(Game1.ConsolasSmallFont, descriptionStrings[i].Item1, Camera.Zero + new Vector2(Game1.ScreenSize.X / 2 - descriptionStrings[i].Item2.X / 2, 128 + (descriptionStrings[i].Item2.Y + stringSpacing) * (i + 1)) + shadowOffset, shadowColor);
                spriteBatch.DrawString(Game1.ConsolasSmallFont, descriptionStrings[i].Item1, Camera.Zero + new Vector2(Game1.ScreenSize.X / 2 - descriptionStrings[i].Item2.X / 2, 128 + (descriptionStrings[i].Item2.Y + stringSpacing) * (i + 1)), textColor);
            }
        }
    }
}
