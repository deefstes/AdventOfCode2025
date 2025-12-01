using AdventOfCode2025;
using AdventOfCode2025.Utils;
using System.Reflection;

string? userInput = null;
if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SOLVE_CURRENT_DAY")))
{
    Console.WriteLine("Which day do you want to solve? (1-25):");
    userInput = Console.ReadLine();
}

if (String.IsNullOrEmpty(userInput))
    userInput = DateTime.Now.Day.ToString();

if (!int.TryParse(userInput, out var day))
{
    Console.WriteLine("Incorrect day specified");
    return;
}

var solverType = Assembly
    .GetEntryAssembly()!
    .GetTypes()
    .Where(t => t.GetTypeInfo().IsClass && typeof(ISolver).IsAssignableFrom(t))
    .FirstOrDefault(t => t.FullName!.Split('.')[1] == $"Day{day:D2}");

if (solverType == null)
{
    Console.WriteLine($"No solver found for day {day}", day);
    return;
}

var fileInput = File.ReadAllText($"Day{day:D2}\\input.txt");

var solver = Activator.CreateInstance(solverType, new object[] { fileInput }) as ISolver;

if (solver == null)
{
    Console.WriteLine("Could not instantiate solver");
    return;
}

var p1 = Utils.MeasureExecutionTime(() => solver.Part1());
var p2 = Utils.MeasureExecutionTime(() => solver.Part2());

Console.WriteLine();
Console.WriteLine($"Solution for day {day}");
Console.WriteLine($"Part 1 ({p1.Item2.Format()}): {p1.Item1}");
Console.WriteLine($"Part 2 ({p2.Item2.Format()}): {p2.Item1}");
