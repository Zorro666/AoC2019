using System.Collections.Generic;
using NUnit.Framework;

namespace Day18
{
    [TestFixture]
    public class Tests
    {
        private static readonly string[] mapA = {
"#########",
"#b.A.@.a#",
"#########"
        };

        private static readonly string[] mapB = {
"########################",
"#f.D.E.e.C.b.A.@.a.B.c.#",
"######################.#",
"#d.....................#",
"########################"
        };

        private static readonly string[] mapC = {
"########################",
"#...............b.C.D.f#",
"#.######################",
"#.....@.a.B.c.d.A.e.F.g#",
"########################"
        };

        private static readonly string[] mapD = {
"#################",
"#i.G..c...e..H.p#",
"########.########",
"#j.A..b...f..D.o#",
"########@########",
"#k.E..a...g..B.n#",
"########.########",
"#l.F..d...h..C.m#",
"#################"
        };

        private static readonly string[] mapE = {
"########################",
"#@..............ac.GI.b#",
"###d#e#f################",
"###A#B#C################",
"###g#h#i################",
"########################"
        };

        public static IEnumerable<TestCaseData> ShortestPathCases => new[]
        {
            new TestCaseData(mapA, 8).SetName("ShorttestPath A 8"),
            new TestCaseData(mapB, 86).SetName("ShorttestPath B 86"),
            new TestCaseData(mapC, 132).SetName("ShorttestPath C 132"),
            new TestCaseData(mapD, 136).SetName("ShorttestPath D 136"),
            new TestCaseData(mapE, 81).SetName("ShorttestPath E 81")
        };

        [Test]
        [TestCaseSource("ShortestPathCases")]
        public void ShortestPath(string[] map, int expectedStepCount)
        {
            Program.ParseMap(map);
            Program.OutputMap();
            Assert.That(Program.ShortestPath(), Is.EqualTo(expectedStepCount));
        }

        public static IEnumerable<TestCaseData> NavigateToKeyCases => new[]
        {
            new TestCaseData(mapA, 5, 1, 0, 0).SetName("Navigate @ -> a [] 2").Returns(2),
            new TestCaseData(mapA, 5, 1, 1, 0).SetName("Navigate @ -> b [] -1").Returns(-1),
            new TestCaseData(mapA, 5, 1, 1, 1).SetName("Navigate @ -> b [a] 4").Returns(4),
        };

        [Test]
        [TestCaseSource("NavigateToKeyCases")]
        public int Navigate(string[] map, int fromX, int fromY, int toKeyIndex, int collectedKeys)
        {
            Program.ParseMap(map);
            Program.OutputMap();
            return Program.NavigateToKey(fromX, fromY, toKeyIndex, collectedKeys);
        }
    }
}
