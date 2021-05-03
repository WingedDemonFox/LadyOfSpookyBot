using Discord;
using System;
using System.Collections.Generic;
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
    }
}
