using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LadyOfSpooky.Helpers;
using LadyOfSpooky.Services;

namespace LadyOfSpooky.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly List<string> availableModules = new() { "InfoModule", "monster", "player" };

        [Command("help")]
        [Summary("Helps")]
        public async Task HelpAsync(string some = null)
        {
            var commands = CommandHandler.CommandList;

            EmbedBuilder builder = new();

            builder.WithTitle($"{Program.Client.CurrentUser.Username}");
            builder.WithDescription($"Here to help my minions {some}");
            builder.WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl());

            builder.AddField("Prefix", $"`{Global.Prefix}`", false);

            foreach (var module in availableModules)
            {
                var modModule = commands.Where(c => c.Module.Name == module && c.Preconditions.Count == 0).ToList();
                var mod = GetModuleString(modModule);
                builder.AddField(module, mod, false);
            }

            builder.WithCurrentTimestamp();
            builder.WithColor(Color.Red);

            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }

        [RequireOwner]
        [Command("adminhelp")]
        [Summary("Helps")]
        public async Task AdminHelpAsync(string some = null)
        {
            var commands = CommandHandler.CommandList;

            EmbedBuilder builder = new();

            builder.WithTitle($"{Program.Client.CurrentUser.Username}");
            builder.WithDescription($"Here to help my minions {some}");
            builder.WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl());

            builder.AddField("Prefix", $"`{Global.Prefix}`", false);

            foreach (var module in availableModules)
            {
                var modModule = commands.Where(c => c.Module.Name == module).ToList();
                var mod = GetModuleString(modModule);
                builder.AddField(module, mod, false);
            }

            builder.WithColor(Color.Red);

            var msg = await Context.Channel.SendMessageAsync("", false, builder.Build());
            EmbedHelper.DeleteMessage(msg, TimeSpan.FromSeconds(20));
        }


        private static string GetModuleString(List<CommandInfo> commandsModule)
        {
            var commandNameList = string.Empty;

            foreach (var command in commandsModule)
            {
                if (command.Module.Group != null)
                {
                    if (command.Name == "DetailAsync")
                    {
                        commandNameList += $"{command.Module.Group} <monstername> \n";
                        continue;
                    }
                    commandNameList += $"{command.Module.Group} {command.Name} \n";
                }
                else
                {
                    commandNameList += $"{command.Name} \n";
                }
            }

            return "```" + commandNameList + "```";
        }
    }
}
