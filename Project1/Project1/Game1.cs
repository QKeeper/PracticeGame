using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Project1
{
    public class Game1 : Game
    {
        internal static Vector2 ScreenSize = new(2560, 1080);

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        internal static SpriteFont Berlin32Font;
        internal static SpriteFont ConsolasRegular;
        internal static SpriteFont ConsolasBold;

        internal static Texture2D BackgroundImage;
        internal static Texture2D BackgroundTestImage;

        internal static Texture2D CircleSprite;
        internal static Texture2D HeartSprite;
        internal static Texture2D ExtraHeartSprite;

        internal static Texture2D ShadowE1;
        internal static Texture2D ShadowE2;
        internal static Texture2D ShadowE3;
        internal static Texture2D ShadowE4;
        internal static Texture2D ShadowE5;
        internal static Texture2D ShadowE6;
        internal static Texture2D ShadowE7;
        internal static Texture2D ShadowE8;

        internal static Texture2D PlayerSprite;
        internal static Texture2D GooSprite;
        internal static Texture2D GnollSprite;
        internal static Texture2D RatSprite;

        internal static SoundEffect HitSound;

        internal static List<Entity> Entities = new();
        internal static List<SystemComponent> Systems = new();

        internal float TimerDebug = 0f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = (int)ScreenSize.X;
            _graphics.PreferredBackBufferHeight = (int)ScreenSize.Y;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            LoadContent();

            BiomeSystem.RandomizeBiome();

            Entities.Add(new Player() { position = new(ScreenSize.X / 2, ScreenSize.Y / 2) });
            //Entities.Add(new Rat() { position = new(1000, 200) });

            Systems.Add(new SpawnSystem(5f));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Berlin32Font = Content.Load<SpriteFont>("Fonts/BerlinMain");
            ConsolasRegular = Content.Load<SpriteFont>("Fonts/consolas_regular");
            ConsolasBold = Content.Load<SpriteFont>("Fonts/consolas_bold");

            HitSound = Content.Load<SoundEffect>("Sounds/hit");

            BackgroundImage = Content.Load<Texture2D>("Sprites/floor_bricks");
            BackgroundTestImage = Content.Load<Texture2D>("Sprites/backgroundtest2");

            CircleSprite = Content.Load<Texture2D>("circle");
            HeartSprite = Content.Load<Texture2D>("Sprites/UI/heart");
            ExtraHeartSprite = Content.Load<Texture2D>("Sprites/UI/extraHeart");

            ShadowE1 = Content.Load<Texture2D>("Sprites/Other/e1");
            ShadowE2 = Content.Load<Texture2D>("Sprites/Other/e2");
            ShadowE3 = Content.Load<Texture2D>("Sprites/Other/e3");
            ShadowE4 = Content.Load<Texture2D>("Sprites/Other/e4");
            ShadowE5 = Content.Load<Texture2D>("Sprites/Other/e5");
            ShadowE6 = Content.Load<Texture2D>("Sprites/Other/e6");
            ShadowE7 = Content.Load<Texture2D>("Sprites/Other/e7");
            ShadowE8 = Content.Load<Texture2D>("Sprites/Other/e8");

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
                entity.Update(gameTime);

            TimerDebug += (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Draw Background
            Texture2D background = BackgroundTestImage;
            for (var i = 0; i < _graphics.PreferredBackBufferWidth / BackgroundImage.Width; i++)
                for (var j = 0; j < _graphics.PreferredBackBufferHeight / BackgroundImage.Height + 1; j++)
                    _spriteBatch.Draw(background, new Vector2(background.Width * i, background.Height * j), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Draw each entity
            var entitiesListDebug = "- Entities List -\n";
            foreach (var entity in Entities.ToList())
            {
                entity.Draw(_spriteBatch);
                entitiesListDebug += entity.GetType() + " | " + Vector2.Round(entity.position) + "\n";
            }
            _spriteBatch.DrawString(Berlin32Font, entitiesListDebug, new(16, 196), Color.White);

            // Draw info for debugging
            string text = "";
            text += "\nResolution: " + _graphics.PreferredBackBufferWidth + "x" + _graphics.PreferredBackBufferHeight;
            text += "\nEntities count: " + Entities.Count;
            text += "\nSystems count: " + Systems.Count;
            text += "\nElapsed time: " + MathF.Round(TimerDebug) + " seconds";
            _spriteBatch.DrawString(Berlin32Font, text, new(16, 64), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
