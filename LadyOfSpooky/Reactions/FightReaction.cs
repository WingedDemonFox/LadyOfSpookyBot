using Discord;
using Discord.WebSocket;
using LadyOfSpooky.Helpers;
using LadyOfSpooky.Models;
using LadyOfSpooky.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LadyOfSpooky.Models.Enums;

namespace LadyOfSpooky.Reactions
{
    public class FightReaction
    {
        public static async void Start1vsMonster(ulong userId, MonsterSpawnWrapper spawnWrapper, ISocketMessageChannel socketMessageChannel)
        {
            var players = DataHelper.GetPlayersFromFile();
            var player = players.Where(p => p.DiscordUserId == userId).SingleOrDefault();

            if (player == null)
            {
                return;
            }
            spawnWrapper.PlayersInFight.Add(player);

            RemoveSpawnMessage(spawnWrapper);

            _ = Task.Delay(5);
            StartFight(spawnWrapper, socketMessageChannel);
        }

        private static async void StartFight(MonsterSpawnWrapper spawnWrapper, ISocketMessageChannel socketMessageChannel)
        {
            Fight fight = new(spawnWrapper.MonsterSpawned, spawnWrapper.PlayersInFight);

            var builder = EmbedHelper.GetFightEmbed(fight);

            var msg = await socketMessageChannel.SendMessageAsync("", false, builder.Build());
            await msg.AddReactionsAsync(new Emoji[] { EmojiHelper.Emojis["Attack"] });

            Global.OnGoingFight.Add(new FightMsgWrapper(msg, fight));
        }

        private static async void RemoveSpawnMessage(MonsterSpawnWrapper spawnMsg)
        {
            Global.SpawnedMonsters.Remove(spawnMsg);
            await spawnMsg.Message.DeleteAsync();
        }

        public static async void FightLoop(FightMsgWrapper fightMsgWrapper, IEmote reaction, ISocketMessageChannel socketMessageChannel)
        {
            var fight = fightMsgWrapper.Fight;
            var prevHealthActive = fight.ActivePlayer.Health;
            var prevHealthMonster = fight.Monster.Health;

            if (reaction.Name == EmojiHelper.Emojis["Attack"].Name)
            {
                fight.TurnAttack();
                fight.Turn += 1;
            }

            if (fight.FightStatus == FightStatus.Win)
            {
                StopFight(fight.Monster.Exp, FightStatus.Win, fight.ActivePlayer, fightMsgWrapper);
            }
            else if (fight.FightStatus == FightStatus.Lose)
            {
                StopFight(-fight.Monster.Exp, FightStatus.Lose, fight.ActivePlayer, fightMsgWrapper);
            }
            else if (prevHealthActive != fight.ActivePlayer.Health || prevHealthMonster != fight.Monster.Health)
            {
                UpdateEmbed(fightMsgWrapper, FightStatus.Ongoing);
                //remove reaction from the user
            }
            fightMsgWrapper.Fight = fight;
        }

        private static async void StopFight(int exp, FightStatus fightStatus, Player activePlayer, FightMsgWrapper fightMsgWrapper)
        {
            activePlayer.UpdateExpAndLevel(exp);
            var players = DataHelper.GetPlayersFromFile();

            var player = players.Where(m => m.DiscordUserId == activePlayer.DiscordUserId).SingleOrDefault();
            players.Remove(player);
            players.Add(activePlayer);

            DataHelper.WritePlayersToFile(players);

            UpdateEmbed(fightMsgWrapper, fightStatus);
            EmbedHelper.DeleteMessage(fightMsgWrapper.Message, TimeSpan.FromSeconds(30));
            Global.OnGoingFight.Remove(fightMsgWrapper);
        }

        private static async void UpdateEmbed(FightMsgWrapper fightMsgWrapper, FightStatus fightStatus)
        {
            var fightMessage = fightMsgWrapper.Message as IUserMessage;
            var fightEmbed = new EmbedBuilder();
            switch (fightStatus)
            {
                case FightStatus.Ongoing:
                    fightEmbed = EmbedHelper.GetFightEmbed(fightMsgWrapper.Fight);
                    break;
                case FightStatus.Win:
                    fightEmbed = EmbedHelper.GetFightWonEmbed(fightMsgWrapper.Fight);
                    break;
                case FightStatus.Lose:
                    fightEmbed = EmbedHelper.GetFightGameOverEmbed(fightMsgWrapper.Fight);
                    break;
            }

            await fightMessage.ModifyAsync(x =>
            {
                x.Embed = fightEmbed.Build();
            });

        }
    }
}
