using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace LadyOfSpooky.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("info")]
        [Summary("Tale of lady")]
        public async Task InfoAsync()
        {
            EmbedBuilder builder = new();

            builder.WithTitle($"{Program.Client.CurrentUser.Username}");
            builder.WithDescription($"I am the {Program.Client.CurrentUser.Username} and I command legions of monsters.");

            builder.WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl());

            builder.AddField("Version", $"`{Global.Version}`", false);
            builder.AddField("Source", "https://github.com/WingedDemonFox/LadyOfSpookyBot", false);

            builder.WithColor(Color.Red);
            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }
    }
}
