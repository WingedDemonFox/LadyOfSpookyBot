using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadyOfSpooky.Models
{
    public abstract class BaseFighter
    {
        public int Attack { get; set; } = 1;
        public int Defense { get; set; } = 1;
        public int Health { get; set; } = 1;
        public int HitProbability { get; set; } = 50;
        public int Level { get; set; } = 1;
        public int Exp { get; set; } = 0;

        public int WinCounter = 0;
        public int LostCounter = 0;

        public string Name { get; set; } = string.Empty;

        private bool AttackHits(int enemyDefense)
        {
            Random ran = new();
            var hitRate = ran.Next(0, 100);
            if (hitRate <= HitProbability - enemyDefense)
            {
                return true;
            }
            return false;
        }

        public int DealtDamage(int playerDefense)
        {
            if (AttackHits(playerDefense))
            {
                return Attack;
            }
            return 0;
        }
    }
}
