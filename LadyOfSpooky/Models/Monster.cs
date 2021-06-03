using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadyOfSpooky.Models
{
    public class Monster : BaseFighter
    {
        public int NumberOfAttacks { get; set; }

        public string PicUrl { get; set; } = string.Empty;

        public bool HasSpawnedBefore { get; set; } = false;
    }
}
