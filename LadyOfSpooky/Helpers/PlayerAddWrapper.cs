using Discord;
using Discord.Rest;
using LadyOfSpooky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadyOfSpooky.Helpers
{
    public class PlayerAddWrapper
    {
        public PlayerAddWrapper(RestUserMessage msg, Player player)
        {
            Message = msg;
            PlayerHasToChoose = player;
        }

        public IMessage Message { get; set; }
        public Player PlayerHasToChoose { get; set; }
    }
}
