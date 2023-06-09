using Microsoft.VisualBasic.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Point = Microsoft.Xna.Framework.Point;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using System.Reflection.Metadata.Ecma335;
using SharpDX;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Project1
{
    internal class Player : Entity
    {
        internal int Health = 3;
        internal int extraHealth = 0;

        internal Weapon weapon;

        internal Vector2 Input { get; private set; }
        private Vector2 AcceleratedInput = Vector2.Zero;
        private readonly float Acceleration = 12f;

        internal Vector2 ViewPoint { get; set; }
        internal readonly float ViewPointAcceleration = 2f;

        internal Vector2 SnapPoint { get; set; }

        internal float HitDelay = 0f;
        internal float HitDelayClamped = 0f;
        private readonly float DamageError = .15f;
        private readonly float ReqAccuracy = .4f;

        SpriteEffects spriteEffects = SpriteEffects.None;

        internal Player()
        {
            Sprite = Game1.PlayerSprite;
            MovementSpeed = 300f;

            weapon = new(Weapon.GetRandomType(),
                         damageLevel: 1,
                         attackRateLevel: 1,
                         movementSlowdownLevel: 1,
                         attackRangeLevel: 1,
                         knockbackForceLevel: 1,
                         knockbackTimeLevel: 1) { owner = this };
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Movement(gameTime);
            ViewPointUpdate(gameTime);
            SnapPointUpdate();
            weapon.Update(gameTime);
            Knockback(gameTime);
            Hit(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            DrawShadow(spriteBatch);

            // Display Attack Range
            // spriteBatch.Draw(Game1.CircleSprite, new Rectangle((int)(Position.X - (weapon.attackRange.value + colliderRadius)), (int)(Position.Y - (weapon.attackRange.value + colliderRadius)), (int)(weapon.attackRange.value + colliderRadius) * 2, (int)(weapon.attackRange.value + colliderRadius) * 2), new Color(Color.Black, 50));

            spriteEffects = (Position.X - Camera.MousePosition.X < 0) ? SpriteEffects.None : ((Position.X - Camera.MousePosition.X > 0) ? SpriteEffects.FlipHorizontally : spriteEffects);
            var rotation = Vector2.Distance(Vector2.Zero, AcceleratedInput) * (MathF.Sin(Time * 18) / 9);

            spriteBatch.Draw(Sprite, Position, null, DefineColor(), rotation, Center(Sprite), 1f, spriteEffects, 0f);

            weapon.Draw(spriteBatch);

            var margin = 4;
            var padding = 2;
            var barWidth = 3;
            spriteBatch.Draw(Game1.BlackSquareSprite, new Rectangle((int)Position.X - Sprite.Width / 2 - padding, (int)Position.Y + Sprite.Height / 2 + margin - padding, Sprite.Width + padding * 2, barWidth + padding * 2), new Color(Color.Black, 170));
            spriteBatch.Draw(Game1.WhiteSquareSprite, new Rectangle((int)Position.X - Sprite.Width / 2, (int)Position.Y + Sprite.Height / 2 + margin, Sprite.Width - (int)Lerp(0, Sprite.Width,  HitDelay / (1 / weapon.attackRate.value)), barWidth), new Color(Color.Gold, 255));

            for (int i = 0; i < Health + extraHealth; i++)
                if (i < Health) spriteBatch.Draw(Game1.HeartSprite, Camera.Zero + new Vector2(16 + (8 + Game1.HeartSprite.Width) * i, 16), Color.White);
                else spriteBatch.Draw(Game1.ExtraHeartSprite, Camera.Zero + new Vector2(16 + (8 + Game1.HeartSprite.Width) * i, 16), Color.White);

            // var text = string.Format("{0}\nDmg\nAtkRt\nAtkRng\nMvmtSl\nKnbkFrc\nKnbkTm", weapon.name);
            var text = string.Format("{0}\nDamage\nAttack Rate\nAttack Range\nMovement Slowdown\nKnockback Force\nKnockback Time", weapon.name);
            var position = Camera.Zero + new Vector2(24, Game1.ScreenSize.Y - 176);
            var maximumLevel = Math.Max(weapon.damage.level, Math.Max(weapon.attackRate.level, Math.Max(weapon.attackRange.level, Math.Max(weapon.movementSlowdown.level, Math.Max(weapon.knockbackForce.level, Math.Max(weapon.knockbackTime.level, 0))))));
            spriteBatch.Draw(Game1.BlackSquareSprite, new Rectangle((int)position.X - 8, (int)position.Y - 8, Math.Max(184, maximumLevel * (3 + Game1.CellSprite.Width) + 16), 168), new Color(Color.Black, 45));

            for (int i = 0; i < weapon.damage.level; i++) spriteBatch.Draw(Game1.CellSprite, Camera.Zero + new Vector2(24 + i * (3 + Game1.CellSprite.Width), Game1.ScreenSize.Y - 144), Color.White);
            for (int i = 0; i < weapon.attackRate.level; i++) spriteBatch.Draw(Game1.CellSprite, Camera.Zero + new Vector2(24 + i * (3 + Game1.CellSprite.Width), Game1.ScreenSize.Y - 122), Color.White);
            for (int i = 0; i < weapon.attackRange.level; i++) spriteBatch.Draw(Game1.CellSprite, Camera.Zero + new Vector2(24 + i * (3 + Game1.CellSprite.Width), Game1.ScreenSize.Y - 100), Color.White);
            for (int i = 0; i < weapon.movementSlowdown.level; i++) spriteBatch.Draw(Game1.CellSprite, Camera.Zero + new Vector2(24 + i * (3 + Game1.CellSprite.Width), Game1.ScreenSize.Y - 78), Color.White);
            for (int i = 0; i < weapon.knockbackForce.level; i++) spriteBatch.Draw(Game1.CellSprite, Camera.Zero + new Vector2(24 + i * (3 + Game1.CellSprite.Width), Game1.ScreenSize.Y - 56), Color.White);
            for (int i = 0; i < weapon.knockbackTime.level; i++) spriteBatch.Draw(Game1.CellSprite, Camera.Zero + new Vector2(24 + i * (3 + Game1.CellSprite.Width), Game1.ScreenSize.Y - 34), Color.White);

            spriteBatch.DrawString(Game1.ConsolasSmallFont, text, position + new Vector2(2, 2), Color.Black);;
            spriteBatch.DrawString(Game1.ConsolasSmallFont, text, position, Color.White);
        }

        internal void Movement(GameTime gameTime)
        {
            if (knockback.time > 0) return;

            KeyboardState keyboard = Keyboard.GetState();

            Input = new((keyboard.IsKeyDown(Keys.D) ? 1 : 0) - (keyboard.IsKeyDown(Keys.A) ? 1 : 0), (keyboard.IsKeyDown(Keys.S) ? 1 : 0) - (keyboard.IsKeyDown(Keys.W) ? 1 : 0));
            AcceleratedInput = Vector2.Lerp(AcceleratedInput, (Input == Vector2.Zero) ? new(0, 0) : Vector2.Normalize(Input), Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds);

            Position += AcceleratedInput * MovementSpeed * (100 - weapon.movementSlowdown.value) * .01f * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        internal void Hit(GameTime gameTime)
        {
            HitDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            HitDelayClamped = HitDelay * weapon.attackRate.value;
            if (HitDelay < 0) HitDelay = 0;

            if (Mouse.GetState().LeftButton != ButtonState.Pressed) return;

            Vector2 mousePosition = Camera.Zero + Mouse.GetState().Position.ToVector2();
            List<Enemy> possibleTargets = new();
            Enemy target = null;

            foreach (Entity entity in Game1.Entities.ToList())
            {
                if (entity is not Enemy) continue;

                if (Vector2.Distance(Position, entity.Position) > weapon.attackRange.value + entity.ColliderRadius) continue;

                float accuracy = Vector2.Dot(Vector2.Normalize(mousePosition - Position), Vector2.Normalize(entity.Position - Position));
                if (accuracy < ReqAccuracy) continue;

                if ((entity as Enemy).Health <= 0) continue;

                possibleTargets.Add(target = entity as Enemy);
            }

            if (HitDelay <= 0)
            {
                foreach (Enemy entity in possibleTargets)
                    if (Vector2.Distance(mousePosition, entity.Position) < Vector2.Distance(Position, target.Position))
                        target = entity;
                if (target == null) return;

                target.Damage(weapon.damage.value);
                target.Knockback(target.Position - Position, weapon.knockbackForce.value, weapon.knockbackTime.value);
                HitDelay = 1 / weapon.attackRate.value;
                Particle.Create(Game1.HitParticle, target.Position, new Random().Next(12, 18), new Color(170, 52, 52), 250f, 3);
                Game1.Entities.Add(new PopoutText(target.Position, Game1.ConsolasBigFont, new Color(255, 77, 77), .8f, "-" + MathF.Round(weapon.damage.value * new Random().NextFloat(1 - DamageError, 1 + DamageError), 2)));
                Game1.HitSound.Play();
            }

        }

        internal void Damage(float amount)
        {
            for (var i = 0; i < amount; i++)
            {
                if (extraHealth > 0)
                    extraHealth--;
                else
                    Health--;

                if (Health <= 0)
                    Destroy();
            }
        }

        internal void ViewPointUpdate(GameTime gameTime)
        {
            ViewPoint = Vector2.Lerp(ViewPoint, Position, ViewPointAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        internal void SnapPointUpdate()
        {
            var offset = new Vector2(ViewPoint.X % Background.Image.Width, ViewPoint.Y % Background.Image.Height);
            SnapPoint = Camera.Zero - offset;
        }
    }
}
