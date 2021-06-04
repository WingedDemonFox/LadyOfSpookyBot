﻿using System;
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
            if (ChosenClass == Classes.Wizard)
            {
                Attack = 12 + Level;
                Defense = 5 + Level;
                Health = 8 + (2 * Level);
                HitProbability = 60;
            }
            else if (ChosenClass == Classes.Tank)
            {
                Attack = 5 + Level;
                Defense = 12 + Level;
                Health = 12 + (2 * Level);
                HitProbability = 90;
            }
            else if (ChosenClass == Classes.Fighter)
            {
                Attack = 8 + Level;
                Defense = 8 + Level;
                Health = 10 + (2 * Level);
                HitProbability = 85;
            }
        }

        public void AwardXp(int expAmount)
        {
            int newExp = Exp + expAmount;
            // if newExp < 0 then 0 else newExp
            Exp = (newExp < 0) ? 0 : newExp;
            LevelHelper.checkCurrentXP(this);
            UpdateClassValues();
        }
    }
}
