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

var components = userInput.Split('.');

if (!int.TryParse(components[0], out var day))
{
    Console.WriteLine($"Incorrect day ({components[0]}) specified");
    return;
}

var part = 0;
if (components.Length > 1 && !int.TryParse(components[1], out part))
{
    Console.WriteLine($"Incorrect part ({components[1]}) specified");
    return;
}

if (part<0 || part>2)
{
    Console.WriteLine($"Incorrect part ({part}) specified");
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

var (solver, spinUpTime) = Utils.MeasureExecutionTime(() => Activator.CreateInstance(solverType, [fileInput]) as ISolver);

if (solver == null)
{
    Console.WriteLine("Could not instantiate solver");
    return;
}

(string, TimeSpan) p1 = default;
(string, TimeSpan) p2 = default;

if (part==0 || part==1)
    p1 = Utils.MeasureExecutionTime(() => solver.Part1());

if (part==0 || part==2)
    p2 = Utils.MeasureExecutionTime(() => solver.Part2());

Console.WriteLine();
Console.WriteLine($"Solution for day {day}");
Console.WriteLine($"Constructor ({spinUpTime.Format()})");
if (part == 0 || part == 1)
    Console.WriteLine($"Part 1 ({p1.Item2.Format()}): {p1.Item1}");
if (part == 0 || part == 2)
    Console.WriteLine($"Part 2 ({p2.Item2.Format()}): {p2.Item1}");
Console.WriteLine($"Overall ({(spinUpTime + p1.Item2 + p2.Item2).Format()})");
