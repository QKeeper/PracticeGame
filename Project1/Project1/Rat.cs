using Microsoft.VisualBasic.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;

namespace Project1
{
    internal class Rat : Enemy
    {
        private Vector2 target = Vector2.Zero;
        private readonly float stepFrequency = 3f;
        private float stepTimer = 0f;
        private readonly float hitDelay = 1f;
        private float hitDelayTimer = 0f;
        private float rotation = 0f;
        private SpriteEffects spriteEffects = SpriteEffects.None;

        internal Rat()
        {
            sprite = Game1.RatSprite;
            shadow = Game1.ShadowR8;
            colliderRadius = sprite.Width / 2.5f;

            maxHealth = health = 3;

            movementSpeed = 450f;
        }

        internal override void Update(GameTime gameTime)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            Collide(gameTime);
            Knockback(gameTime);
            Movement(gameTime);
            HitPlayer(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteEffects = target.X > position.X ? spriteEffects = SpriteEffects.None : (target.X < position.X ? SpriteEffects.FlipHorizontally : spriteEffects);

            DrawShadow(spriteBatch);

            var influence = Vector2.Distance(position, target) < 64 ? Vector2.Distance(position, target) / 64 : 1;
            rotation = Game1.Player.health > 0 ? (MathF.Sin(time * 36) / 9 * influence) : 0;
            spriteBatch.Draw(sprite, position, null, DefineColor(), rotation, Center(sprite), 1f, spriteEffects, 0f);
        }

        internal override void Knockback(GameTime gameTime)
        {
            if (knockback.time <= 0) return;

            knockback.time -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            RunAway();
        }

        internal override void Destroy()
        {
            Random random = new();
            DropLoot(position, random.Next(2) + 1);
            base.Destroy();
        }

        private void Movement(GameTime gameTime)
        {
            if (Game1.Player.health <= 0) return;
            if (target == Vector2.Zero) target = GetRandomPointAroundPlayer();

            else if (Vector2.Distance(position, target) < movementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) target = position;

            var directionToTarget = target - position;
            var normalizedVector = (directionToTarget == Vector2.Zero) ? Vector2.Zero : Vector2.Normalize(directionToTarget);

            position += normalizedVector * movementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            stepTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (stepTimer > 0f) return;
            stepTimer += stepFrequency;
            target = GetRandomPointAroundPlayer();
        }

        private Vector2 GetRandomPointAroundPlayer()
        {
            Random random = new();
            var distanceToPlayer = (int)Vector2.Distance(position, Game1.Player.position);
            var vectorToPlayer = Vector2.Normalize(Game1.Player.position - position);
            return position + new Vector2(vectorToPlayer.X * random.Next(distanceToPlayer), vectorToPlayer.Y * random.Next(distanceToPlayer));
        }

        private void HitPlayer(GameTime gameTime)
        {
            hitDelayTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (knockback.time > 0f) return;
            if (hitDelayTimer > 0f) return;
            if (Vector2.Distance(position, Game1.Player.position) > sprite.Width) return;
            if (Game1.Player.health <= 0) return;
            hitDelayTimer = hitDelay;

            Game1.Player.Damage(1);
            Game1.Player.Knockback(Game1.Player.position - position, 125f, .2f);
            Game1.HitSound.Play();

            RunAway();
        }

        private void RunAway()
        {
            target = position + Vector2.Normalize(position - Game1.Player.position) * 250f;
            stepTimer = stepFrequency;
        }

    }
}
