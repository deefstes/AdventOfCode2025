namespace AdventOfCode2025.Day03
{
    public class Solver(string input) : ISolver
    {
        private readonly string _input = input;

        public string Part1()
        {
            var banks = _input.Split("\r\n");
            var total = 0L;

            foreach (var bank in banks)
            {
                var jolt = LargestNumber(bank, 2);
                total += jolt;
            }

            return total.ToString();
        }

        public string Part2()
        {
            var banks = _input.Split("\r\n");
            var total = 0L;

            foreach (var bank in banks)
            {
                var jolt = LargestNumber(bank, 12);
                total += jolt;
            }

            return total.ToString();
        }

        private static long LargestNumber(string bank, int n)
        {
            if (n <= 0 || n > bank.Length)
                throw new ArgumentException("n must be between 1 and the length of the string.");

            int start = 0;
            long result = 0;

            for (int picked = 0; picked < n; picked++)
            {
                int maxDigit = -1;
                int maxPos = start;

                // The furthest we can search while still being able to pick enough digits later
                int end = bank.Length - (n - picked);

                for (int i = start; i <= end; i++)
                {
                    int d = bank[i] - '0';
                    if (d > maxDigit)
                    {
                        maxDigit = d;
                        maxPos = i;

                        // 9 is the highest digit, we can stop early if we found a 9
                        if (d == 9) break;
                    }
                }

                result = result * 10 + maxDigit;

                start = maxPos + 1;
            }

            return result;
        }
    }
}
