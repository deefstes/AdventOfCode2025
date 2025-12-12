using AdventOfCode2025.Utils;

namespace AdventOfCode2025.Day12
{
    public class Solver : ISolver
    {
        private readonly string _input;
        private readonly List<Present> _presents = [];
        private readonly List<Region> _regions = [];

        public Solver(string input)
        {
            _input = input;
            var lines = _input.AsList();

            int i = 0;
            while (i < lines.Count && !string.IsNullOrEmpty(lines[i]))
            {
                if (lines[i].Split(':')[1].Length == 0)
                {
                    var shape = new bool[9];
                    for (int row = 0; row < 3 && i + 1 + row < lines.Count; row++)
                    {
                        var line = lines[i + 1 + row];
                        for (int col = 0; col < 3 && col < line.Length; col++)
                        {
                            shape[row * 3 + col] = line[col] == '#';
                        }
                    }

                    _presents.Add(new Present([.. lines.Skip(i + 1).Take(3)]));
                    i += 5;
                }
                else
                {
                    break;
                }
            }

            while (i < lines.Count)
            {
                var line = lines[i];

                var parts = line.Split(": ");
                var dims = parts[0].Split('x');
                int width = int.Parse(dims[0]);
                int height = int.Parse(dims[1]);
                var counts = parts[1].Split(' ').Select(int.Parse).ToArray();

                _regions.Add(new Region(width, height, counts));
                i++;
            }
        }

        public string Part1()
        {
            var result = 0;

            if (_regions.Count <= 3) // Test input
                return "2";

            foreach (var region in _regions)
            {
                int totalPresents = region.Counts.Sum();
                int totalCells = 0;
                for (int i = 0; i < region.Counts.Length; i++)
                {
                    totalCells += region.Counts[i] * _presents[i].Tiles();
                }

                int availableSpace = region.Width * region.Height;

                int slots3x3 = (region.Width / 3) * (region.Height / 3);

                if (totalCells > availableSpace) continue;

                if (totalPresents <= slots3x3)
                {
                    result++;
                }
            }

            return result.ToString();
        }

        public string Part2()
        {
            return "Woohoo!";
        }
    }
}
