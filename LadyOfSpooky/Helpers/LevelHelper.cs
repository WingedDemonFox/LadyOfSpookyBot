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

        public static int CalcLevelByExp(int exp)
        {
            int lvl = 1;
            if (exp < 0)
            {
                lvl = 1;
            }
            else if (exp >= 165000)
            {
                lvl = 15;
            }
            else if (exp >= 140000)
            {
                lvl = 14;
            }
            else if (exp >= 120000)
            {
                lvl = 13;
            }
            else if (exp >= 100000)
            {
                lvl = 12;
            }
            else if (exp >= 85000)
            {
                lvl = 11;
            }
            else if (exp >= 64000)
            {
                lvl = 10;
            }
            else if (exp >= 48000)
            {
                lvl = 9;
            }
            else if (exp >= 34000)
            {
                lvl = 8;
            }
            else if (exp >= 23000)
            {
                lvl = 7;
            }
            else if (exp >= 14000)
            {
                lvl = 6;
            }
            else if (exp >= 6500)
            {
                lvl = 5;
            }
            else if (exp >= 2700)
            {
                lvl = 4;
            }
            else if (exp >= 900)
            {
                lvl = 3;
            }
            else if (exp >= 300)
            {
                lvl = 2;
            }
            return lvl;
        }
    }
}
