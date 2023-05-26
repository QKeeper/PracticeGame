using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Project1
{
    public class Game1 : Game
    {
        internal static Vector2 ScreenSize = new(640, 480);

        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Fonts
        internal static SpriteFont ConsolasBigFont;
        internal static SpriteFont ConsolasMediumFont;
        internal static SpriteFont ConsolasSmallFont;

        // Backgrounds
        internal static Texture2D BackgroundImage;

        // UI
        internal static Texture2D HeartSprite;
        internal static Texture2D ExtraHeartSprite;

        // Others
        internal static Texture2D ShadowR8;
        internal static Texture2D BlackSquareSprite;
        internal static Texture2D ExperienceOrb1;
        internal static Texture2D ExperienceOrb2;
        internal static Texture2D ExperienceOrb3;

        // Entities
        internal static Texture2D PlayerSprite;
        internal static Texture2D GooSprite;
        internal static Texture2D GnollSprite;
        internal static Texture2D RatSprite;

        // Sounds
        internal static SoundEffect HitSound;
        internal static SoundEffect NotifcationSound;

        // Collections
        internal static List<Entity> Entities = new();
        internal static List<SystemComponent> Systems = new();
        internal static List<Notification> Notifications = new();

        internal static Player Player;

        internal float TimerDebug = 0f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.IsFullScreen = false;
            ScreenSize = new(1920, 1080);
            _graphics.PreferredBackBufferWidth = (int)ScreenSize.X;
            _graphics.PreferredBackBufferHeight = (int)ScreenSize.Y;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            LoadContent();

            BiomeSystem.RandomizeBiome();

            Entities.Add(new Player() { position = ScreenSize / 2 });
            Systems.Add(new SpawnSystem(5f));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Fonts
            ConsolasBigFont = Content.Load<SpriteFont>("Fonts/consolasBig");
            ConsolasMediumFont = Content.Load<SpriteFont>("Fonts/consolasMedium");
            ConsolasSmallFont = Content.Load<SpriteFont>("Fonts/consolasSmall");

            // Sounds
            HitSound = Content.Load<SoundEffect>("Sounds/hit");
            NotifcationSound = Content.Load<SoundEffect>("Sounds/notification");

            // Backgrounds
            BackgroundImage = Content.Load<Texture2D>("Sprites/Backgrounds/background");

            // UI
            HeartSprite = Content.Load<Texture2D>("Sprites/UI/heart");
            ExtraHeartSprite = Content.Load<Texture2D>("Sprites/UI/extraHeart");

            // Other
            ShadowR8 = Content.Load<Texture2D>("Sprites/Other/r8");
            BlackSquareSprite = Content.Load<Texture2D>("Sprites/Other/blackSquare");
            ExperienceOrb1 = Content.Load<Texture2D>("Sprites/Other/experience-orb1");
            ExperienceOrb2 = Content.Load<Texture2D>("Sprites/Other/experience-orb2");
            ExperienceOrb3 = Content.Load<Texture2D>("Sprites/Other/experience-orb3");

            // Entities
            PlayerSprite = Content.Load<Texture2D>("Sprites/Entities/Player");
            GooSprite = Content.Load<Texture2D>("Sprites/Entities/Goo");
            GnollSprite = Content.Load<Texture2D>("Sprites/Entities/Gnoll");
            RatSprite = Content.Load<Texture2D>("Sprites/Entities/Rat");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (SystemComponent system in Systems.ToList())
                system.Update(gameTime);

            foreach (Entity entity in Entities.ToList())
            {
                if (entity is Player) Player = entity as Player;

                entity.Update(gameTime);
            }

            if (Notifications.Count > 0) Notifications[0].Update(gameTime);

            TimerDebug += (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Draw Background
            _spriteBatch.Draw(BackgroundImage, new(0, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Draw each entity
            var entListDebug = "Total entities: " + Entities.Count + "\n";
            foreach (var entity in Entities.ToList())
            {
                entity.Draw(_spriteBatch);
                entListDebug += entity.GetType().ToString().Split('.')[1] + " " + MathF.Round(entity.position.X) + " " + MathF.Round(entity.position.Y) + "\n";
            }
            // _spriteBatch.DrawString(ConsolasSmallFont, entListDebug, new Vector2(17, 65), Color.Violet);
            // _spriteBatch.DrawString(ConsolasSmallFont, entListDebug, new Vector2(16, 64), Color.White);

            // Draw notifications
            if (Notifications.Count > 0) Notifications[0].Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
