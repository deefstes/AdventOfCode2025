using AdventOfCode2025.Day14;
using NUnit.Framework;

namespace AdventOfCode2025.Tests.Day14
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new(File.ReadAllText($"Day14\\sample.txt"));
            var rsp = solver.Part1();

            Assert.That(rsp, Is.EqualTo(""));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new(File.ReadAllText($"Day14\\sample.txt"));
            var rsp = solver.Part2();

            Assert.That(rsp, Is.EqualTo(""));
        }
    }
}