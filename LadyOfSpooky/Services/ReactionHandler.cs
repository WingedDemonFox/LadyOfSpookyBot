using Discord;
using Discord.WebSocket;
using LadyOfSpooky.Helpers;
using LadyOfSpooky.Reactions;
using LadyOfSpooky.Wrappers;
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
        private readonly ulong MessageId;
        private readonly SocketReaction SocketReaction;
        private readonly ISocketMessageChannel SocketMessageChannel;

        private readonly DiscordSocketClient _client;

        public ReactionHandler(IServiceProvider services)
        {
            _client = services.GetRequiredService<DiscordSocketClient>();
            _client.ReactionAdded += ReactionAddedAsync;
        }

        public ReactionHandler(ulong message, SocketReaction socketReaction, ISocketMessageChannel socketMessageChannel)
        {
            MessageId = message;
            SocketReaction = socketReaction;
            SocketMessageChannel = socketMessageChannel;
        }

        public void Initialize()
        {
            return;
        }

        private async Task ReactionAddedAsync(Cacheable<IUserMessage, ulong> argMessage, ISocketMessageChannel socketMessageChannel, SocketReaction argReaction)
        {
            HandleReaction(argMessage, socketMessageChannel, argReaction);
        }


        private async void HandleReaction(Cacheable<IUserMessage, ulong> argMessage, ISocketMessageChannel socketMessageChannel, SocketReaction argReaction)
        {
            if (argReaction.UserId == Global.BotId)
            {
                return;
            }

            var messagePair = Global.SentMessagesWithInvokingUser.Where(m => m.Key.Id == argMessage.Id).ToList().FirstOrDefault();
            var fightMsgs = Global.OnGoingFight.Where(m => m.Message.Id == argMessage.Id && m.Fight.ActivePlayer.DiscordUserId == argReaction.UserId).ToList();
            if (!messagePair.Equals(default(KeyValuePair<IMessage, ulong>)) && messagePair.Value == argReaction.UserId)
            {
                Global.SentMessagesWithInvokingUser.Remove(messagePair.Key);
                ReactionHandler reactionHandler = new(argMessage.Id, argReaction, socketMessageChannel);
                reactionHandler.AddClassToPlayer();
            }
            else if (!messagePair.Equals(default(KeyValuePair<IMessage, ulong>)))
            {
                //Global.SentMessagesWithInvokingUser.Remove(messagePair.Key);
                ReactionHandler reactionHandler = new(argMessage.Id, argReaction, socketMessageChannel);
                reactionHandler.AddPlayerToFightHandling();
            }
            else if (fightMsgs.Count() > 0)
            {
                ReactionHandler reactionHandler = new(argMessage.Id, argReaction, socketMessageChannel);
                var fightMsg = fightMsgs.First();
                fightMsg.Message = await GetFullMessage(fightMsg.Message.Id, socketMessageChannel);
                reactionHandler.FightHandling(fightMsg);
            }
        }

        private async void Run()
        {
            Console.WriteLine($"Message was previously sent. The User that added the reaction has id: {SocketReaction.UserId}");
        }

        private async Task<IMessage> GetFullMessage(ulong messageId, ISocketMessageChannel socketMessageChannel)
        {
            return await socketMessageChannel.GetMessageAsync(messageId);
        }

        private async void FightHandling(FightMsgWrapper fightMsg)
        {
            if (fightMsg != null)
            {
                FightReaction.FightLoop(fightMsg, SocketReaction.Emote, SocketMessageChannel);
            }
        }

        private async void AddPlayerToFightHandling()
        {
            var monsterSpawnMsg = Global.SpawnedMonsters.Where(m => m.Message.Id == MessageId).SingleOrDefault();
            if (monsterSpawnMsg != null)
            {
                FightReaction.Start1vsMonster(SocketReaction.UserId, monsterSpawnMsg, SocketMessageChannel);
                return;
            }
        }

        private async void AddClassToPlayer()
        {
            var chooseClassMsg = Global.ChooseClassWrapper.Where(m => m.Message.Id == MessageId).SingleOrDefault();
            if (chooseClassMsg != null)
            {
                ClassReactions.AddClassToPlayer(SocketReaction.UserId, SocketReaction.Emote);
                Global.ChooseClassWrapper.Remove(chooseClassMsg);
                await chooseClassMsg.Message.DeleteAsync();
                return;
            }
        }
    }
}
