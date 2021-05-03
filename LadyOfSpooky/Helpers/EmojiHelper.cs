using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadyOfSpooky.Helpers
{
    public static class EmojiHelper
    {
        public static Dictionary<string, Emoji> Emojis { get; } = new Dictionary<string, Emoji>();

        public static void InitEmojis()
        {
            Emojis.Add("Attack", new Emoji("\u2694"));
            Emojis.Add("Defend", new Emoji("\uD83D\uDEE1"));
            Emojis.Add("Join", new Emoji("🤝"));
            Emojis.Add("Flee", new Emoji("🏃‍♀️"));
            Emojis.Add("Ghost", new Emoji("\uD83D\uDC7B"));
            Emojis.Add("Skull", new Emoji("\uD83D\uDC80"));
            Emojis.Add("Wizard", new Emoji("🧙"));
            Emojis.Add("Fighter", new Emoji("💪"));
            Emojis.Add("Tank", new Emoji("\uD83D\uDEE1"));
        }
    }
}
