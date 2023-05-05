using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame01
{
    public class Entity
    {
        public Texture2D sprite;
        public List<Component> components = new();

        public void AddComponent(Component component)
        {
            components.Add(component);
            component.entity = this;
        }

        public void RemoveComponent(Component component)
        {
            components.Remove(component);
        }

        public T GetComponent<T>() where T : Component
        {
            foreach (Component component in components)
            {
                if (component.GetType().Equals(typeof(T)))
                {
                    return (T)component;
                }
            }
            return null;
        }
    }
}