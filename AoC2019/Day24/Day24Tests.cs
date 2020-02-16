using System.Collections.Generic;
using NUnit.Framework;

namespace Day24
{
    [TestFixture]
    public class Tests
    {
        private static readonly string[] StartingState = {
"....#",
"#..#.",
"#..##",
"..#..",
"#...."
};

        private static readonly string[] AfterOneMinute = {
"#..#.",
"####.",
"###.#",
"##.##",
".##.."
};

        private static readonly string[] AfterTwoMinutes = {
"#####",
"....#",
"....#",
"...#.",
"#.###"
};

        private static readonly string[] AfterThreeMinutes = {
"#....",
"####.",
"...##",
"#.##.",
".##.#"
};

        private static readonly string[] AfterFourMinutes = {
"####.",
"....#",
"##..#",
".....",
"##..."
};

        private static readonly string[] SimilarState = {
".....",
".....",
".....",
"#....",
".#..."
};

        public static IEnumerable<TestCaseData> SimulateTestCases => new[]
        {
            new TestCaseData(StartingState, 1, AfterOneMinute).SetName("1 Minute"),
            new TestCaseData(StartingState, 2, AfterTwoMinutes).SetName("2 Minutes"),
            new TestCaseData(StartingState, 3, AfterThreeMinutes).SetName("3 Minutes"),
            new TestCaseData(StartingState, 4, AfterFourMinutes).SetName("4_Minutes"),
        };

        public static IEnumerable<TestCaseData> BioDiversityTestCases => new[]
        {
            new TestCaseData(SimilarState, 2129920).SetName("StableState 2129920"),
        };

        [Test]
        [TestCaseSource("SimulateTestCases")]
        public void SimulateTests(string[] start, int numIterations, string[] expectedState)
        {
            Program.ParseInput(start);
            Program.Simulate(numIterations);
            Assert.That(Program.CurrentState, Is.EqualTo(expectedState));
        }

        [Test]
        [TestCaseSource("BioDiversityTestCases")]
        public void BioDiversityTests(string[] start, int expectedValue)
        {
            Program.ParseInput(start);
            Assert.That(Program.BioDiversityRating, Is.EqualTo(expectedValue));
        }
    }
}
