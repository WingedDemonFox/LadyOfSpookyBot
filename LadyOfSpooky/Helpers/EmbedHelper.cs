using Discord;
using LadyOfSpooky.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LadyOfSpooky.Helpers
{
    public static class EmbedHelper
    {
        public static async void DeleteMessage(IMessage message, TimeSpan timespan, string auditLogReason = null)
        {
            await Task.Delay(timespan);
            await message.DeleteAsync(new RequestOptions() { AuditLogReason = auditLogReason });
        }

        public static EmbedBuilder GetMonsterEmbed(Monster monster, string description = null)
        {
            EmbedBuilder builder = new();

            builder.WithTitle($"{monster.MonsterName}");
            if (description != null)
            {
                builder.WithDescription(description);
            }

            builder.AddField("Defense", $"{monster.Defense}", true);
            builder.AddField("Attack", $"{monster.Attack}", true);

            builder.AddField('\u200B'.ToString(), '\u200B', true);

            builder.AddField("Nr Attacks", $"{monster.NumberOfAttacks}", true);
            builder.AddField("HitProb", $"{monster.HitProb}", true);

            builder.WithFooter(new EmbedFooterBuilder().WithText($"Level: {monster.Level}"));

            builder.WithColor(LevelHelper.GetColorByLevel(monster.Level));

            return builder;
        }

        public static EmbedBuilder GetPlayerEmbed(Player player)
        {
            EmbedBuilder builder = new();

            builder.WithTitle($"{player.DiscordUserName}");
            builder.WithDescription($"Level {player.Level} {player.ChosenClass}");

            builder.AddField("Defense", "soon", true);
            builder.AddField("Attack", "soon", true);

            builder.AddField('\u200B'.ToString(), '\u200B', true);

            builder.AddField("Nr Attacks", "soon", true);
            builder.AddField("HitProb", "soon", true);

            builder.WithColor(LevelHelper.GetColorByLevel(player.Level));

            return builder;
        }
    }
}
