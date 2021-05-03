using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadyOfSpooky.Models
{
    public class Monster
    {
        public string MonsterName { get; set; }
        public int Defense { get; set; }
        public int Attack { get; set; }
        public int NumberOfAttacks { get; set; }
        public int HitProb { get; set; }
        public int Level { get; set; }

        public int AttackPlayer()
        {
            if (AttackHits())
            {
                return Attack;
            }
            return 0;
        }

        private bool AttackHits()
        {
            Random ran = new();
            var hitRate = ran.Next(0, 100);
            if (hitRate <= HitProb)
            {
                return true;
            }
            return false;
        }
    }
}
