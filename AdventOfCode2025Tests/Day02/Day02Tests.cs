using AdventOfCode2025.Day02;
using NUnit.Framework;

namespace AdventOfCode2025.Tests.Day02
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new(File.ReadAllText($"Day02\\sample.txt"));
            var rsp = solver.Part1();

            Assert.That(rsp, Is.EqualTo("1227775554"));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new(File.ReadAllText($"Day02\\sample.txt"));
            var rsp = solver.Part2();

            Assert.That(rsp, Is.EqualTo("4174379265"));
        }
    }
}