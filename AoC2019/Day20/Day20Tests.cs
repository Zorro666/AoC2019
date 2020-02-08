﻿using System.Collections.Generic;
using NUnit.Framework;

namespace Day20
{
    [TestFixture]
    public class Tests
    {
        private static readonly string[] mapA = {
"         A           ",
"         A           ",
"  #######.#########  ",
"  #######.........#  ",
"  #######.#######.#  ",
"  #######.#######.#  ",
"  #######.#######.#  ",
"  #####  B    ###.#  ",
"BC...##  C    ###.#  ",
"  ##.##       ###.#  ",
"  ##...DE  F  ###.#  ",
"  #####    G  ###.#  ",
"  #########.#####.#  ",
"DE..#######...###.#  ",
"  #.#########.###.#  ",
"FG..#########.....#  ",
"  ###########.#####  ",
"             Z       ",
"             Z       "
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

        private static readonly string[] mapC = {
"             Z L X W       C                 ",
"             Z P Q B       K                 ",
"  ###########.#.#.#.#######.###############  ",
"  #...#.......#.#.......#.#.......#.#.#...#  ",
"  ###.#.#.#.#.#.#.#.###.#.#.#######.#.#.###  ",
"  #.#...#.#.#...#.#.#...#...#...#.#.......#  ",
"  #.###.#######.###.###.#.###.###.#.#######  ",
"  #...#.......#.#...#...#.............#...#  ",
"  #.#########.#######.#.#######.#######.###  ",
"  #...#.#    F       R I       Z    #.#.#.#  ",
"  #.###.#    D       E C       H    #.#.#.#  ",
"  #.#...#                           #...#.#  ",
"  #.###.#                           #.###.#  ",
"  #.#....OA                       WB..#.#..ZH",
"  #.###.#                           #.#.#.#  ",
"CJ......#                           #.....#  ",
"  #######                           #######  ",
"  #.#....CK                         #......IC",
"  #.###.#                           #.###.#  ",
"  #.....#                           #...#.#  ",
"  ###.###                           #.#.#.#  ",
"XF....#.#                         RF..#.#.#  ",
"  #####.#                           #######  ",
"  #......CJ                       NM..#...#  ",
"  ###.#.#                           #.###.#  ",
"RE....#.#                           #......RF",
"  ###.###        X   X       L      #.#.#.#  ",
"  #.....#        F   Q       P      #.#.#.#  ",
"  ###.###########.###.#######.#########.###  ",
"  #.....#...#.....#.......#...#.....#.#...#  ",
"  #####.#.###.#######.#######.###.###.#.#.#  ",
"  #.......#.......#.#.#.#.#...#...#...#.#.#  ",
"  #####.###.#####.#.#.#.#.###.###.#.###.###  ",
"  #.......#.....#.#...#...............#...#  ",
"  #############.#.#.###.###################  ",
"               A O F   N                     ",
"               A A D   M                     "
        };

        public static IEnumerable<TestCaseData> ShortestPathCases => new[]
        {
            new TestCaseData(mapA, false).SetName("ShorttestPath Part1 A 23").Returns(23),
            new TestCaseData(mapB, false).SetName("ShorttestPath Part1 B 58").Returns(58),
            new TestCaseData(mapA, true).SetName("ShorttestPath Part2 A 26").Returns(26),
            new TestCaseData(mapB, true).SetName("ShorttestPath Part2 B -1").Returns(-1),
            new TestCaseData(mapC, true).SetName("ShorttestPath Part2 C 396").Returns(396)
        };

        [Test]
        [TestCaseSource("ShortestPathCases")]
        public int ShortestPathPart1(string[] map, bool part2)
        {
            Program.ParseMap(map);
            Program.OutputMap(false);
            return Program.ShortestPath(part2);
        }
    }
}

