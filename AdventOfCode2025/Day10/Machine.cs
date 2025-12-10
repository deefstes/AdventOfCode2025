using System.Text.RegularExpressions;
using Microsoft.Z3;

namespace AdventOfCode2025.Day10
{
    public partial class Solver
    {
        public partial class Machine
        {
            private readonly List<bool> _lights = [];
            private readonly List<List<int>> _buttons = [];
            private readonly List<int> _joltage = [];

            [GeneratedRegex(@"\[(.*?)\]")] // [...]
            private static partial Regex LightsRegex();

            [GeneratedRegex(@"\((.*?)\)")] // (...) x n
            private static partial Regex ButtonsRegex();

            [GeneratedRegex(@"\{(.*?)\}")] // {...}
            private static partial Regex JoltageRegex();

            public Machine(string input)
            {
                // Parse lights
                var lightsMatch = LightsRegex().Match(input);
                if (!lightsMatch.Success)
                    throw new ArgumentException("Cannot parse lights");

                _lights = [.. lightsMatch.Groups[1].Value.ToCharArray().Select(c => c == '#')];

                // Parse buttons
                var buttonMatches = ButtonsRegex().Matches(input);
                if (buttonMatches.Count == 0)
                    throw new ArgumentException("Cannot parse buttons");

                _buttons = [.. buttonMatches.Select(m => m.Groups[1].Value.Split(',').Select(int.Parse).ToList())];

                // Parse joltages
                var joltageMatch = JoltageRegex().Match(input);
                if (!joltageMatch.Success)
                    throw new ArgumentException("Cannot parse joltage");

                _joltage = [.. joltageMatch.Groups[1].Value.Split(',').Select(int.Parse)];
            }

            public List<int> TurnOffAllLights()
            {
                int n = _lights.Count;
                int initialState = 0;

                // Convert initial lights to bitmask
                for (int i = 0; i < n; i++)
                    if (_lights[i])
                        initialState |= (1 << i);

                int targetState = 0; // all off

                if (initialState == targetState)
                    return [];

                int[] buttonMasks = new int[_buttons.Count];
                for (int i = 0; i < _buttons.Count; i++)
                {
                    var mask = 0;

                    foreach (var lightIndex in _buttons[i])
                        mask |= (1 << lightIndex);

                    buttonMasks[i] = mask;
                }

                // BFS
                var queue = new Queue<(int State, List<int> Path)>();
                var visited = new HashSet<int>();

                queue.Enqueue((initialState, new List<int>()));
                visited.Add(initialState);

                while (queue.Count > 0)
                {
                    var (state, path) = queue.Dequeue();

                    for (int button = 0; button < buttonMasks.Length; button++)
                    {
                        int nextState = state ^ buttonMasks[button];

                        if (visited.Contains(nextState))
                            continue;

                        var nextPath = new List<int>(path) { button };

                        if (nextState == targetState)
                            return nextPath;

                        visited.Add(nextState);
                        queue.Enqueue((nextState, nextPath));
                    }
                }

                throw new InvalidDataException("No solution found");
            }

            public Dictionary<string, int> ReachJoltageLevels()
            {
                var z3Context = new Context();
                var optimisation = z3Context.MkOptimize();
                var buttonConstants = new IntExpr[_buttons.Count];

                for (int i = 0; i < _buttons.Count; i++)
                {
                    buttonConstants[i] = z3Context.MkIntConst($"B{i}");
                    optimisation.Add(z3Context.MkGe(buttonConstants[i], z3Context.MkInt(0))); // Each button must be pressed 0 or more times
                }

                for (int i = 0; i < _joltage.Count; i++)
                {
                    var expressions = new List<ArithExpr>();

                    for (int j = 0; j < _buttons.Count; j++)
                    {
                        if (_buttons[j].Contains(i))
                            expressions.Add(buttonConstants[j]);
                    }

                    if (expressions.Count == 0)
                        throw new InvalidDataException($"No solution found: No button wired to joltage {i}");

                    var polynomial = z3Context.MkAdd([.. expressions]);

                    optimisation.Add(z3Context.MkEq(polynomial, z3Context.MkInt(_joltage[i])));
                }

                optimisation.MkMinimize(z3Context.MkAdd(buttonConstants));

                if (optimisation.Check() != Status.SATISFIABLE)
                    throw new InvalidDataException("No solution found");

                var result = new Dictionary<string, int>();

                foreach (var button in buttonConstants)
                {
                    var buttonPresses = optimisation.Model.Evaluate(button);

                    result.Add(button.ToString(), ((IntNum)buttonPresses).Int);
                }

                return result;
            }
        }
    }
}
