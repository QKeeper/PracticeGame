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
        internal static Vector2 ScreenSize = new(1366, 768);

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
        internal static Texture2D CircleSprite;
        internal static Texture2D BlackSquareSprite;
        internal static Texture2D WhiteSquareSprite;
        internal static Texture2D ExperienceOrb1;
        internal static Texture2D ExperienceOrb2;
        internal static Texture2D ExperienceOrb3;
        internal static Texture2D CoinSprite;
        internal static Texture2D CellSprite;

        // Entities
        internal static Texture2D GhostSprite;
        internal static Texture2D PlayerSprite;
        internal static Texture2D GooSprite;
        internal static Texture2D GnollSprite;
        internal static Texture2D RatSprite;
        internal static Texture2D RatKingSprite;
        internal static Texture2D AnvilSprite;
        internal static Texture2D DummySprite;

        // Items
        internal static Texture2D SwordSprite;
        internal static Texture2D SpearSprite;

        //Particles
        internal static Texture2D HitParticle;

        // Sounds
        internal static SoundEffect HitSound;
        internal static SoundEffect NotifcationSound;
        internal static SoundEffect ExperienceSound;

        internal static List<Entity> Entities;
        internal static Player Player;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.IsFullScreen = false;
            // ScreenSize = new(1920, 1080);
            // ScreenSize = new(2560, 1080);
            _graphics.PreferredBackBufferWidth = (int)ScreenSize.X;
            _graphics.PreferredBackBufferHeight = (int)ScreenSize.Y;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            LoadContent();
            Manager.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Fonts
            ConsolasBigFont = Content.Load<SpriteFont>("Fonts/consolasBig");
            ConsolasMediumFont = Content.Load<SpriteFont>("Fonts/consolasMedium");
            ConsolasSmallFont = Content.Load<SpriteFont>("Fonts/consolasSmall");

            // Backgrounds
            BackgroundImage = Content.Load<Texture2D>("Sprites/Backgrounds/background");

            // UI
            HeartSprite = Content.Load<Texture2D>("Sprites/UI/heart");
            ExtraHeartSprite = Content.Load<Texture2D>("Sprites/UI/extraHeart");

            // Other
            ShadowR8 = Content.Load<Texture2D>("Sprites/Other/r8");
            CircleSprite = Content.Load<Texture2D>("Sprites/Other/Circle");
            BlackSquareSprite = Content.Load<Texture2D>("Sprites/Other/blackSquare");
            WhiteSquareSprite = Content.Load<Texture2D>("Sprites/Other/whiteSquare");
            ExperienceOrb1 = Content.Load<Texture2D>("Sprites/Other/experience-orb1");
            ExperienceOrb2 = Content.Load<Texture2D>("Sprites/Other/experience-orb2");
            ExperienceOrb3 = Content.Load<Texture2D>("Sprites/Other/experience-orb3");
            CoinSprite = Content.Load<Texture2D>("Sprites/Other/coin");
            CellSprite = Content.Load<Texture2D>("Sprites/Other/cell1");

            // Entities
            GhostSprite = Content.Load<Texture2D>("Sprites/Entities/Ghost");
            PlayerSprite = Content.Load<Texture2D>("Sprites/Entities/Player");
            GooSprite = Content.Load<Texture2D>("Sprites/Entities/Goo");
            GnollSprite = Content.Load<Texture2D>("Sprites/Entities/Gnoll");
            RatSprite = Content.Load<Texture2D>("Sprites/Entities/Rat");
            RatKingSprite = Content.Load<Texture2D>("Sprites/Entities/Rat-King");
            AnvilSprite = Content.Load<Texture2D>("Sprites/Entities/Anvil");
            DummySprite = Content.Load<Texture2D>("Sprites/Entities/Statue");

            // Items
            SwordSprite = Content.Load<Texture2D>("Sprites/Items/Sword");
            SpearSprite = Content.Load<Texture2D>("Sprites/Items/Spear");

            // Particles
            HitParticle = Content.Load<Texture2D>("Sprites/Other/HitParticle");

            // Sounds
            HitSound = Content.Load<SoundEffect>("Sounds/hit");
            NotifcationSound = Content.Load<SoundEffect>("Sounds/notification");
            ExperienceSound = Content.Load<SoundEffect>("Sounds/experience");
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            Manager.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(transformMatrix: Camera.Transform);
            Manager.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
