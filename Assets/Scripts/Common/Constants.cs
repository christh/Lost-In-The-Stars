using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IR
{
    public static class Constants
    {

        public const float DropForceRandomnessMin = 3f;
        public const float DropForceRandomnessMax = 6f;

        public const float MaximumDegreesOfInaccuracy = 90f;

        public static class Layers
        {
            public const int IgnoreEnemyBullets = 6;
            public const int PlayerBullets = 10;
            public const int EnemyBullets = 11;
            public const int ActivePickups = 12;
            public const int Player = 13;
            public const int Environment = 15;
            public const int DeadThings = 19;
        }
    }
}
