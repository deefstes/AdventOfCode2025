using AdventOfCode2025.Day08;
using NUnit.Framework;

namespace AdventOfCode2025.Tests.Day08
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new(File.ReadAllText($"Day08\\sample.txt"));
            var rsp = solver.Part1();

            Assert.That(rsp, Is.EqualTo("40"));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new(File.ReadAllText($"Day08\\sample.txt"));
            var rsp = solver.Part2();

            Assert.That(rsp, Is.EqualTo("25272"));
        }
    }
}