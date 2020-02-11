using System.Collections.Generic;
using NUnit.Framework;

namespace Day22
{
    [TestFixture]
    public class Tests
    {
        private static readonly string[] DealNewStack = {
"deal into new stack"
        };

        private static readonly string[] DealNewStackTwice = {
"deal into new stack",
"deal into new stack"
        };

        private static readonly string[] DealWithIncrementOne = {
"deal with increment 1",
        };

        private static readonly string[] DealWithIncrementThree = {
"deal with increment 3",
        };

        private static readonly string[] DealWithIncrementEleven = {
"deal with increment 11",
        };

        private static readonly string[] CutZero = {
"cut 0",
        };

        private static readonly string[] CutOne = {
"cut 1",
        };

        private static readonly string[] CutThree = {
"cut 3",
        };

        private static readonly string[] CutFive = {
"cut 5",
        };

        private static readonly string[] CutSix = {
"cut 6",
        };

        private static readonly string[] CutMinusOne = {
"cut -1",
        };

        private static readonly string[] CutMinusFour = {
"cut -4",
        };

        private static readonly string[] CutMinusFive = {
"cut -5",
        };

        private static readonly string[] ShuffleA = {
"deal with increment 7",
"deal into new stack",
"deal into new stack"
        };

        private static readonly string[] ShuffleB = {
"cut 6",
"deal with increment 7",
"deal into new stack"
        };

        private static readonly string[] ShuffleC = {
"deal with increment 7",
"deal with increment 9",
"cut -2",
        };

        private static readonly string[] ShuffleD = {
"deal into new stack",
"cut -2",
"deal with increment 7",
"cut 8",
"cut -4",
"deal with increment 7",
"cut 3",
"deal with increment 9",
"deal with increment 3",
"cut -1",
        };

        public static IEnumerable<TestCaseData> ShuffleTestCases => new[]
        {
            new TestCaseData(null, "0 1 2 3 4 5 6 7 8 9").SetName("StartingDeck"),

            new TestCaseData(DealNewStack, "9 8 7 6 5 4 3 2 1 0").SetName(nameof(DealNewStack)),
            new TestCaseData(DealNewStackTwice, "0 1 2 3 4 5 6 7 8 9").SetName(nameof(DealNewStackTwice)),

            new TestCaseData(DealWithIncrementOne, "0 1 2 3 4 5 6 7 8 9").SetName(nameof(DealWithIncrementOne)),
            new TestCaseData(DealWithIncrementThree, "0 7 4 1 8 5 2 9 6 3").SetName(nameof(DealWithIncrementThree)),
            new TestCaseData(DealWithIncrementEleven, "0 1 2 3 4 5 6 7 8 9").SetName(nameof(DealWithIncrementEleven)),

            new TestCaseData(CutZero, "0 1 2 3 4 5 6 7 8 9").SetName(nameof(CutZero)),
            new TestCaseData(CutOne, "1 2 3 4 5 6 7 8 9 0").SetName(nameof(CutOne)),
            new TestCaseData(CutThree, "3 4 5 6 7 8 9 0 1 2").SetName(nameof(CutThree)),
            new TestCaseData(CutFive, "5 6 7 8 9 0 1 2 3 4").SetName(nameof(CutFive)),
            new TestCaseData(CutSix, "6 7 8 9 0 1 2 3 4 5").SetName(nameof(CutSix)),
            new TestCaseData(CutMinusOne, "9 0 1 2 3 4 5 6 7 8").SetName(nameof(CutMinusOne)),
            new TestCaseData(CutMinusFour, "6 7 8 9 0 1 2 3 4 5").SetName(nameof(CutMinusFour)),
            new TestCaseData(CutMinusFive, "5 6 7 8 9 0 1 2 3 4").SetName(nameof(CutMinusFive)),

            new TestCaseData(ShuffleA, "0 3 6 9 2 5 8 1 4 7").SetName(nameof(ShuffleA)),
            new TestCaseData(ShuffleB, "3 0 7 4 1 8 5 2 9 6").SetName(nameof(ShuffleB)),
            new TestCaseData(ShuffleC, "6 3 0 7 4 1 8 5 2 9").SetName(nameof(ShuffleC)),
            new TestCaseData(ShuffleD, "9 2 5 8 1 4 7 0 3 6").SetName(nameof(ShuffleD))
        };

        [Test]
        [TestCaseSource("ShuffleTestCases")]
        public void ShuffleTests(string[] instructions, string expectedResult)
        {
            Program.CreateDeck(10);
            Program.RunInstructions(instructions);
            Assert.That(Program.DeckAsString(), Is.EqualTo(expectedResult));

            Program.CreateDeck(10);
            Program.ApplyInstructionsUsingEquation(instructions);
            Assert.That(Program.DeckAsString(), Is.EqualTo(expectedResult));
        }
    }
}
