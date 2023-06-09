using Microsoft.VisualBasic.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = Microsoft.Xna.Framework.Color;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Project1
{
    internal class Rat : Enemy
    {
        private Vector2 Target = Vector2.Zero;
        private readonly float StepFrequency = 3f;
        private float StepTimer = 0f;
        private readonly float HitDelay = 1f;
        private float HitDelayTimer = 0f;
        private float Rotation = 0f;
        private float Scale = 1f;
        private SpriteEffects Effects = SpriteEffects.None;

        internal Rat()
        {
            Sprite = Game1.RatSprite;

            MaxHealth = Health = 2.4f + 1.4f * (Manager.CurrentRoom - 1);

            MovementSpeed = 450f;
            Scale = new Random().NextFloat(.8f, 1.05f);
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Collide(gameTime);
            Knockback(gameTime);
            Movement(gameTime);
            HitPlayer(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            DrawShadow(spriteBatch);

            var influence = Vector2.Distance(Position, Target) < 64 ? Vector2.Distance(Position, Target) / 64 : 1;
            Rotation = Game1.Player.Health > 0 ? (MathF.Sin(Time * 36) / 9 * influence) : 0;
            Effects = Target.X > Position.X ? Effects = SpriteEffects.None : (Target.X < Position.X ? SpriteEffects.FlipHorizontally : Effects);
            spriteBatch.Draw(Sprite, Position, null, DefineColor(), Rotation, Center(Sprite), Scale, Effects, 0f);

            var margin = 4;
            var padding = 2;
            var barWidth = 3;
            spriteBatch.Draw(Game1.BlackSquareSprite, new Rectangle((int)Position.X - Sprite.Width / 2 - padding, (int)Position.Y + Sprite.Height / 2 + margin - padding, Sprite.Width + padding * 2, barWidth + padding * 2), new Color(Color.Black, 170));
            spriteBatch.Draw(Game1.WhiteSquareSprite, new Rectangle((int)Position.X - Sprite.Width / 2, (int)Position.Y + Sprite.Height / 2 + margin, (int)Lerp(0, Sprite.Width, Health / (MaxHealth)), barWidth), new Color(255, 77, 77));
        }

        internal override void Knockback(GameTime gameTime)
        {
            if (knockback.time <= 0) return;

            knockback.time -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            RunAway();
        }

        internal override void Destroy()
        {
            if (new Random().Next(100) > 50) SpawnSystem.SpawnEnemy(new Gnoll());

            Loot.Drop(Position, xp: new Random().Next(0, 2), coins: new Random().Next(0, 3));
            Ghost.Create(Position);

            Statistics.DefeatedCreatures++;
            base.Destroy();
        }

        private void Movement(GameTime gameTime)
        {
            if (Game1.Player.Health <= 0) return;
            if (Target == Vector2.Zero) Target = GetRandomPointAroundPlayer();

            else if (Vector2.Distance(Position, Target) < MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) Target = Position;

            var directionToTarget = Target - Position;
            var normalizedVector = (directionToTarget == Vector2.Zero) ? Vector2.Zero : Vector2.Normalize(directionToTarget);

            Position += normalizedVector * MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            StepTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (StepTimer > 0f) return;
            StepTimer += StepFrequency;
            Target = GetRandomPointAroundPlayer();
        }

        private Vector2 GetRandomPointAroundPlayer()
        {
            Random random = new();
            var distanceToPlayer = (int)Vector2.Distance(Position, Game1.Player.Position);
            var vectorToPlayer = Vector2.Normalize(Game1.Player.Position - Position);
            return Position + new Vector2(vectorToPlayer.X * random.Next(distanceToPlayer), vectorToPlayer.Y * random.Next(distanceToPlayer));
        }

        private void HitPlayer(GameTime gameTime)
        {
            HitDelayTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (knockback.time > 0f) return;
            if (HitDelayTimer > 0f) return;
            if (Vector2.Distance(Position, Game1.Player.Position) > Sprite.Width) return;
            if (Game1.Player.Health <= 0) return;
            HitDelayTimer = HitDelay;

            Game1.Player.Damage(1);
            Game1.Player.Knockback(Game1.Player.Position - Position, 125f, .2f);
            Game1.HitSound.Play();
            Particle.Create(Game1.HitParticle, Game1.Player.Position, new Random().Next(10, 20), new Color(216, 186, 145), 200f, 2);
            Game1.Entities.Add(new PopoutText(Game1.Player.Position, Game1.ConsolasBigFont, new Color(216, 186, 145), .9f, "-1"));

            RunAway();
        }

        private void RunAway()
        {
            Target = Position + Vector2.Normalize(Position - Game1.Player.Position) * 250f;
            StepTimer = StepFrequency;
        }

    }
}
