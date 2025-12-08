using AdventOfCode2025.Utils;

namespace AdventOfCode2025.Day05
{
    public class Solver : ISolver
    {
        private readonly string _input;
        private readonly List<(long low, long high)> _ranges;
        private readonly List<long> _ingredients;

        public Solver(string input)
        {
            _input = input;
            _ranges = [];
            _ingredients = [];

            foreach (var line in _input.AsList())
            {
                if (line.Contains('-'))
                {
                    var low = line.Split('-')[0];
                    var high = line.Split('-')[1];
                    _ranges.Add((long.Parse(low), long.Parse(high)));
                }
                else
                {
                    if (string.IsNullOrEmpty(line))
                        continue;

                    _ingredients.Add(long.Parse(line));
                }
            }
        }

        public string Part1()
        {
            var freshIngredients = new List<long>();

            foreach (var ingredient in _ingredients)
            {
                foreach (var (low, high) in _ranges)
                {
                    if (low <= ingredient && high >= ingredient)
                    {
                        freshIngredients.Add(ingredient);
                        continue;
                    }
                }
            }

            var count = freshIngredients.Distinct().ToList().Count;

            return count.ToString();
        }

        public string Part2()
        {
            var uniqueRanges = CollapseRanges(_ranges);
            var count = 0L;

            foreach (var (low, high) in uniqueRanges)
            {
                count += high - low + 1;
            }

            return count.ToString();
        }

        private static List<(long low, long high)> CollapseRanges(List<(long low, long high)> ranges)
        {
            var sortedRanges = ranges
                .OrderBy(r => r.low)
                .ToList();

            var result = new List<(long low, long high)>();
            var current = sortedRanges[0];

            foreach (var range in sortedRanges.Skip(1))
            {
                if (range.low <= current.high)
                {
                    current.high = Math.Max(current.high, range.high);
                }
                else
                {
                    result.Add(current);
                    current = range;
                }
            }

            result.Add(current);

            return result;
        }

    }
}
