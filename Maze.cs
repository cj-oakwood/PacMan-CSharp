using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    public class Maze
    {
        const int N = 8;
        public bool Solve(int [,] maze)
        {
            int[,] sol = new int[N, N];
            return this.SolveHelper(maze, 0, 0, sol);
        }

        public bool SolveHelper(int [,] maze, int x, int y, int [,] sol)
        {
            if(x == N-1 && y == N-1)
            {
                sol[x, y] = 1;
                return true;
            }

            if (isSafe(maze, x, y))
            {
                var result = SolveHelper(maze, x+1, y, sol) ? true : (SolveHelper(maze, x + 1, y, sol) ? true : false);
                sol[x, y] = result ? 1 : 0;

                return result;
            }

            return false;
        }

        public bool isSafe(int[,] maze, int x, int y)
        {
            if( x>= 0 && x < N && y >= 0 && y < N && maze[x,y] == 1)
                return true;

            return false;
        }
    }
}
