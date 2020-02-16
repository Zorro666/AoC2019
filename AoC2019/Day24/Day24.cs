using System;
using System.Collections.Generic;
using System.IO;

/*

--- Day 24: Planet of Discord ---

You land on Eris, your last stop before reaching Santa.As soon as you do, your sensors start picking up strange life forms moving around: Eris is infested with bugs! With an over 24-hour roundtrip for messages between you and Earth, you'll have to deal with this problem on your own.

Eris isn't a very large place; a scan of the entire area fits into a 5x5 grid (your puzzle input). The scan shows bugs (#) and empty spaces (.).

Each minute, The bugs live and die based on the number of bugs in the four adjacent tiles:

A bug dies(becoming an empty space) unless there is exactly one bug adjacent to it.
An empty space becomes infested with a bug if exactly one or two bugs are adjacent to it.
Otherwise, a bug or empty space remains the same. (Tiles on the edges of the grid have fewer than four adjacent tiles; the missing tiles count as empty space.) This process happens in every location simultaneously; that is, within the same minute, the number of adjacent bugs is counted for every tile first, and then the tiles are updated.

Here are the first few minutes of an example scenario:


Initial state:
....#
#..#.
#..##
..#..
#....

After 1 minute:
#..#.
####.
###.#
##.##
.##..

After 2 minutes:
#####
....#
....#
...#.
#.###

After 3 minutes:
#....
####.
...##
#.##.
.##.#

After 4 minutes:
####.
....#
##..#
.....
##...
To understand the nature of the bugs, watch for the first time a layout of bugs and empty spaces matches any previous layout. In the example above, the first layout to appear twice is:

.....
.....
.....
#....
.#...
To calculate the biodiversity rating for this layout, consider each tile left-to-right in the top row, then left-to-right in the second row, and so on.Each of these tiles is worth biodiversity points equal to increasing powers of two: 1, 2, 4, 8, 16, 32, and so on.Add up the biodiversity points for tiles with bugs; in this example, the 16th tile (32768 points) and 22nd tile (2097152 points) have bugs, a total biodiversity rating of 2129920.

What is the biodiversity rating for the first layout that appears twice?

*/
namespace Day24
{
    class Program
    {
        static readonly int MAP_WIDTH = 5;
        static readonly int MAP_HEIGHT = 5;
        static int[,] sCells;
        static int[,] sCellsNext;
        static HashSet<int> sBioDiversities;

        private Program(string inputFile, bool part1)
        {
            var lines = ReadFile(inputFile);
            ParseInput(lines);

            if (part1)
            {
                int result1 = -666;
                for (int i = 0; i < 100000; ++i)
                {
                    Simulate(1);
                    int bioDiversity = BioDiversityRating();
                    if (sBioDiversities.Contains(bioDiversity))
                    {
                        result1 = bioDiversity;
                        break;
                    }
                    else
                    {
                        sBioDiversities.Add(bioDiversity);
                    }
                    if (result1 != -666)
                    {
                        break;
                    }
                }
                Console.WriteLine($"Day24: Result1 {result1}");
                int expected = 17863741;
                if (result1 != expected)
                {
                    throw new InvalidDataException($"Part1 is broken {result1} != {expected}");
                }
            }
        }

        private string[] ReadFile(string inputFile)
        {
            var lines = File.ReadAllLines(inputFile);
            return lines;
        }

        public static void ParseInput(string[] lines)
        {
            sCells = new int[MAP_WIDTH, MAP_HEIGHT];
            sCellsNext = new int[MAP_WIDTH, MAP_HEIGHT];
            sBioDiversities = new HashSet<int>(100000);
            for (int y = 0; y < MAP_HEIGHT; ++y)
            {
                for (int x = 0; x < MAP_WIDTH; ++x)
                {
                    sCells[x, y] = 0;
                    sCellsNext[x, y] = 0;
                }
            }
            if (lines.Length != MAP_HEIGHT)
            {
                throw new InvalidDataException($"Input data wrong height {lines.Length} != {MAP_HEIGHT}");
            }
            for (int y = 0; y < MAP_HEIGHT; ++y)
            {
                var line = lines[y];
                if (line.Length != MAP_HEIGHT)
                {
                    throw new InvalidDataException($"Input Line {y} data wrong width {line.Length} != {MAP_WIDTH}");
                }
                for (int x = 0; x < MAP_WIDTH; ++x)
                {
                    var cell = line[x];
                    if (cell == '.')
                    {
                        sCells[x, y] = 0;
                    }
                    else if (cell == '#')
                    {
                        sCells[x, y] = 1;
                    }
                    else
                    {
                        throw new InvalidDataException($"Invalid input {x},{y} '{cell}'");
                    }
                }
            }
        }

        public static void Simulate(int numIterations)
        {
            int[,] numAdjacent = new int[MAP_WIDTH, MAP_HEIGHT];
            for (int i = 0; i < numIterations; ++i)
            {
                for (int y = 0; y < MAP_HEIGHT; ++y)
                {
                    for (int x = 0; x < MAP_WIDTH; ++x)
                    {
                        numAdjacent[x, y] = 0;
                    }
                }

                for (int y = 0; y < MAP_HEIGHT; ++y)
                {
                    for (int x = 0; x < MAP_WIDTH; ++x)
                    {
                        int nonEmptyCount = 0;
                        nonEmptyCount += GetCell(x + 0, y - 1);
                        nonEmptyCount += GetCell(x - 1, y + 0);
                        nonEmptyCount += GetCell(x + 1, y + 0);
                        nonEmptyCount += GetCell(x + 0, y + 1);
                        numAdjacent[x, y] = nonEmptyCount;
                    }
                }

                for (int y = 0; y < MAP_HEIGHT; ++y)
                {
                    for (int x = 0; x < MAP_WIDTH; ++x)
                    {
                        var adjacentCount = numAdjacent[x, y];
                        int oldValue = sCells[x, y];
                        int newValue = oldValue;
                        if ((oldValue == 0) && ((adjacentCount == 1) || (adjacentCount == 2)))
                        {
                            newValue = 1;
                        }
                        else if ((oldValue == 1) && (adjacentCount != 1))
                        {
                            newValue = 0;
                        }
                        sCellsNext[x, y] = newValue;
                    }
                }
                for (int y = 0; y < MAP_HEIGHT; ++y)
                {
                    for (int x = 0; x < MAP_WIDTH; ++x)
                    {
                        sCells[x, y] = sCellsNext[x, y];
                    }
                }
            }
        }

        private static int GetCell(int x, int y)
        {
            if ((x < 0) || (x >= MAP_WIDTH))
            {
                return 0;
            }
            if ((y < 0) || (y >= MAP_HEIGHT))
            {
                return 0;
            }
            return sCells[x, y];
        }

        public static int BioDiversityRating()
        {
            int bioDiversity = 0;
            int cellValue = 1;
            for (int y = 0; y < MAP_HEIGHT; ++y)
            {
                for (int x = 0; x < MAP_WIDTH; ++x)
                {
                    if (sCells[x, y] == 1)
                    {
                        bioDiversity += cellValue;
                    }
                    cellValue *= 2;
                }
            }
            return bioDiversity;
        }

        public static string[] CurrentState()
        {
            var result = new string[MAP_HEIGHT];
            for (int y = 0; y < MAP_HEIGHT; ++y)
            {
                string line = "";
                for (int x = 0; x < MAP_WIDTH; ++x)
                {
                    var cell = sCells[x, y];
                    if (cell == 0)
                    {
                        line += '.';
                    }
                    else if (cell == 1)
                    {
                        line += '#';
                    }
                    else
                    {
                        throw new InvalidDataException($"Invalid cell {x},{y} '{cell}'");
                    }
                }
                result[y] = line;
            }
            return result;
        }

        public static void Run()
        {
            Console.WriteLine("Day24 : Start");
            _ = new Program("Day24/input.txt", true);
            _ = new Program("Day24/input.txt", false);
            Console.WriteLine("Day24 : End");
        }
    }
}
