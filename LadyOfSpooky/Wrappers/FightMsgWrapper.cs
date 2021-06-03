using Discord;
using Discord.Rest;
using LadyOfSpooky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadyOfSpooky.Wrappers
{
    public class FightMsgWrapper
    {
        public FightMsgWrapper(RestUserMessage msg, Fight fight)
        {
            Message = msg;
            Fight = fight;
        }

        public IMessage Message { get; set; }
        public Fight Fight { get; set; }
    }
}
