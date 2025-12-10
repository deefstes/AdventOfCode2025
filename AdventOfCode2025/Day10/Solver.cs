using AdventOfCode2025.Utils;

namespace AdventOfCode2025.Day10
{
    public partial class Solver : ISolver
    {
        private readonly string _input;
        private readonly List<Machine> _machines = [];

        public Solver(string input)
        {
            _input = input;

            foreach (var machine in _input.AsList())
            {
                _machines.Add(new Machine(machine));
            }
        }

        public string Part1()
        {
            var count = 0;

            foreach( var machine in _machines)
            {
                var buttonPresses = machine.TurnOffAllLights();
                count += buttonPresses.Count;
            }

            return count.ToString();
        }

        public string Part2()
        {
            var count = 0;

            foreach (var machine in _machines)
            {
                var buttonPresses = machine.ReachJoltageLevels();
                count += buttonPresses.Sum(b => b.Value);
            }

            return count.ToString();
        }
    }
}
