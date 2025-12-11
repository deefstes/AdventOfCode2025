using AdventOfCode2025.Day11;
using NUnit.Framework;

namespace AdventOfCode2025.Tests.Day11
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new(File.ReadAllText($"Day11\\sample.txt"));
            var rsp = solver.Part1();

            Assert.That(rsp, Is.EqualTo("5"));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new(File.ReadAllText($"Day11\\sample2.txt"));
            var rsp = solver.Part2();

            Assert.That(rsp, Is.EqualTo("2"));
        }
    }
}