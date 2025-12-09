using AdventOfCode2025.Day01;
using NUnit.Framework;

namespace AdventOfCode2025.Tests.Day01
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new(File.ReadAllText($"Day01\\sample.txt"));
            var rsp = solver.Part1();

            Assert.That(rsp, Is.EqualTo("3"));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new(File.ReadAllText($"Day01\\sample.txt"));
            var rsp = solver.Part2();

            Assert.That(rsp, Is.EqualTo("6"));
        }
    }
}