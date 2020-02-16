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

Your puzzle answer was 17863741.

--- Part Two ---

After careful analysis, one thing is certain: you have no idea where all these bugs are coming from.

Then, you remember: Eris is an old Plutonian settlement! Clearly, the bugs are coming from recursively-folded space.

This 5x5 grid is only one level in an infinite number of recursion levels. The tile in the middle of the grid is actually another 5x5 grid, the grid in your scan is contained as the middle tile of a larger 5x5 grid, and so on. Two levels of grids look like this:

     |     |         |     |     
     |     |         |     |     
     |     |         |     |     
-----+-----+---------+-----+-----
     |     |         |     |     
     |     |         |     |     
     |     |         |     |     
-----+-----+---------+-----+-----
     |     | | | | | |     |     
     |     |-+-+-+-+-|     |     
     |     | | | | | |     |     
     |     |-+-+-+-+-|     |     
     |     | | |?| | |     |     
     |     |-+-+-+-+-|     |     
     |     | | | | | |     |     
     |     |-+-+-+-+-|     |     
     |     | | | | | |     |     
-----+-----+---------+-----+-----
     |     |         |     |     
     |     |         |     |     
     |     |         |     |     
-----+-----+---------+-----+-----
     |     |         |     |     
     |     |         |     |     
     |     |         |     |     
(To save space, some of the tiles are not drawn to scale.) Remember, this is only a small part of the infinitely recursive grid; there is a 5x5 grid that contains this diagram, and a 5x5 grid that contains that one, and so on. Also, the ? in the diagram contains another 5x5 grid, which itself contains another 5x5 grid, and so on.

The scan you took (your puzzle input) shows where the bugs are on a single level of this structure. The middle tile of your scan is empty to accommodate the recursive grids within it. Initially, no other levels contain bugs.

Tiles still count as adjacent if they are directly up, down, left, or right of a given tile. Some tiles have adjacent tiles at a recursion level above or below its own level. For example:

     |     |         |     |     
  1  |  2  |    3    |  4  |  5  
     |     |         |     |     
-----+-----+---------+-----+-----
     |     |         |     |     
  6  |  7  |    8    |  9  |  10 
     |     |         |     |     
-----+-----+---------+-----+-----
     |     |A|B|C|D|E|     |     
     |     |-+-+-+-+-|     |     
     |     |F|G|H|I|J|     |     
     |     |-+-+-+-+-|     |     
 11  | 12  |K|L|?|N|O|  14 |  15 
     |     |-+-+-+-+-|     |     
     |     |P|Q|R|S|T|     |     
     |     |-+-+-+-+-|     |     
     |     |U|V|W|X|Y|     |     
-----+-----+---------+-----+-----
     |     |         |     |     
 16  | 17  |    18   |  19 |  20 
     |     |         |     |     
-----+-----+---------+-----+-----
     |     |         |     |     
 21  | 22  |    23   |  24 |  25 
     |     |         |     |     
Tile 19 has four adjacent tiles: 14, 18, 20, and 24.
Tile G has four adjacent tiles: B, F, H, and L.
Tile D has four adjacent tiles: 8, C, E, and I.
Tile E has four adjacent tiles: 8, D, 14, and J.
Tile 14 has eight adjacent tiles: 9, E, J, O, T, Y, 15, and 19.
Tile N has eight adjacent tiles: I, O, S, and five tiles within the sub-grid marked ?.
The rules about bugs living and dying are the same as before.

For example, consider the same initial state as above:

....#
#..#.
#.?##
..#..
#....
The center tile is drawn as ? to indicate the next recursive grid. Call this level 0; the grid within this one is level 1, and the grid that contains this one is level -1. Then, after ten minutes, the grid at each level would look like this:

Depth -5:
..#..
.#.#.
..?.#
.#.#.
..#..

Depth -4:
...#.
...##
..?..
...##
...#.

Depth -3:
#.#..
.#...
..?..
.#...
#.#..

Depth -2:
.#.##
....#
..?.#
...##
.###.

Depth -1:
#..##
...##
..?..
...#.
.####

Depth 0:
.#...
.#.##
.#?..
.....
.....

Depth 1:
.##..
#..##
..?.#
##.##
#####

Depth 2:
###..
##.#.
#.?..
.#.##
#.#..

Depth 3:
..###
.....
#.?..
#....
#...#

Depth 4:
.###.
#..#.
#.?..
##.#.
.....

Depth 5:
####.
#..#.
#.?#.
####.
.....
In this example, after 10 minutes, a total of 99 bugs are present.

Starting with your scan, how many bugs are present after 200 minutes?

*/

namespace Day24
{
    class Program
    {
        static readonly int MAP_WIDTH = 5;
        static readonly int MAP_HEIGHT = 5;
        static int sMinDepth = 0;
        static int sMaxDepth = 0;
        static Dictionary<int, int[,]> sCellsPointers;
        static Dictionary<int, int[,]> sAdjacencyCountsPointers;
        static HashSet<int> sBioDiversities;

        private Program(string inputFile, bool part1)
        {
            var lines = ReadFile(inputFile);
            ParseInput(lines);

            if (part1)
            {
                var result1 = -666;
                for (var i = 0; i < 100000; ++i)
                {
                    Simulate(1, part1);
                    var bioDiversity = BioDiversityRating();
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
                var expected = 17863741;
                if (result1 != expected)
                {
                    throw new InvalidDataException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                Simulate(200, part1);
                var result2 = TotalCount();
                Console.WriteLine($"Day24: {sMinDepth} -> {sMaxDepth}");
                Console.WriteLine($"Day24: Result2 {result2}");
                var expected = 2029;
                if (result2 != expected)
                {
                    throw new InvalidDataException($"Part2 is broken {result2} != {expected}");
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
            var depth = 0;
            int[,] cells = new int[MAP_WIDTH, MAP_HEIGHT];
            sCellsPointers = new Dictionary<int, int[,]>(1000);
            sAdjacencyCountsPointers = new Dictionary<int, int[,]>(1000);
            sBioDiversities = new HashSet<int>(100000);

            sCellsPointers[depth] = cells;
            for (var y = 0; y < MAP_HEIGHT; ++y)
            {
                for (var x = 0; x < MAP_WIDTH; ++x)
                {
                    cells[x, y] = 0;
                }
            }
            if (lines.Length != MAP_HEIGHT)
            {
                throw new InvalidDataException($"Input data wrong height {lines.Length} != {MAP_HEIGHT}");
            }
            for (var y = 0; y < MAP_HEIGHT; ++y)
            {
                var line = lines[y];
                if (line.Length != MAP_HEIGHT)
                {
                    throw new InvalidDataException($"Input Line {y} data wrong width {line.Length} != {MAP_WIDTH}");
                }
                for (var x = 0; x < MAP_WIDTH; ++x)
                {
                    var cell = line[x];
                    if (cell == '.')
                    {
                        cells[x, y] = 0;
                    }
                    else if (cell == '#')
                    {
                        cells[x, y] = 1;
                    }
                    else
                    {
                        throw new InvalidDataException($"Invalid input {x},{y} '{cell}'");
                    }
                }
            }
        }

        public static void Simulate(int numIterations, bool nonRecursive)
        {
            for (var i = 0; i < numIterations; ++i)
            {
                sMinDepth -= 1;
                sMaxDepth += 1;
                if (nonRecursive)
                {
                    sMinDepth = 0;
                    sMaxDepth = 0;
                }
                for (var d = sMinDepth; d <= sMaxDepth; ++d)
                {
                    if (!sAdjacencyCountsPointers.ContainsKey(d))
                    {
                        sAdjacencyCountsPointers[d] = new int[MAP_WIDTH, MAP_HEIGHT];
                    }
                    if (!sCellsPointers.ContainsKey(d))
                    {
                        sCellsPointers[d] = new int[MAP_WIDTH, MAP_HEIGHT];
                        var cells = sCellsPointers[d];
                        for (var y = 0; y < MAP_HEIGHT; ++y)
                        {
                            for (var x = 0; x < MAP_WIDTH; ++x)
                            {
                                cells[x, y] = 0;
                            }
                        }
                    }
                    var adjacencyCounts = sAdjacencyCountsPointers[d];
                    for (var y = 0; y < MAP_HEIGHT; ++y)
                    {
                        for (var x = 0; x < MAP_WIDTH; ++x)
                        {
                            adjacencyCounts[x, y] = 0;
                        }
                    }

                    for (var y = 0; y < MAP_HEIGHT; ++y)
                    {
                        for (var x = 0; x < MAP_WIDTH; ++x)
                        {
                            var nonEmptyCount = 0;
                            nonEmptyCount += CellCount(d, x + 0, y - 1, nonRecursive, x, y);
                            nonEmptyCount += CellCount(d, x - 1, y + 0, nonRecursive, x, y);
                            nonEmptyCount += CellCount(d, x + 1, y + 0, nonRecursive, x, y);
                            nonEmptyCount += CellCount(d, x + 0, y + 1, nonRecursive, x, y);
                            adjacencyCounts[x, y] = nonEmptyCount;
                        }
                    }
                }

                for (var d = sMinDepth; d <= sMaxDepth; ++d)
                {
                    var cells = sCellsPointers[d];
                    var adjacencyCounts = sAdjacencyCountsPointers[d];
                    for (var y = 0; y < MAP_HEIGHT; ++y)
                    {
                        for (var x = 0; x < MAP_WIDTH; ++x)
                        {
                            var adjacentCount = adjacencyCounts[x, y];
                            var oldValue = cells[x, y];
                            var newValue = oldValue;
                            if ((oldValue == 0) && ((adjacentCount == 1) || (adjacentCount == 2)))
                            {
                                newValue = 1;
                            }
                            else if ((oldValue == 1) && (adjacentCount != 1))
                            {
                                newValue = 0;
                            }
                            if (!nonRecursive && (x == 2) && (y == 2))
                            {
                                newValue = 0;
                            }

                            cells[x, y] = newValue;
                        }
                    }
                }
            }
        }

        /*
        -----+-----+---------+-----+-----
             |     |         |     |     
          6  |  7  |    8    |  9  |  10 
             |     |         |     |     
        -----+-----+---------+-----+-----
             |     |A|B|C|D|E|     |     
             |     |-+-+-+-+-|     |     
             |     |F|G|H|I|J|     |     
             |     |-+-+-+-+-|     |     
         11  | 12  |K|L|?|N|O|  14 |  15 
             |     |-+-+-+-+-|     |     
             |     |P|Q|R|S|T|     |     
             |     |-+-+-+-+-|     |     
             |     |U|V|W|X|Y|     |     
        -----+-----+---------+-----+-----
             |     |         |     |     
         16  | 17  |    18   |  19 |  20 
             |     |         |     |     
        -----+-----+---------+-----+-----
        */
        private static int CellCount(int depth, int x, int y, bool nonRecursive, int fromX, int fromY)
        {
            if (((x < 0) || (x >= MAP_WIDTH)) && ((y < 0) || (y >= MAP_HEIGHT)))
            {
                throw new ArgumentOutOfRangeException($"Invalid x,y {x},{y}");
            }
            if (x == -1)
            {
                if (nonRecursive)
                {
                    return 0;
                }
                else
                {
                    // Go up one level and get cell : x = 1, y = 2
                    var d = depth - 1;
                    if (sCellsPointers.ContainsKey(d))
                    {
                        var cells = sCellsPointers[d];
                        return cells[1, 2];
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else if (x == MAP_WIDTH)
            {
                if (nonRecursive)
                {
                    return 0;
                }
                else
                {
                    // Go up one level and get cell : x = 3, y = 2
                    var d = depth - 1;
                    if (sCellsPointers.ContainsKey(d))
                    {
                        var cells = sCellsPointers[d];
                        return cells[3, 2];
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else if ((x < 0) || (x >= MAP_WIDTH))
            {
                throw new ArgumentOutOfRangeException($"Invalid x {x}");
            }
            else if (y == -1)
            {
                if (nonRecursive)
                {
                    return 0;
                }
                else
                {
                    // Go up one level and get cell : x = 2, y = 1
                    var d = depth - 1;
                    if (sCellsPointers.ContainsKey(d))
                    {
                        var cells = sCellsPointers[d];
                        return cells[2, 1];
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else if (y == MAP_WIDTH)
            {
                if (nonRecursive)
                {
                    return 0;
                }
                else
                {
                    // Go up one level and get cell : x = 2, y = 3
                    var d = depth - 1;
                    if (sCellsPointers.ContainsKey(d))
                    {
                        var cells = sCellsPointers[d];
                        return cells[2, 3];
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else if ((y < 0) || (y >= MAP_HEIGHT))
            {
                throw new ArgumentOutOfRangeException($"Invalid y {y}");
            }
            else if ((x == 2) && (y == 2))
            {
                if (nonRecursive)
                {
                    var cells = sCellsPointers[depth];
                    return cells[x, y];
                }
                else
                {
                    // Go down one level and total the row, column
                    var d = depth + 1;
                    if (sCellsPointers.ContainsKey(d))
                    {
                        var cells = sCellsPointers[d];
                        var count = 0;
                        // Get the level below left column
                        if ((fromX == 1) && (fromY == 2))
                        {
                            for (var childY = 0; childY < MAP_HEIGHT; ++childY)
                            {
                                count += cells[0, childY];
                            }
                        }
                        // Get the level below right column
                        else if ((fromX == 3) && (fromY == 2))
                        {
                            for (var childY = 0; childY < MAP_HEIGHT; ++childY)
                            {
                                count += cells[4, childY];
                            }
                        }
                        // Get the level below top row
                        else if ((fromX == 2) && (fromY == 1))
                        {
                            for (var childX = 0; childX < MAP_WIDTH; ++childX)
                            {
                                count += cells[childX, 0];
                            }
                        }
                        // Get the level below bottom row
                        else if ((fromX == 2) && (fromY == 3))
                        {
                            for (var childX = 0; childX < MAP_WIDTH; ++childX)
                            {
                                count += cells[childX, 4];
                            }
                        }
                        return count;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else
            {
                var cells = sCellsPointers[depth];
                return cells[x, y];
            }
        }

        public static int BioDiversityRating()
        {
            var bioDiversity = 0;
            var cellValue = 1;
            var depth = 0;
            var cells = sCellsPointers[depth];
            for (var y = 0; y < MAP_HEIGHT; ++y)
            {
                for (var x = 0; x < MAP_WIDTH; ++x)
                {
                    if (cells[x, y] == 1)
                    {
                        bioDiversity += cellValue;
                    }
                    cellValue *= 2;
                }
            }
            return bioDiversity;
        }

        public static int TotalCount()
        {
            var count = 0;
            for (var d = sMinDepth; d <= sMaxDepth; ++d)
            {
                var cells = sCellsPointers[d];
                for (var y = 0; y < MAP_HEIGHT; ++y)
                {
                    for (var x = 0; x < MAP_WIDTH; ++x)
                    {
                        count += cells[x, y];
                    }
                }
            }
            return count;
        }

        public static string[] CurrentState(int depth)
        {
            var cells = sCellsPointers[depth];
            var result = new string[MAP_HEIGHT];
            for (var y = 0; y < MAP_HEIGHT; ++y)
            {
                string line = "";
                for (var x = 0; x < MAP_WIDTH; ++x)
                {
                    var cell = cells[x, y];
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
