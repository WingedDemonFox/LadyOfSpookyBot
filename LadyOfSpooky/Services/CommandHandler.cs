using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using LadyOfSpooky.Helpers;

namespace LadyOfSpooky.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public static List<CommandInfo> CommandList { get; set; } = new();

        public CommandHandler(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            _commands.CommandExecuted += CommandExecuteAsync;
            _client.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            if (rawMessage is not SocketUserMessage message)
            {
                return;
            }

            if (message.Source != MessageSource.User)
            {
                return;
            }

            var argPos = 0;

            if (!(message.HasMentionPrefix(_client.CurrentUser, ref argPos) || message.HasStringPrefix(Global.Prefix, ref argPos)) || message.Author.IsBot)
            {
                return;
            }

            CommandList = _commands.Commands.ToList();

            var context = new SocketCommandContext(_client, message);


            await _commands.ExecuteAsync(context, argPos, _services);
        }

        public static async Task CommandExecuteAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!command.IsSpecified)
            {
                await context.Message.AddReactionAsync(EmojiHelper.Emojis["Skull"]);
                System.Console.WriteLine($"Command failed to execute for [{context.User.Username}] <-> [{result.ErrorReason}]!");
                // check if prefix is set
                if (String.IsNullOrWhiteSpace(Global.Prefix))
                {
                    Console.WriteLine("Your prefix seems to not be set. Please check and try again.");
                }
                return;
            }

            if (result.IsSuccess)
            {
                System.Console.WriteLine($"Command [{command.Value.Name}] executed for -> [{context.User.Username}]");
                EmbedHelper.DeleteMessage(context.Message, TimeSpan.FromSeconds(2));
                return;
            }

            await context.Message.AddReactionAsync(EmojiHelper.Emojis["Skull"]);
        }
    }
}
