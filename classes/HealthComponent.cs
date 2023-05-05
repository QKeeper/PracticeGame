using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame01
{
    public class HealthComponent : Component
    {
        public float currentHealth;
        public float maximumHealth;
        public HealthComponent(float health)
        {
            maximumHealth = currentHealth = health;
        }

    }
}
