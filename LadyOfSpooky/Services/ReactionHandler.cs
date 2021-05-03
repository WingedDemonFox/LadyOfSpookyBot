using Discord;
using Discord.WebSocket;
using LadyOfSpooky.Reactions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadyOfSpooky.Services
{
    public class ReactionHandler
    {
        private readonly IMessage Message;
        private readonly SocketReaction SocketReaction;

        private readonly DiscordSocketClient _client;

        public ReactionHandler(IServiceProvider services)
        {
            _client = services.GetRequiredService<DiscordSocketClient>();
            _client.ReactionAdded += ReactionAddedAsync;
        }

        public ReactionHandler(IMessage message, SocketReaction socketReaction)
        {
            Message = message;
            SocketReaction = socketReaction;
        }

        public void Initialize()
        {
            return;
        }

        private async Task ReactionAddedAsync(Cacheable<IUserMessage, ulong> argMessage, ISocketMessageChannel socketMessageChannel, SocketReaction argReaction)
        {
            HandleReaction(argMessage, socketMessageChannel, argReaction);
        }


        private static async void HandleReaction(Cacheable<IUserMessage, ulong> argMessage, ISocketMessageChannel socketMessageChannel, SocketReaction argReaction)
        {
            IMessage currentMessage = null;
            if (argMessage.HasValue)
            {
                currentMessage = argMessage.Value;
            }
            else
            {
                currentMessage = await socketMessageChannel.GetMessageAsync(argMessage.Id);
            }

            var messagePair = Global.SentMessagesWithInvokingUser.Where(m => m.Key.Id == currentMessage.Id).ToList().FirstOrDefault();

            if (argReaction.UserId != Global.BotId && !messagePair.Equals(default(KeyValuePair<IMessage, ulong>)) && messagePair.Value == argReaction.UserId)
            {
                Global.SentMessagesWithInvokingUser.Remove(messagePair.Key);
                ReactionHandler reactionHandler = new(currentMessage, argReaction);
                reactionHandler.Run();
            }
        }

        private async void Run()
        {
            var chooseClassMsg = Global.ChooseClassWrapper.Where(m => m.Message.Id == Message.Id).SingleOrDefault();
            if (chooseClassMsg != null)
            {
                ClassReactions.AddClassToPlayer(SocketReaction.UserId, SocketReaction.Emote);
                Global.ChooseClassWrapper.Remove(chooseClassMsg);
                await chooseClassMsg.Message.DeleteAsync();
                return;
            }
            Console.WriteLine($"Message was previously sent. The User that added the reaction has id: {SocketReaction.UserId}");
        }
    }
}
