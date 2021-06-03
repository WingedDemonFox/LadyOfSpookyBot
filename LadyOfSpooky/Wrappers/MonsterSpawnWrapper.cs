using Discord;
using Discord.Rest;
using LadyOfSpooky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadyOfSpooky.Wrappers
{
    public class MonsterSpawnWrapper
    {
        public MonsterSpawnWrapper(RestUserMessage msg, Monster monster)
        {
            Message = msg;
            MonsterSpawned = monster;
        }

        public IMessage Message { get; set; }
        public Monster MonsterSpawned { get; set; }
        public List<Player> PlayersInFight { get; set; } = new List<Player>();
    }
}
