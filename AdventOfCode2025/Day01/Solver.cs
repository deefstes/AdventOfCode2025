using System.Net;

namespace AdventOfCode2025.Day01
{
    public class Solver(string input) : ISolver
    {
        private readonly string _input = input;

        public string Part1()
        {
            var instructions = _input.Split("\r\n");
            var dial = 50;
            var countZeroes = 0;

            foreach (var instruction in instructions)
            {
                var dir = instruction[0] == 'R' ? 1 : -1;
                var amount = int.Parse(instruction[1..]);

                dial = dial + dir * amount;
                while (dial >= 100)
                    dial = dial - 100;
                while (dial < 0)
                    dial = dial + 100;
                if (dial == 0)
                    countZeroes++;
            }
            
            return countZeroes.ToString();
        }

        public string Part2()
        {
            var instructions = _input.Split("\r\n");
            var dial = 50;
            var countZeroes = 0;
            var prevDir = 0;

            foreach (var instruction in instructions)
            {
                var dir = instruction[0] == 'R' ? 1 : -1;
                var amount = int.Parse(instruction[1..]);
                var start = dial;

                while (amount > 0)
                {
                    dial = (dial + dir) % 100;
                    amount--;
                    if (dial == 0)
                        countZeroes++;
                }
            }

            return countZeroes.ToString();
        }
    }
}
