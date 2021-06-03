using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using LadyOfSpooky.Models;
using LadyOfSpooky.Helpers;
using LadyOfSpooky.Wrappers;

namespace LadyOfSpooky
{
    public static class Global
    {
        public static string Version { get; set; } = string.Empty;
        public static ulong BotId { get; set; } = 0;
        public static ulong OwnerId { get; set; } = 0;
        public static string PlayersFile { get; set; } = string.Empty;
        public static string MonstersFile { get; set; } = string.Empty;


        public static Dictionary<IMessage, ulong> SentMessagesWithInvokingUser { get; } = new Dictionary<IMessage, ulong>();

        public static List<MonsterSpawnWrapper> SpawnedMonsters { get; } = new List<MonsterSpawnWrapper>();
        public static List<PlayerAddWrapper> ChooseClassWrapper { get; } = new List<PlayerAddWrapper>();

        public static string Prefix { get; set; } = string.Empty;

        public static List<FightMsgWrapper> OnGoingFight { get; } = new List<FightMsgWrapper>();
    }
}
