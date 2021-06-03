using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LadyOfSpooky.Helpers;
using LadyOfSpooky.Models;
using LadyOfSpooky.Wrappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LadyOfSpooky.Modules
{
    [Group("player")]
    public class PlayerModule : ModuleBase<SocketCommandContext>
    {
        [Command("all")]
        [Summary("List all players")]
        public async Task AllPlayersAsync(string echo = null)
        {
            var players = DataHelper.GetPlayersFromFile();

            EmbedBuilder builder = new();
            builder.WithTitle($"All Players {echo}");

            var playersList = players.OrderBy(p => p.Level).ToList();

            var playerLvls = string.Empty;
            var playerClass = string.Empty;
            var playerName = string.Empty;

            foreach (var player in playersList)
            {
                playerLvls += $"{player.Level}\n";
                playerClass += $"{player.ChosenClass}\n";
                playerName += $"{player.Name}\n";
            }

            builder.AddField("Level", playerLvls, true);
            builder.AddField("Class", playerClass, true);
            builder.AddField("Player", playerName, true);
            builder.WithCurrentTimestamp();
            builder.WithColor(Color.LighterGrey);

            var msg = await Context.Channel.SendMessageAsync("", false, builder.Build());
            EmbedHelper.DeleteMessage(msg, TimeSpan.FromSeconds(20));
        }

        [Command("add", RunMode = RunMode.Async)]
        [Summary("add yourself as player and choose class")]
        public async Task AddPlayerAsync()
        {
            var user = Context.Message.Author as SocketGuildUser;

            var player = new Player() { DiscordUserId = user.Id, Name = user.Nickname ?? user.Username };

            var players = DataHelper.GetPlayersFromFile();
            if (!players.Where(p => p.DiscordUserId == user.Id).Any())
            {
                players.Add(player);
                DataHelper.WritePlayersToFile(players);
            }

            EmbedBuilder builder = new();

            builder.WithTitle($"Choose your class {player.Name}");

            builder.AddField("Wizard", $"{EmojiHelper.Emojis["Wizard"]}", true);
            builder.AddField("Fighter", $"{EmojiHelper.Emojis["Fighter"]}", true);
            builder.AddField("Tank", $"{EmojiHelper.Emojis["Tank"]}", true);

            var msg = await Context.Channel.SendMessageAsync("", false, builder.Build());
            await msg.AddReactionsAsync(new Emoji[] { EmojiHelper.Emojis["Wizard"], EmojiHelper.Emojis["Fighter"], EmojiHelper.Emojis["Tank"] });

            Global.SentMessagesWithInvokingUser.TryAdd(msg, Context.Message.Author.Id);
            Global.ChooseClassWrapper.Add(new PlayerAddWrapper(msg, player));
        }

        [Command("me")]
        [Summary("detail info for a player")]
        public async Task DetailAsync(string playerName = null)
        {
            var players = DataHelper.GetPlayersFromFile();

            var player = players.Where(m => m.DiscordUserId == Context.Message.Author.Id).SingleOrDefault();

            if (player == null)
            {
                await ReplyAsync($"Not a player {playerName}");
                return;
            }

            var embedBuilder = EmbedHelper.GetPlayerEmbed(player);
            var msg = await Context.Channel.SendMessageAsync("", false, embedBuilder.Build());
            EmbedHelper.DeleteMessage(msg, TimeSpan.FromSeconds(20));
        }
    }
}
