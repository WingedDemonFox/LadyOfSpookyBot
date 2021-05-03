using Discord;
using Discord.Commands;
using LadyOfSpooky.Helpers;
using LadyOfSpooky.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LadyOfSpooky.Modules
{
    [Group("monster")]
    public class MonsterModule : ModuleBase<SocketCommandContext>
    {
        [Command("all")]
        [Summary("List all monster")]
        public async Task AllMonstersAsync(string failThing = null)
        {
            var monsters = DataHelper.GetMonstersFromFile();

            EmbedBuilder builder = new();

            builder.WithTitle($"Monsters with lvl");
            builder.WithDescription($"Showing all monsters {failThing}");

            var monstersList = monsters.OrderBy(m => m.Level).ToList();

            var monsterLvls = string.Empty;
            var monsterNames = string.Empty;
            foreach (var monster in monstersList)
            {
                monsterLvls += $"{monster.Level}\n";
                monsterNames += $"{monster.MonsterName}\n";
            }

            builder.AddField("Lvl", monsterLvls, true);
            builder.AddField("Monsters", monsterNames, true);

            builder.WithCurrentTimestamp();

            builder.WithColor(Color.Red);
            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }

        [RequireOwner]
        [Command("spawn", RunMode = RunMode.Async)]
        [Summary("Spawn a monster")]
        public async Task SpawnMonsterAsync(string failThing = null)
        {
            var monsters = DataHelper.GetMonstersFromFile();

            var random = new Random();
            int index = random.Next(monsters.Count);
            var monster = monsters[index];

            var embedBuilder = EmbedHelper.GetMonsterEmbed(monster, "is wreaking havoc");
            var msg = await Context.Channel.SendMessageAsync("", false, embedBuilder.Build());

            await msg.AddReactionsAsync(new Emoji[] { EmojiHelper.Emojis["Join"], EmojiHelper.Emojis["Flee"] });
            Global.SpawnedMonsters.Add(new MonsterSpawnWrapper(msg, monster));
            Global.SentMessagesWithInvokingUser.TryAdd(msg, Context.Message.Author.Id);
        }



        [RequireOwner]
        [Command("add", RunMode = RunMode.Async)]
        [Summary("add a new monster")]
        public async Task AddMonsterInitAsync(string monsterName, int defense, int attack, int nrAttack, int hitProb, int level)
        {
            var monster = new Monster
            {
                MonsterName = monsterName,
                Defense = defense,
                Attack = attack,
                NumberOfAttacks = nrAttack,
                HitProb = hitProb,
                Level = level
            };

            var monsters = DataHelper.GetMonstersFromFile();
            monsters.Add(monster);
            DataHelper.WriteMonstersToFile(monsters);

            var embedBuilder = EmbedHelper.GetMonsterEmbed(monster);
            await Context.Channel.SendMessageAsync("", false, embedBuilder.Build());

        }

        [Command]
        [Summary("detail info for a monster")]
        public async Task DetailAsync([Remainder][Summary("Monster to show detail")] string monsterName)
        {
            var monsters = DataHelper.GetMonstersFromFile();

            var monster = monsters.Where(m => m.MonsterName.ToLower().Equals(monsterName.ToLower())).FirstOrDefault();

            if (monster == null)
            {
                await ReplyAsync("No monster found");
                return;
            }

            var embedBuilder = EmbedHelper.GetMonsterEmbed(monster);
            await Context.Channel.SendMessageAsync("", false, embedBuilder.Build());
        }
    }
}
