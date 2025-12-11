using AdventOfCode2025.Utils;

namespace AdventOfCode2025.Day11
{
    public class Solver : ISolver
    {
        private readonly string _input;
        private readonly Dictionary<string, List<string>> _machines = [];

        public Solver(string input)
        {
            _input = input;

            foreach (var line in _input.AsList())
            {
                var links = line.Split(' ');
                _machines.Add(links.First()[..3], [.. links[1..]]);
            }
        }

        public string Part1()
        {
            var paths = CountPaths("you", "out", []);
            return paths.ToString();
        }

        public string Part2()
        {
            var paths = CountPaths("svr", "out", ["fft", "dac"]);
            return paths.ToString();
        }

        public long CountPaths(string startNode, string endNode, List<string> requiredNodes)
        {
            var requiredSet = new HashSet<string>(requiredNodes);

            var cache = new Dictionary<(string Node, string RequiredNodes), long>();
            var visited = new HashSet<string>();

            long Dfs(string currentNode, HashSet<string> requiredNodes)
            {
                // Make local copy of hash set
                var remaining = new HashSet<string>(requiredNodes);

                // If end node reached, count only if all required nodes were also visited
                if (currentNode == endNode)
                    return remaining.Count == 0 ? 1 : 0;

                var key = (currentNode, string.Join(",", remaining.Order()));

                if (cache.TryGetValue(key, out long cached))
                    return cached;

                // Discard cyclic paths
                if (visited.Contains(currentNode))
                    return 0;

                visited.Add(currentNode);

                long total = 0;

                var removed = remaining.Remove(currentNode);

                foreach (var next in _machines[currentNode])
                {
                    total += Dfs(next, remaining);
                }

                visited.Remove(currentNode);

                cache[key] = total;

                return total;
            }

            return Dfs(startNode, requiredSet);
        }
    }
}
