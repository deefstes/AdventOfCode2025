using AdventOfCode2025.Day03;
using NUnit.Framework;

namespace AdventOfCode2025.Tests.Day03
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new(File.ReadAllText($"Day03\\sample.txt"));
            var rsp = solver.Part1();

            Assert.That(rsp, Is.EqualTo("357"));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new(File.ReadAllText($"Day03\\sample.txt"));
            var rsp = solver.Part2();

            Assert.That(rsp, Is.EqualTo("3121910778619"));
        }
    }
}