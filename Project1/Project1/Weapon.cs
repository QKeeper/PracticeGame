 using Microsoft.VisualBasic.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;

namespace Project1
{
    internal class Weapon : Entity
    {
        internal Entity owner;
        internal Type type;

        internal string name;
        internal string description;

        internal Vector2 offset = new Vector2(-24);
        internal Color color = Color.White;
        internal int direction = 1;

        // Damage
        internal (float value, int level, float minValue, float step) damage = (1, 1, 1, .65f);

        // Attach Rate
        internal (float value, int level, float minValue, float step) attackRate = (.8f, 1, .8f, .3f);

        // Movement Slowdown
        internal (float value, int level, float minValue, float step) movementSlowdown = (0, 1, 0, 3);

        // Attack Range
        internal (float value, int level, float minValue, float step) attackRange = (72, 1, 72, 12);

        // Knockback Force
        internal (float value, int level, float minValue, float step) knockbackForce = (24f, 1, 24f, 24f);

        // Hit Stun
        internal (float value, int level, float minValue, float step) knockbackTime = (.15f, 1, .15f, .07f);

        internal enum Type
        {
            sword,
            spear,
        }

        internal Weapon(Type type, int damageLevel, int attackRateLevel, int movementSlowdownLevel, int attackRangeLevel, int knockbackForceLevel, int knockbackTimeLevel)
        {
            damage.SetLevel(damageLevel);
            attackRate.SetLevel(attackRateLevel);
            movementSlowdown.SetLevel(movementSlowdownLevel);
            attackRange.SetLevel(attackRangeLevel);
            knockbackForce.SetLevel(knockbackForceLevel);
            knockbackTime.SetLevel(knockbackTimeLevel);
            this.type = type;

            name = type.ToString();
            name = ">> " + name[0].ToString().ToUpper() + name.Substring(1) + " <<";

            switch (type)
            {
                case Type.sword:
                    Sprite = Game1.SwordSprite;
                    break;
                case Type.spear:
                    Sprite = Game1.SpearSprite;
                    break;
            }
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MouseState mouse = Mouse.GetState();

            if (owner != null)
            {
                Position = owner.Position;

                if (owner is Player)
                {
                    direction = (owner.Position.X - Camera.MousePosition.X < 0) ? 1 : (owner.Position.X - Camera.MousePosition.X > 0) ? -1 : direction;
                }
            }

        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            var player = (owner as Player);

            // Animation for sword
            if (type == Type.sword)
                spriteBatch.Draw(Sprite, Position, null, color, .8f * player.HitDelayClamped * direction, Center(Sprite) + offset * new Vector2(direction, 1), .8f, direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 1f);

            // Animation for spear
            if (type == Type.spear)
                spriteBatch.Draw(Sprite, Position, null, color, .7f * direction, new Vector2(Sprite.Width * (direction == 1 ? .3f : .7f), Sprite.Height * .5f) + new Vector2((direction == 1 ? -24f : 24f) * player.HitDelayClamped, 12f * player.HitDelayClamped), .8f, direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 1f);

        }

        private int pointer = 0;
        internal void Upgrade()
        {
            switch (pointer)
            {
                case 0: damage.AddLevel(); break;
                case 1: attackRate.AddLevel(); break;
                case 2: attackRange.AddLevel(); break;
                case 3: movementSlowdown.AddLevel(); break;
                case 4: knockbackForce.AddLevel(); break;
                case 5: knockbackTime.AddLevel(); break;
            }

            pointer += new Random().Next(100) > 50 ? 0 : 1;
            if (pointer > 5) pointer -= 6;
        }

        internal static Type GetRandomType() => (Type)Enum.GetValues(typeof(Type)).GetValue(new Random().Next(Enum.GetValues(typeof(Type)).Length));
    }
}
