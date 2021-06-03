using Discord;
using Discord.Commands;
using LadyOfSpooky.Helpers;
using LadyOfSpooky.Models;
using LadyOfSpooky.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LadyOfSpooky.Modules
{
    [Group("monster")]
    public class MonsterModule : ModuleBase<SocketCommandContext>
    {
        [Command("all")]
        [Summary("List all monster")]
        public async Task AllMonstersAsync(bool showAll = false)
        {
            var monsters = DataHelper.GetMonstersFromFile();

            EmbedBuilder builder = new();

            builder.WithTitle($"Monsters with lvl");
            builder.WithDescription($"Showing all monsters");

            List<Monster> monstersList;
            if (showAll && Context.User.Id == Global.OwnerId)
            {
                monstersList = monsters.OrderBy(m => m.Level).ToList();
            }
            else
            {
                monstersList = monsters.Where(m => m.HasSpawnedBefore == true).OrderBy(m => m.Level).ToList();
            }

            if (monstersList.Count != 0)
            {
                var monsterLvls = string.Empty;
                var monsterNames = string.Empty;
                foreach (var monster in monstersList)
                {
                    monsterLvls += $"{monster.Level}\n";
                    monsterNames += $"{monster.Name}\n";
                }

                builder.AddField("Lvl", monsterLvls, true);
                builder.AddField("Monsters", monsterNames, true);
            }
            else
            {
                builder.AddField("Info", "No monsters seen", false);
            }



            builder.WithColor(Color.Red);
            var msg = await Context.Channel.SendMessageAsync("", false, builder.Build());
            EmbedHelper.DeleteMessage(msg, TimeSpan.FromSeconds(20));
        }

        [RequireOwner]
        [Command("spawn", RunMode = RunMode.Async)]
        [Summary("Spawn a monster")]
        public async Task SpawnMonsterAsync(string failThing = null)
        {
            var allMonsters = DataHelper.GetMonstersFromFile();

            var allPlayers = DataHelper.GetPlayersFromFile();
            var lvl = allPlayers.Select(p => p.Level).ToList().Max();

            var monsters = allMonsters.Where(m => m.Level <= lvl).ToList();

            var random = new Random();
            int index = random.Next(monsters.Count);
            var monster = monsters[index];

            var embedBuilder = EmbedHelper.GetMonsterEmbed(monster, "is wreaking havoc");
            var msg = await Context.Channel.SendMessageAsync("", false, embedBuilder.Build());


            if (monster.HasSpawnedBefore == false)
            {
                allMonsters.Remove(monster);
                monster.HasSpawnedBefore = true;
                allMonsters.Add(monster);
                DataHelper.WriteMonstersToFile(monsters);
            }

            await msg.AddReactionsAsync(new Emoji[] { EmojiHelper.Emojis["Join"] });
            Global.SpawnedMonsters.Add(new MonsterSpawnWrapper(msg, monster));
            Global.SentMessagesWithInvokingUser.TryAdd(msg, Global.BotId);
        }



        [RequireOwner]
        [Command("add", RunMode = RunMode.Async)]
        [Summary("add a new monster")]
        public async Task AddMonsterInitAsync(string monsterName, int defense, int attack, int nrAttack, int hitProb, int level)
        {
            var monster = new Monster
            {
                Name = monsterName,
                Defense = defense,
                Attack = attack,
                NumberOfAttacks = nrAttack,
                HitProbability = hitProb,
                Level = level
            };

            var monsters = DataHelper.GetMonstersFromFile();
            monsters.Add(monster);
            DataHelper.WriteMonstersToFile(monsters);

            var embedBuilder = EmbedHelper.GetMonsterEmbed(monster);
            var msg = await Context.Channel.SendMessageAsync("", false, embedBuilder.Build());
            EmbedHelper.DeleteMessage(msg, TimeSpan.FromSeconds(20));

        }

        [Command]
        [Summary("detail info for a monster")]
        public async Task DetailAsync([Remainder][Summary("Monster to show detail")] string monsterName)
        {
            var monsters = DataHelper.GetMonstersFromFile();

            var monster = monsters.Where(m => m.Name.ToLower().Equals(monsterName.ToLower())).FirstOrDefault();

            if (monster == null)
            {
                await ReplyAsync("No monster found");
                return;
            }

            var embedBuilder = EmbedHelper.GetMonsterEmbed(monster);
            var msg = await Context.Channel.SendMessageAsync("", false, embedBuilder.Build());
            EmbedHelper.DeleteMessage(msg, TimeSpan.FromSeconds(20));
        }
    }
}
