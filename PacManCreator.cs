using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    public class PacManCreator
    {
        const int height = 8;
        const int width = 8;

        int[,] gameArray;

        public PacManCreator()
        {
            gameArray = new int[8, 8];
        }

        public int[,] SetWalls(int Seed=0)
        {
            //can't be {0,0} or {7,7}
            var rand = new Random(Seed);
            var length = 16;
            for (var i = 0; i < length; i++)
            {
                var v = rand.Next(1, 7);
                var h = rand.Next(0, 7);

                if ((v == 0 && h == 0) || (v == 7 && h == 7))
                    continue;

                gameArray[v, h] = 1;
            }

            return gameArray;
        }
    }
}
