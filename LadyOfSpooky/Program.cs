using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LadyOfSpooky.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using LadyOfSpooky.Helpers;
using Microsoft.Extensions.Configuration.Json;

namespace LadyOfSpooky
{

    public class Program
    {
        private IConfiguration _config;
        public static DiscordSocketClient Client;

        public static void Main(string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {

            _config = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .Build();

            Global.OwnerId = Convert.ToUInt64(_config["OwnerId"]);
            Global.PlayersFile = _config["PlayersFile"];
            Global.MonstersFile = _config["MonstersFile"];
            Global.Version = _config["Version"];

#if DEBUG
            Global.Prefix = _config["PrefixDebug"];
#else
            Global.Prefix = _config["Prefix"];
#endif


            using var services = ConfigureServices();
            var client = services.GetRequiredService<DiscordSocketClient>();
            Client = client;

            client.Log += LogAsync;
            client.Ready += ReadyAsync;

            services.GetRequiredService<CommandService>().Log += LogAsync;
            var token = _config["DiscordToken"];
            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();
#if DEBUG
            await Client.SetGameAsync("dev mode", null, ActivityType.Listening);
#else
            await _client.SetGameAsync("the monsters", null, ActivityType.Watching);
#endif


            await services.GetRequiredService<CommandHandler>().InitializeAsync();
            services.GetRequiredService<ReactionHandler>().Initialize();

            EmojiHelper.InitEmojis();

            await Task.Delay(-1);
        }

        private Task LogAsync(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task ReadyAsync()
        {
            Console.WriteLine($"Connected as -> [{Client.CurrentUser}] :)");
            Global.BotId = Client.CurrentUser.Id;
            return Task.CompletedTask;
        }

        private static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<ReactionHandler>()
                .BuildServiceProvider();
        }
    }
}
