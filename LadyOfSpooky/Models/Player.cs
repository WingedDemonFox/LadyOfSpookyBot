using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using LadyOfSpooky.Helpers;
using static LadyOfSpooky.Models.Enums;

namespace LadyOfSpooky.Models
{
    public class Player : BaseFighter
    {
        public decimal DiscordUserId { get; set; }
        public Classes ChosenClass { get; set; } = Classes.Civil;

        public Player()
        {
            UpdateClassValues();
        }

        public void UpdateClassValues()
        {

            switch (ChosenClass)
            {
                case Classes.Wizard:
                    Attack = 12 + Level;
                    Defense = 5 + Level;
                    Health = 8 + (2 * Level);
                    HitProbability = 60;
                    break;
                case Classes.Tank:
                    Attack = 5 + Level;
                    Defense = 12 + Level;
                    Health = 12 + (2 * Level);
                    HitProbability = 90;
                    break;
                case Classes.Fighter:
                    Attack = 8 + Level;
                    Defense = 8 + Level;
                    Health = 10 + (2 * Level);
                    HitProbability = 85;
                    break;
                case Classes.Civil:
                    break;
                default:
                    break;
            }
        }

        public int GetAwardedXp(int expAmount)
        {
            int newPlayerExp = Exp + expAmount;
            int xpForCurrentLevel = LevelHelper.XpToLevelUp(Level - 1);
            // don't subtract xp if its at the lowest amount of the current level
            if (newPlayerExp <= xpForCurrentLevel)
            {
                newPlayerExp = xpForCurrentLevel;
            }
            int awardedXp = newPlayerExp - Exp;
            Exp = newPlayerExp;
            LevelHelper.CheckCurrentXP(this);
            UpdateClassValues();
            return awardedXp;
        }
    }
}
