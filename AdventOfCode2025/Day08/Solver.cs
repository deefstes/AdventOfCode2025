using AdventOfCode2025.Utils;

namespace AdventOfCode2025.Day08
{
    public class Solver : ISolver
    {
        private readonly string _input;
        private readonly List<(int x, int y, int z)> coords = [];
        private readonly List<(long dist, int a, int b)> pairs = [];

        public Solver(string input)
        {
            _input = input;

            foreach (var coord in _input.AsList())
            {
                var vals = coord.Split(',');
                coords.Add((int.Parse(vals[0]), int.Parse(vals[1]), int.Parse(vals[2])));
            }

            // Calculate squared distances between all coordinate pairs
            for (int i = 0; i < coords.Count; i++)
                for (int j = i + 1; j < coords.Count; j++)
                    pairs.Add((DistanceSquared(coords[i], coords[j]), i, j));

            pairs.Sort((p, q) => p.dist.CompareTo(q.dist));
        }

        public string Part1()
        {
            // The sample inut is calculated against only 10 iterations
            var iterations = _input.Length > 250 ? 1000 : 10;

            List<int> groups = GetGroupSizes(iterations);

            var result = groups.Take(3).Aggregate(1, (acc, val) => acc * val);

            return result.ToString();
        }

        public string Part2()
        {
            var (coord1, coord2) = ConnectUntilSingleGroup();

            return (coord1.x * coord2.x).ToString();
        }

        private static int FindRoot(int[] parent, int x)
        {
            if (parent[x] != x)
            {
                // Short circuits the path to the root parent to make future lookups faster
                parent[x] = FindRoot(parent, parent[x]);
            }

            return parent[x];
        }

        private static bool JoinCircuits(int[] parent, int[] size, int a, int b)
        {
            int rootA = FindRoot(parent, a);
            int rootB = FindRoot(parent, b);

            if (rootA == rootB)
                return false; // Already in same circuit

            // Join smaller circuit to larger circuit
            if (size[rootA] < size[rootB])
                (rootA, rootB) = (rootB, rootA);

            parent[rootB] = rootA;
            size[rootA] += size[rootB];

            return true;
        }

        private static long DistanceSquared((int x, int y, int z) a, (int x, int y, int z) b)
        {
            long dx = a.x - b.x;
            long dy = a.y - b.y;
            long dz = a.z - b.z;
            return dx * dx + dy * dy + dz * dz;
        }

        private List<int> GetGroupSizes(
            int connections)
        {
            int[] parent = new int[coords.Count];
            int[] size = new int[coords.Count];

            for (int i = 0; i < coords.Count; i++)
            {
                parent[i] = i;
                size[i] = 1;
            }

            for (int i = 0; i < connections && i < pairs.Count; i++)
                JoinCircuits(parent, size, pairs[i].a, pairs[i].b);

            var groupSizes = new Dictionary<int, int>();

            for (int i = 0; i < coords.Count; i++)
            {
                int root = FindRoot(parent, i);
                if (!groupSizes.ContainsKey(root))
                    groupSizes[root] = 0;
                groupSizes[root]++;
            }

            return groupSizes.Values
                             .OrderByDescending(x => x)
                             .ToList();
        }

        private ((int x, int y, int z) A, (int x, int y, int z) B) ConnectUntilSingleGroup()
        {
            int[] parent = new int[coords.Count];
            int[] size = new int[coords.Count];

            for (int i = 0; i < coords.Count; i++)
            {
                parent[i] = i;
                size[i] = 1;
            }

            int components = coords.Count;

            foreach (var p in pairs)
            {
                if (JoinCircuits(parent, size, p.a, p.b))
                {
                    components--;

                    if (components == 1)
                        return (coords[p.a], coords[p.b]);
                }
            }

            throw new InvalidOperationException("Unable to connect all circuits");
        }
    }
}
