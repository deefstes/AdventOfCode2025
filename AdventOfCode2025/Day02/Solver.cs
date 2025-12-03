namespace AdventOfCode2025.Day02
{
    public class Solver(string input) : ISolver
    {
        private readonly string _input = input;

        public string Part1()
        {
            var ranges = _input.Split(',');
            var invalidIds = new List<long>();
            long total = 0;

            foreach (var range in ranges)
            {
                var low = long.Parse(range.Split('-')[0]);
                var high = long.Parse(range.Split('-')[1]);

                for (long id=low; id <= high; id++)
                {
                    if (IsRepeatedTwice(id))
                    {
                        invalidIds.Add(id);
                        total += id;
                    }
                }
            }

            return total.ToString();
        }

        public string Part2()
        {
            var ranges = _input.Split(',');
            var invalidIds = new List<long>();
            long total = 0;

            foreach (var range in ranges)
            {
                var low = long.Parse(range.Split('-')[0]);
                var high = long.Parse(range.Split('-')[1]);

                for (long id = low; id <= high; id++)
                {
                    if (IsRepeatedN(id))
                    {
                        invalidIds.Add(id);
                        total += id;
                    }
                }
            }

            return total.ToString();
        }

        private bool IsRepeatedTwice(long id)
        {
            string idString = id.ToString();

            if (idString.Length % 2 != 0)
                return false;

            int half = idString.Length / 2;

            string first = idString.Substring(0, half);
            string second = idString.Substring(half, half);

            return first == second;
        }

        private bool IsRepeatedN(long id)
        {
            string idString = id.ToString();

            if (idString.Length <= 1)
                return false;

            // Double the string and remove the first and last digits
            string doubled = (idString + idString)[1..^1];

            // If the original string appears anywhere inside this truncated double string, it must consist of repetitions
            return doubled.Contains(idString);
        }
    }
}
