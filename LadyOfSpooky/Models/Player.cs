using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using static LadyOfSpooky.Models.Enums;

namespace LadyOfSpooky.Models
{
    public class Player
    {
        public decimal DiscordUserId { get; set; }
        public string DiscordUserName { get; set; }

        public Classes ChosenClass { get; set; }

        public int Level { get; set; } = 1;

        public int AttackMonster()
        {
            return ChosenClass switch
            {
                Classes.Fighter => 3,
                Classes.Tank => 2,
                Classes.Wizard => 5,
                _ => 1,
            };
        }
    }
}
