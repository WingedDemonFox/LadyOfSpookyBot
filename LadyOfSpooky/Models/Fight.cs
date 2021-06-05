using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LadyOfSpooky.Models.Enums;

namespace LadyOfSpooky.Models
{
    public class Fight
    {
        public Monster Monster { get; set; }
        public List<Player> Players { get; set; }
        public Player ActivePlayer { get; private set; }

        public int Turn { get; set; }
        public FightStatus FightStatus { get; set; } = FightStatus.Ongoing;
        public int AwarderXp { get; set; }

        public Fight(Monster monster, List<Player> players)
        {
            Monster = monster;
            Players = players;
            ActivePlayer = players.First();
            Turn = 0;
        }

        public void TurnAttack()
        {
            Monster.Health -= ActivePlayer.DealtDamage(Monster.Defense);
            ActivePlayer.Health -= Monster.DealtDamage(ActivePlayer.Defense);

            if (Monster.Health <= 0)
            {
                FightStatus = FightStatus.Win;
            }
            else if (ActivePlayer.Health <= 0)
            {
                FightStatus = FightStatus.Lose;
            }
        }
    }
}
