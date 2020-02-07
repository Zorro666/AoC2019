using System.Collections.Generic;
using NUnit.Framework;

namespace Day20
{
    [TestFixture]
    public class Tests
    {
        private static readonly string[] mapA = {
"         A         ",
"         A         ",
"  #######.#########",
"  #######.........#",
"  #######.#######.#",
"  #######.#######.#",
"  #######.#######.#",
"  #####  B    ###.#",
"BC...##  C    ###.#",
"  ##.##       ###.#",
"  ##...DE  F  ###.#",
"  #####    G  ###.#",
"  #########.#####.#",
"DE..#######...###.#",
"  #.#########.###.#",
"FG..#########.....#",
"  ###########.#####",
"             Z     ",
"             Z     "
        };

        private static readonly string[] mapB = {
"                   A               ",
"                   A               ",
"  #################.#############  ",
"  #.#...#...................#.#.#  ",
"  #.#.#.###.###.###.#########.#.#  ",
"  #.#.#.......#...#.....#.#.#...#  ",
"  #.#########.###.#####.#.#.###.#  ",
"  #.............#.#.....#.......#  ",
"  ###.###########.###.#####.#.#.#  ",
"  #.....#        A   C    #.#.#.#  ",
"  #######        S   P    #####.#  ",
"  #.#...#                 #......VT",
"  #.#.#.#                 #.#####  ",
"  #...#.#               YN....#.#  ",
"  #.###.#                 #####.#  ",
"DI....#.#                 #.....#  ",
"  #####.#                 #.###.#  ",
"ZZ......#               QG....#..AS",
"  ###.###                 #######  ",
"JO..#.#.#                 #.....#  ",
"  #.#.#.#                 ###.#.#  ",
"  #...#..DI             BU....#..LF",
"  #####.#                 #.#####  ",
"YN......#               VT..#....QG",
"  #.###.#                 #.###.#  ",
"  #.#...#                 #.....#  ",
"  ###.###    J L     J    #.#.###  ",
"  #.....#    O F     P    #.#...#  ",
"  #.###.#####.#.#####.#####.###.#  ",
"  #...#.#.#...#.....#.....#.#...#  ",
"  #.#####.###.###.#.#.#########.#  ",
"  #...#.#.....#...#.#.#.#.....#.#  ",
"  #.###.#####.###.###.#.#.#######  ",
"  #.#.........#...#.............#  ",
"  #########.###.###.#############  ",
"           B   J   C               ",
"           U   P   P               "
        };

        public static IEnumerable<TestCaseData> ShortestPathCases => new[]
        {
            new TestCaseData(mapA, 23).SetName("ShorttestPath A 23"),
            new TestCaseData(mapB, 58).SetName("ShorttestPath B 58"),
        };

        [Test]
        [TestCaseSource("ShortestPathCases")]
        public void ShortestPath(string[] map, int expectedNumSteps)
        {
            Program.ParseMap(map, false);
            Program.OutputMap(false);
            Assert.That(Program.ShortestPath(), Is.EqualTo(expectedNumSteps));
        }
    }
}

