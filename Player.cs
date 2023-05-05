using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Monogame01
{
    public class Player : Entity
    {
        public Player(float health)
        {
            this.AddComponent(new HealthComponent(health));
        }
        
        //Movement Script
    }
}
