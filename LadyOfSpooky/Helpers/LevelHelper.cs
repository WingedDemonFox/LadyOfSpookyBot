using Discord;
using LadyOfSpooky.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadyOfSpooky.Helpers
{
    public static class LevelHelper
    {
        public static Color GetColorByLevel(int level)
        {
            return level switch
            {
                1 => Color.DarkBlue,
                2 => Color.Blue,
                3 => Color.Teal,
                4 => Color.DarkTeal,
                5 => Color.Green,
                6 => Color.DarkGreen,
                7 => Color.LightOrange,
                8 => Color.Orange,
                9 => Color.DarkOrange,
                10 => Color.Red,
                11 => Color.DarkRed,
                12 => Color.Magenta,
                13 => Color.DarkMagenta,
                14 => Color.Purple,
                15 => Color.DarkPurple,
                _ => Color.DarkerGrey,
            };
        }

        // returns a dictionary that containes xp requirements for each defined level. that data is saved in 'xpPerLevel.json'
        private static SortedDictionary<int, int> loadXpPerLevel()
        {
            var xpPerLevel = new SortedDictionary<int, int>();
            var json = JObject.Parse(File.ReadAllText("xpPerLevel.json"));
            var levels = json.ToObject<Dictionary<String, int>>();
            foreach (var level in levels)
            {
                xpPerLevel.Add(Convert.ToInt32(level.Key), level.Value);
            }
            return xpPerLevel;
        }

        // return the amoun of xp that is needed for a level up
        public static int getXpUntilNextLevel(Player player)
        {
            return loadXpPerLevel()[player.Level] - player.Exp;
        }

        // check if player can level up
        private static bool playerCanLevelUp(Player player)
        {
            if (player.Exp >= loadXpPerLevel()[player.Level])
            {
                return true;
            }
            return false;
        }

        // levels up the player if they have enough xp
        public static void checkCurrentXP(Player player)
        {
            while (playerCanLevelUp(player))
            {
                int xpRequired = loadXpPerLevel()[player.Level];
                if (player.Exp >= xpRequired)
                {
                    player.Level++;
                }
            }
        }
    }
}
