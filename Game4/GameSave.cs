using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Game4
{
    public class GameSave
    {
        public int Level { get; set; }

        public int Speed { get; set; }

        public int Asteroids { get; set; }

        public int Seed {get; set; }

        public GameSave(int level, int speed, int asteroids, int seed)
        { 
            Level = level;
            Speed = speed;
            Asteroids = asteroids;
            Seed = seed;
        }
    }
}
