using AdventOfCode2025.Utils;

namespace AdventOfCode2025.Day07
{
    public class Solver(string input) : ISolver
    {
        private readonly string _input = input;

        public string Part1()
        {
            var manifold = _input.AsGrid();
            var (x, y) = manifold.FindCells('S').First();
            var tachyons = new HashSet<int> { x };
            var splits = 0;

            for (int row = 0; row < manifold.GetLength(1); row++)
            {
                for (int col = 0; col < manifold.GetLength(0); col++)
                {
                    if (manifold[col, row] == '^' && tachyons.Contains(col))
                    {
                        tachyons.Remove(col);
                        tachyons.Add(col - 1);
                        tachyons.Add(col + 1);
                        splits++;
                    }

                }
            }

            return splits.ToString();
        }

        public string Part2()
        {
            var manifold = _input.AsGrid();
            var (x, y) = manifold.FindCells('S').First();

            var timelines = CountPaths(manifold, x);

            return timelines.ToString();
        }

        private static long CountPaths(char[,] grid, int startX)
        {
            int cols = grid.GetLength(0);
            int rows = grid.GetLength(1);

            var cache = new Dictionary<(int x, int y), long>();

            long Dfs(int x, int y)
            {
                long result;

                // Exit condition
                if (y == rows)
                    return 1;

                if (cache.TryGetValue((x, y), out var cached))
                    return cached;

                if (grid[x, y] == '^')
                    result = Dfs(x - 1, y + 1) + Dfs(x + 1, y + 1);
                else
                    result = Dfs(x, y + 1);

                cache[(x, y)] = result;
                return result;
            }

            return Dfs(startX, 0);
        }
    }
}