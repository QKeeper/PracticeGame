using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class Gnoll : Enemy
    {
        private Vector2 Direction = Vector2.Zero;
        private SpriteEffects SpriteEffects;
        private float Rotation;

        private float HitDelayTimer = 0f;
        private float HitDelay = .45f;

        internal Gnoll()
        {
            Sprite = Game1.GnollSprite;
            Shadow = Game1.ShadowR8;

            MaxHealth = Health = 4f + 2.6f * (Manager.CurrentRoom - 1);
            MovementSpeed = 250;
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

            SpriteEffects = Direction.X > 0 ? SpriteEffects.None : (Direction.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects);
            spriteBatch.Draw(Sprite, Position, null, DefineColor(), Rotation, Center(Sprite), 1f, SpriteEffects, 0f);

            var margin = 4;
            var padding = 2;
            var barWidth = 3;
            spriteBatch.Draw(Game1.BlackSquareSprite, new Rectangle((int)Position.X - Sprite.Width / 2 - padding, (int)Position.Y + Sprite.Height / 2 + margin - padding, Sprite.Width + padding * 2, barWidth + padding * 2), new Color(Color.Black, 170));
            spriteBatch.Draw(Game1.WhiteSquareSprite, new Rectangle((int)Position.X - Sprite.Width / 2, (int)Position.Y + Sprite.Height / 2 + margin, (int)Lerp(0, Sprite.Width, Health / (MaxHealth)), barWidth), new Color(255, 77, 77));
        }

        internal override void Destroy()
        {
            if (new Random().Next(100) > 50) SpawnSystem.SpawnEnemy(new Rat());

            Loot.Drop(Position, xp: new Random().Next(0, 4), coins: new Random().Next(1, 4));
            Ghost.Create(Position);

            Statistics.DefeatedCreatures++;
            base.Destroy();
        }

        internal void Movement(GameTime gameTime)
        {
            if (knockback.time > 0) return;

            if (Game1.Player.Health > 0)
            {
                Direction = Vector2.Normalize(Game1.Player.Position - Position);
                Rotation = MathF.Sin(Time * 14) / 9;
            }
            else
            {
                Direction = Vector2.Zero;
                Rotation = 0;
            }

            Position += Direction * MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        }

        internal void HitPlayer(GameTime gameTime)
        {
            if (HitDelayTimer > 0)
            {
                HitDelayTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                return;
            }

            if (knockback.time > 0) return;

            foreach (Entity entity in Game1.Entities.ToList())
            {
                if (entity is not Player) continue;
                if (Vector2.Distance(Position, entity.Position) > ColliderRadius + entity.ColliderRadius) continue;
                if (entity.knockback.time > 0) continue;

                (entity as Player).Damage(1);
                HitDelayTimer = HitDelay;
                entity.Knockback(entity.Position - Position, 175f, 0.2f);
                Knockback(Position - entity.Position, 175f, 0.25f);
                Particle.Create(Game1.HitParticle, Game1.Player.Position, new Random().Next(10, 20), new Color(170, 103, 17), 200f, 2);
                Game1.Entities.Add(new PopoutText(Game1.Player.Position, Game1.ConsolasBigFont, new Color(255, 159, 26), .9f, "-1"));
                Game1.HitSound.Play();
            }
        }

    }
}
