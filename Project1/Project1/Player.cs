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
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using System.Reflection.Metadata.Ecma335;

namespace Project1
{
    internal class Player : Entity
    {
        internal int health = 5;
        internal int extraHealth = 1;
        internal float movementSpeed = 300f;

        internal Item weapon = new();

        private Vector2 input = Vector2.Zero;
        private Vector2 acceleratedInput = Vector2.Zero;
        private readonly float acceleration = 20f;

        private float hitDelay = 0f;
        private readonly float reqAccuracy = .4f;

        SpriteEffects spriteEffects = SpriteEffects.None;

        internal Player()
        {
            sprite = Game1.PlayerSprite;
            shadow = Game1.ShadowE8;
            colliderRadius = sprite.Width / 2.5f;
        }

        internal override void Update(GameTime gameTime)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            Knockback(gameTime);
            Movement(gameTime);
            Hit(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            DrawShadow(spriteBatch);

            spriteEffects = (input.X > 0) ? SpriteEffects.None : ((input.X < 0) ? SpriteEffects.FlipHorizontally : spriteEffects);
            var rotation = Vector2.Distance(Vector2.Zero, acceleratedInput) * (MathF.Sin(time * 14) / 9);
            spriteBatch.Draw(sprite, position, null, DefineColor(), rotation, Center(sprite), 1f, spriteEffects, 0f);

            for (int i = 0; i < health + extraHealth; i++)
                if (i < health) spriteBatch.Draw(Game1.HeartSprite, new Vector2(16 + (8 + Game1.HeartSprite.Width) * i, 16), Color.White);
                else spriteBatch.Draw(Game1.ExtraHeartSprite, new Vector2(16 + (8 + Game1.HeartSprite.Width) * i, 16), Color.White);

        }

        internal void Movement(GameTime gameTime)
        {
            if (knockback.time > 0) return;

            KeyboardState keyboard = Keyboard.GetState();

            input = new((keyboard.IsKeyDown(Keys.D) ? 1 : 0) - (keyboard.IsKeyDown(Keys.A) ? 1 : 0), (keyboard.IsKeyDown(Keys.S) ? 1 : 0) - (keyboard.IsKeyDown(Keys.W) ? 1 : 0));
            acceleratedInput = Vector2.Lerp(acceleratedInput, (input == Vector2.Zero) ? new(0, 0) : Vector2.Normalize(input), acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds);

            position += acceleratedInput * movementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        internal void Hit(GameTime gameTime)
        {
            hitDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Mouse.GetState().LeftButton != ButtonState.Pressed) return;

            Vector2 mousePosition = new(Mouse.GetState().X, Mouse.GetState().Y);
            List<Entity> possibleTargets = new();
            Entity target = null;

            foreach (Entity entity in Game1.Entities.ToList())
            {
                if (entity is not Enemy) continue;

                if (Vector2.Distance(position, entity.position) > weapon.attackRange) continue;

                float accuracy = Vector2.Dot(Vector2.Normalize(mousePosition - position), Vector2.Normalize(entity.position - position));
                if (accuracy < reqAccuracy) continue;

                if ((entity as Enemy).health <= 0) continue;

                possibleTargets.Add(target = entity);
                foreach (Entity _entity in possibleTargets)
                    if (Vector2.Distance(position, _entity.position) < Vector2.Distance(position, target.position))
                        target = _entity;
            }

            if (hitDelay <= 0 && target is Enemy)
            {
                (target as Enemy).Damage(weapon.damage);
                target.Knockback(target.position - position, 100f, .2f);
                hitDelay = 1 / weapon.attackRate;
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
                    health--;

                if (health <= 0)
                    Destroy();
            }
        }
    }
}
