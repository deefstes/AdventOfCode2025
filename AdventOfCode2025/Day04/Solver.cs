using AdventOfCode2025.Utils;

namespace AdventOfCode2025.Day04
{
    public class Solver(string input) : ISolver
    {
        private readonly string _input = input;

        public string Part1()
        {
            var wall = _input.AsGrid();

            var total = CountLooseRolls(wall);

            return total.ToString();
        }

        public string Part2()
        {
            var wall = _input.AsGrid();

            var count = 0;
            int removed;

            do
            {
                removed = CountLooseRolls(wall);
                count += removed;
            } while (removed  != 0);

            return count.ToString();
        }

        private static int CountLooseRolls(char[,] grid)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            bool[,] markedGrid = new bool[rows, cols];

            int count = 0;

            int[] dx = [-1, -1, -1,  0,  0,  1,  1,  1];
            int[] dy = [-1,  0,  1, -1,  1, -1,  0,  1];

            // Count accessible rolls and mark them for removal
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    if (grid[y, x] != '@')
                        continue;

                    int neighbourCount = 0;

                    for (int neighbour = 0; neighbour < 8; neighbour++)
                    {
                        int neighbourY = y + dx[neighbour];
                        int neighbourX = x + dy[neighbour];

                        if (neighbourY >= 0 && neighbourY < rows &&
                            neighbourX >= 0 && neighbourX < cols &&
                            grid[neighbourY, neighbourX] == '@')
                        {
                            neighbourCount++;
                        }
                    }

                    if (neighbourCount < 4)
                    {
                        markedGrid[y, x] = true;
                        count++;
                    }
                }
            }

            // Remove accessible rolls from the wall
            for (int y = 0; y < rows; y++)
                for (int x = 0; x < cols; x++)
                    if (markedGrid[y, x])
                        grid[y, x] = 'x';

            return count;
        }
    }
}
