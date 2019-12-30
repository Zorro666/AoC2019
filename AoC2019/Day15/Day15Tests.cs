using System.Collections.Generic;
using NUnit.Framework;

namespace Day15
{
    [TestFixture]
    public class Tests
    {
        private static readonly string[] mapA = {
            " ##   ",
            "#..## ",
            "#.#..#",
            "#.O.# ",
            " ###  "
        };
        private static readonly string[] mapB = {
            " ## # ",
            "#O.#.#",
            "#.#..#",
            "#...# ",
            " ###  "
        };
        private static IEnumerable<TestCaseData> FillMapWithOxygenCases => new[]
        {
            new TestCaseData(mapA, 4).SetName("FillMapWithOxygen A 4"),
            new TestCaseData(mapB, 7).SetName("FillMapWithOxygen B 7"),
        };

        [Test]
        [TestCaseSource("FillMapWithOxygenCases")]
        public void FillMapWithOxygen(string[] mapStart, int expectedResult)
        {
            Program.ParseInput(mapStart);
            var actualResult = Program.FillWithOxygen();
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
    }
}
