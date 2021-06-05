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

            builder.WithTitle($"{monster.Name}");
            if (description != null)
            {
                builder.WithDescription(description);
            }

            builder.AddField("Defense", $"{monster.Defense}", true);
            builder.AddField("Attack", $"{monster.Attack}", true);
            builder.AddField("\u200B", '\u200B', true);

            builder.AddField("Nr Attacks", $"{monster.NumberOfAttacks}", true);
            builder.AddField("HitProb", $"{monster.HitProbability}", true);
            builder.AddField("\u200B", '\u200B', true);

            builder.WithFooter(new EmbedFooterBuilder().WithText($"Level: {monster.Level}"));

            if (!string.IsNullOrEmpty(monster.PicUrl))
            {
                builder.WithThumbnailUrl(monster.PicUrl);
            }

            builder.WithColor(LevelHelper.GetColorByLevel(monster.Level));

            return builder;
        }

        public static EmbedBuilder GetPlayerEmbed(Player player)
        {
            EmbedBuilder builder = new();

            builder.WithTitle($"{player.Name}");
            builder.WithDescription($"Level {player.Level} - {player.ChosenClass} - {player.Exp}xp");

            builder.AddField($"XP until level {player.Level+1}", $"{LevelHelper.GetXpUntilNextLevel(player)}");
            builder.AddField($"Base XP for level {player.Level}", $"{LevelHelper.GetBaseXp(player.Level)}");

            builder.AddField("Defense", $"{player.Defense}", true);
            builder.AddField("Attack", $"{player.Attack}", true);
            builder.AddField("\u200B", '\u200B', true);

            builder.AddField("Health", $"{player.Health}", true);
            builder.AddField("HitProb", $"{player.HitProbability}", true);
            builder.AddField("\u200B", '\u200B', true);

            builder.WithColor(LevelHelper.GetColorByLevel(player.Level));

            return builder;
        }

        public static EmbedBuilder GetFightEmbed(Fight fight)
        {
            EmbedBuilder builder = new();

            builder.WithTitle($"Fight against {fight.Monster.Name}");
            builder.WithDescription($"Group of {fight.Players.Count}");

            builder.AddField($"Health of {fight.Monster.Name}", $"{fight.Monster.Health}", false);

            var fighterName = string.Empty;
            var fighterClass = string.Empty;
            var fighterHealth = string.Empty;

            foreach (var fighter in fight.Players)
            {
                fighterName += $"{fighter.Name}\n";
                fighterClass += $"{fighter.ChosenClass}\n";
                fighterHealth += $"{fighter.Health}\n";
            }

            builder.AddField("Fighter", fighterName, true);
            builder.AddField("Class", fighterClass, true);
            builder.AddField("Health", fighterHealth, true);

            builder.AddField("Active Player", $"{fight.ActivePlayer.Name}", false);

            builder.WithColor(LevelHelper.GetColorByLevel(fight.Monster.Level));

            return builder;
        }

        public static EmbedBuilder GetFightWonEmbed(Fight fight)
        {
            EmbedBuilder builder = new();

            builder.WithTitle($"Won fight against {fight.Monster.Name}");
            builder.WithDescription($"{fight.ActivePlayer.Name} slayed {fight.Monster.Name}");

            builder.AddField("Loot", $"{fight.ActivePlayer.Name} gained {fight.AwarderXp} exp", false);

            builder.WithColor(LevelHelper.GetColorByLevel(fight.Monster.Level));
            if (!string.IsNullOrEmpty(fight.Monster.PicUrl))
            {
                builder.WithThumbnailUrl(fight.Monster.PicUrl);
            }
            return builder;
        }

        public static EmbedBuilder GetFightGameOverEmbed(Fight fight)
        {
            EmbedBuilder builder = new();

            builder.WithTitle($"Lost fight against {fight.Monster.Name}");
            builder.WithDescription($"{fight.ActivePlayer.Name} was killed by {fight.Monster.Name}");

            builder.AddField("Lost", $"{fight.ActivePlayer.Name} lost {fight.AwarderXp} exp", false);

            builder.WithColor(LevelHelper.GetColorByLevel(fight.Monster.Level));
            if (!string.IsNullOrEmpty(fight.Monster.PicUrl))
            {
                builder.WithThumbnailUrl(fight.Monster.PicUrl);
            }

            return builder;
        }

    }
}
