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

        private static readonly string[] RecursiveDepthMinus5 = {
"..#..",
".#.#.",
"....#",
".#.#.",
"..#.."
};

        private static readonly string[] RecursiveDepthMinus4 = {
"...#.",
"...##",
".....",
"...##",
"...#."
};

        private static readonly string[] RecursiveDepthMinus3 = {
"#.#..",
".#...",
".....",
".#...",
"#.#.."
};

        private static readonly string[] RecursiveDepthMinus2 = {
".#.##",
"....#",
"....#",
"...##",
".###."
};

        private static readonly string[] RecursiveDepthMinus1 = {
"#..##",
"...##",
".....",
"...#.",
".####"
};

        private static readonly string[] RecursiveDepth0 = {
".#...",
".#.##",
".#...",
".....",
"....."
};

        private static readonly string[] RecursiveDepthPlus1 = {
".##..",
"#..##",
"....#",
"##.##",
"#####"
};

        private static readonly string[] RecursiveDepthPlus2 = {
"###..",
"##.#.",
"#....",
".#.##",
"#.#.."
};

        private static readonly string[] RecursiveDepthPlus3 = {
"..###",
".....",
"#....",
"#....",
"#...#"
};

        private static readonly string[] RecursiveDepthPlus4 = {
".###.",
"#..#.",
"#....",
"##.#.",
"....."
};

        private static readonly string[] RecursiveDepthPlus5 = {
"####.",
"#..#.",
"#..#.",
"####.",
"....."
};
        /*
        In this example, after 10 minutes, a total of 99 bugs are present.
        */
        public static IEnumerable<TestCaseData> SimulateNonRecursiveTestCases => new[]
        {
            new TestCaseData(StartingState, 1, AfterOneMinute).SetName("Simulate Non Recursive - 1 Minute"),
            new TestCaseData(StartingState, 2, AfterTwoMinutes).SetName("Simulate Non Recursive - 2 Minutes"),
            new TestCaseData(StartingState, 3, AfterThreeMinutes).SetName("Simulate Non Recursive - 3 Minutes"),
            new TestCaseData(StartingState, 4, AfterFourMinutes).SetName("Simulate Non Recursive - 4 Minutes"),
        };

        public static IEnumerable<TestCaseData> BioDiversityTestCases => new[]
        {
            new TestCaseData(SimilarState, 2129920).SetName("StableState 2129920"),
        };

        public static IEnumerable<TestCaseData> SimulateRecursiveTestCases => new[]
        {
            new TestCaseData(StartingState, -5, RecursiveDepthMinus5).SetName("Simulate Recursive - Depth -5"),
            new TestCaseData(StartingState, -4, RecursiveDepthMinus4).SetName("Simulate Recursive - Depth -4"),
            new TestCaseData(StartingState, -3, RecursiveDepthMinus3).SetName("Simulate Recursive - Depth -3"),
            new TestCaseData(StartingState, -2, RecursiveDepthMinus2).SetName("Simulate Recursive - Depth -2"),
            new TestCaseData(StartingState, -1, RecursiveDepthMinus1).SetName("Simulate Recursive - Depth -1"),
            new TestCaseData(StartingState, 0, RecursiveDepth0).SetName("Simulate Recursive - Depth 0"),
            new TestCaseData(StartingState, 1, RecursiveDepthPlus1).SetName("Simulate Recursive - Depth +1"),
            new TestCaseData(StartingState, 2, RecursiveDepthPlus2).SetName("Simulate Recursive - Depth +2"),
            new TestCaseData(StartingState, 3, RecursiveDepthPlus3).SetName("Simulate Recursive - Depth +3"),
            new TestCaseData(StartingState, 4, RecursiveDepthPlus4).SetName("Simulate Recursive - Depth +4"),
            new TestCaseData(StartingState, 5, RecursiveDepthPlus5).SetName("Simulate Recursive - Depth +5"),
        };

        [Test]
        [TestCaseSource("SimulateNonRecursiveTestCases")]
        public void SimulateNonRecursiveTests(string[] start, int numIterations, string[] expectedState)
        {
            Program.ParseInput(start);
            Program.Simulate(numIterations, true);
            Assert.That(Program.CurrentState(0), Is.EqualTo(expectedState));
        }

        [Test]
        [TestCaseSource("SimulateRecursiveTestCases")]
        public void SimulateRecursiveTests(string[] start, int depth, string[] expectedState)
        {
            Program.ParseInput(start);
            Program.Simulate(10, false);
            Assert.That(Program.CurrentState(depth), Is.EqualTo(expectedState));
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
