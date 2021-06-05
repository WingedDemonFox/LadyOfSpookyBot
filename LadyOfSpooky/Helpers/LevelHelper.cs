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

        public static int GetBaseXp(int level)
        {
            return XpToLevelUp(level - 1);
        }

        // returns the total amount of xp needed for next leve
        public static int XpToLevelUp(int level)
        {
            return DataHelper.GetXpPerLevelFromFile()[level];
        }

        // returns the amount of xp left that is needed for a level up
        public static int GetXpUntilNextLevel(Player player)
        {
            return XpToLevelUp(player.Level) - player.Exp;
        }

        // check if player can level up
        private static bool CanPlayerLevelUp(Player player)
        {
            if (player.Exp >= XpToLevelUp(player.Level))
            {
                return true;
            }
            return false;
        }

        // levels up the player if they have enough xp
        public static void CheckCurrentXP(Player player)
        {
            while (CanPlayerLevelUp(player))
            {
                player.Level++;
            }
        }
    }
}
