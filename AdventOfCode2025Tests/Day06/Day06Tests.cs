using AdventOfCode2025.Day06;
using NUnit.Framework;

namespace AdventOfCode2025.Tests.Day06
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new(File.ReadAllText($"Day06\\sample.txt"));
            var rsp = solver.Part1();

            Assert.That(rsp, Is.EqualTo("4277556"));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new(File.ReadAllText($"Day06\\sample.txt"));
            var rsp = solver.Part2();

            Assert.That(rsp, Is.EqualTo("3263827"));
        }
    }
}