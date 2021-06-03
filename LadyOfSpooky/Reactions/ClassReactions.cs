using Discord;
using LadyOfSpooky.Helpers;
using System.Linq;
using static LadyOfSpooky.Models.Enums;

namespace LadyOfSpooky.Reactions
{
    public class ClassReactions
    {
        public static void AddClassToPlayer(ulong userId, IEmote emoji)
        {
            UpdateClassOfPlayer(userId, emoji);
        }

        private static void UpdateClassOfPlayer(ulong userId, IEmote emoji)
        {
            var players = DataHelper.GetPlayersFromFile();
            var player = players.Where(p => p.DiscordUserId == userId).SingleOrDefault();

            player.ChosenClass = GetClassByEmoji(emoji);
            player.UpdateClassValues();
            DataHelper.WritePlayersToFile(players);
        }

        private static Classes GetClassByEmoji(IEmote emoji)
        {
            if (emoji.Name == "\U0001f9d9")
            {
                return Classes.Wizard;
            }

            if (emoji.Name == "💪")
            {
                return Classes.Fighter;
            }


            return Classes.Tank;
        }
    }
}
