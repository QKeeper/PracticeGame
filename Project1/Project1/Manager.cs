using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Microsoft.Xna.Framework.Input;
using System.Drawing.Printing;
using SharpDX.Direct3D9;

namespace Project1
{
    internal static class Manager
    {
        internal static int CurrentRoom = 0;
        internal static int HighestRoom = 1;
        internal static int RoomTime = 120;
        internal static int DefeatedCreaturesGoal = 30;

        internal static void Initialize()
        {
            Game1.Entities = new();

            Biome.Randomize();
            CreateRoom();
        }

        internal static void Update(GameTime gameTime)
        {
            foreach (var entity in Game1.Entities.ToList())
            {
                if (entity is Player) Game1.Player = entity as Player;
                entity.Update(gameTime);
            }

            if (CurrentRoom > HighestRoom) HighestRoom = CurrentRoom;
            if (Keyboard.GetState().IsKeyDown(Keys.T)) Statistics.DefeatedCreatures = DefeatedCreaturesGoal;

            if (CurrentRoom == 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    var weapon = Game1.Player.weapon;
                    GotoRoom(HighestRoom);
                    Game1.Player.weapon = weapon;
                    weapon.owner = Game1.Player;

                    var description = HighestRoom == 4 ? "Defeat the RAT KING" : "Defeat " + DefeatedCreaturesGoal + " creatures";
                    Notification.Add(new Notification("Floor " + HighestRoom, description));
                }
            }
            else if (CurrentRoom <= 3)
            {
                if (Statistics.DefeatedCreatures >= DefeatedCreaturesGoal) { Statistics.DefeatedCreatures = 0; NextRoom(); }

                if (Game1.Player.Health <= 0 || Game1.Player.Time > RoomTime)
                {
                    Game1.Player.Health = 5;
                    var weapon = Game1.Player.weapon;
                    GotoRoom(0);
                    Notification.Add(new("Game Over", string.Format("You defeated {0}/{1} creatures.", Statistics.DefeatedCreatures, DefeatedCreaturesGoal))); Statistics.DefeatedCreatures = 0;
                    Game1.Player.weapon = weapon;
                    weapon.owner = Game1.Player;
                }
            }
            else if (CurrentRoom <= 4)
            {
                if (Game1.Player.Health <= 0)
                {
                    Game1.Player.Health = 5;
                    var weapon = Game1.Player.weapon;
                    GotoRoom(0);
                    Notification.Add(new("Game Over", string.Format("RAT KING is killing you.", Statistics.DefeatedCreatures, DefeatedCreaturesGoal))); Statistics.DefeatedCreatures = 0;
                    Game1.Player.weapon = weapon;
                    weapon.owner = Game1.Player;
                }    
            }
            
            SpawnSystem.Update(gameTime);

            Particle.UpdateAll(gameTime);

            Notification.Update(gameTime);

            Camera.Update(Game1.Player.ViewPoint, gameTime);
        }

        internal static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Background.Draw(spriteBatch);

            foreach (var entity in Game1.Entities.ToList())
                entity.Draw(spriteBatch);

            Particle.DrawAll(spriteBatch);

            Notification.Draw(spriteBatch);

            if (CurrentRoom <= 0)
            {
                DrawText(spriteBatch, new(24, 64), "You are in the lobby", Color.White);
                DrawText(spriteBatch, new(24, 64 + 24), "Press 'R' to start game", Color.LightGreen);
                DrawText(spriteBatch, new(24, 64 + 24 + 24), "Coins: " + MathF.Round(Statistics.Coins), Color.Gold, Game1.CoinSprite, Vector2.Zero, Vector2.One);

                var lobbyText = ">> Controls <<\nMovement: W, A, S, D\nAttack: LMB";
                DrawText(spriteBatch, new(24, 64 + 256), lobbyText, Color.LightGreen);
            }
            else if (CurrentRoom <= 3)
            {
                DrawText(spriteBatch, new(24, 64), RoomTime - MathF.Round(Game1.Player.Time) + " seconds remaining", Color.White);
                DrawText(spriteBatch, new(24, 64 + 24), "Defeated " + Statistics.DefeatedCreatures + "/" + DefeatedCreaturesGoal + " creatures", new Color(255, 77, 77), Game1.SwordSprite, new Vector2(-12, 0), new Vector2(.3f, .3f));
                DrawText(spriteBatch, new(24, 64 + 24 + 24), "Coins: " + MathF.Round(Statistics.Coins), Color.Gold, Game1.CoinSprite, Vector2.Zero, Vector2.One);
            }
            else
            {
                DrawText(spriteBatch, new(24, 64), RoomTime - MathF.Round(Game1.Player.Time) + " seconds remaining", Color.White);
                DrawText(spriteBatch, new(24, 64 + 24), "Coins: " + MathF.Round(Statistics.Coins), Color.Gold, Game1.CoinSprite, Vector2.Zero, Vector2.One);
            }
        }

        internal static void ClearRoom()
        {
            SpawnSystem.Active = false;
            Game1.Entities.Clear();
        }

        internal static void CreateRoom()
        {
            CreatePlayer();

            switch (CurrentRoom)
            {
                case 0:
                    // Training Room
                    Game1.Entities.Add(new Anvil() { Position = new Vector2(256, 0) });
                    Game1.Entities.Add(new Dummy() { Position = new Vector2(-256, 0) });
                    break;

                case 1:
                case 2:
                case 3:
                    // Default Room
                    SpawnSystem.SetFrequency(3f - .6f * (CurrentRoom - 1));
                    SpawnSystem.Active = true;
                    break;

                case 4:
                    // Special Room
                    Game1.Entities.Add(new RatKing() { Position = new Vector2(Game1.ScreenSize.X / 2, 0) });
                    break;

                case 5:
                    // Boss Room
                    break;

                default:
                    // Endgame
                    goto case 0;
            }

        }

        internal static void GotoRoom(int number)
        {
            ClearRoom();
            CurrentRoom = number;
            CreateRoom();
        }

        internal static void CreatePlayer()
        {
            Game1.Entities.Add(new Player());
            Game1.Player = (Player)Game1.Entities[^1];
            Game1.Player.ViewPoint = Game1.Player.Position;
        }

        internal static void Restart()
        {
            ClearRoom();
            CurrentRoom = 0;
            Statistics.DefeatedCreatures = 0;
            CreateRoom();
        }

        internal static void NextRoom()
        {
            var currentHealth = Game1.Player.Health;
            var weapon = Game1.Player.weapon;
            ClearRoom();
            CurrentRoom++;
            CreateRoom();
            Game1.Player.Health = currentHealth;
            Game1.Player.weapon = weapon;
            weapon.owner = Game1.Player;
        }

        private static void DrawText(SpriteBatch spriteBatch, Vector2 position, string text, Color color)
        {
            spriteBatch.DrawString(Game1.ConsolasSmallFont, text, Camera.Zero + position + new Vector2(1, 1), Color.Black);
            spriteBatch.DrawString(Game1.ConsolasSmallFont, text, Camera.Zero + position, color);
        }

        private static void DrawText(SpriteBatch spriteBatch, Vector2 position, string text, Color color, Texture2D texture, Vector2 textureOffset, Vector2 scaleTexture)
        {
            spriteBatch.Draw(texture, Camera.Zero + position + textureOffset, null, Color.White, 0f, Vector2.Zero, scaleTexture, SpriteEffects.None, 1f);
            spriteBatch.DrawString(Game1.ConsolasSmallFont, text, Camera.Zero + position + new Vector2(texture.Width * scaleTexture.X + 6, 0) + new Vector2(textureOffset.X, 0) + new Vector2(1, 1), Color.Black);
            spriteBatch.DrawString(Game1.ConsolasSmallFont, text, Camera.Zero + position + new Vector2(texture.Width * scaleTexture.X + 6, 0) + new Vector2(textureOffset.X, 0), color);
        }

    }
}
