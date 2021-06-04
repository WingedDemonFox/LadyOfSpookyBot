using LadyOfSpooky.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LadyOfSpooky.Helpers
{
    public static class DataHelper
    {
        public static List<Monster> GetMonstersFromFile()
        {
            // check if file exists
            if (File.Exists(Global.MonstersFile))
            {
                var monstersFile = File.ReadAllText(Global.MonstersFile);

                var monsters = new List<Monster>();
                if (monstersFile != String.Empty)
                {
                    monsters = JsonSerializer.Deserialize<List<Monster>>(monstersFile);
                }
                return monsters;
            }
            else
            {
                throw new FileNotFoundException("Monsters file does not exist. Please check your appsettings.json and try again.");
            }
        }

        public static async void WriteMonstersToFile(List<Monster> monsters)
        {
            using FileStream createStream = File.Create(Global.MonstersFile);
            await JsonSerializer.SerializeAsync(createStream, monsters);
        }

        public static List<Player> GetPlayersFromFile()
        {
            // check if file exists
            if (File.Exists(Global.PlayersFile))
            {
                var playersFile = File.ReadAllText(Global.PlayersFile);
                var players = new List<Player>();
                if (playersFile != String.Empty)
                {
                    players = JsonSerializer.Deserialize<List<Player>>(playersFile);
                }
                return players;
            }
            else
            {
                throw new FileNotFoundException("Players file does not exist. Please check your appsettings.json and try again.");
            }
        }

        public static async void WritePlayersToFile(List<Player> players)
        {
            using FileStream createStream = File.Create(Global.PlayersFile);
            await JsonSerializer.SerializeAsync(createStream, players);
        }
    }
}
