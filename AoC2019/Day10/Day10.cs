using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/*
--- Day 10: Monitoring Station ---

You fly into the asteroid belt and reach the Ceres monitoring station.The Elves here have an emergency: they're having trouble tracking all of the asteroids and can't be sure they're safe.

The Elves would like to build a new monitoring station in a nearby area of space; they hand you a map of all of the asteroids in that region(your puzzle input).

The map indicates whether each position is empty(.) or contains an asteroid(#). The asteroids are much smaller than they appear on the map, and every asteroid is exactly in the center of its marked position. The asteroids can be described with X,Y coordinates where X is the distance from the left edge and Y is the distance from the top edge (so the top-left corner is 0,0 and the position immediately to its right is 1,0).

Your job is to figure out which asteroid would be the best place to build a new monitoring station.A monitoring station can detect any asteroid to which it has direct line of sight - that is, there cannot be another asteroid exactly between them. This line of sight can be at any angle, not just lines aligned to the grid or diagonally.The best location is the asteroid that can detect the largest number of other asteroids.

For example, consider the following map:

.#..#
.....
#####
....#
...##
The best location for a new monitoring station on this map is the highlighted asteroid at 3,4 because it can detect 8 asteroids, more than any other location. (The only asteroid it cannot detect is the one at 1,0; its view of this asteroid is blocked by the asteroid at 2,2.) All other asteroids are worse locations; they can detect 7 or fewer other asteroids. Here is the number of other asteroids a monitoring station on each asteroid could detect:

.7..7
.....
67775
....7
...87
Here is an asteroid (#) and some examples of the ways its line of sight might be blocked. If there were another asteroid at the location of a capital letter, the locations marked with the corresponding lowercase letter would be blocked and could not be detected:

#.........
...A......
...B..a...
.EDCG....a
..F.c.b...
.....c....
..efd.c.gb
.......c..
....f...c.
...e..d..c
Here are some larger examples:

Best is 5,8 with 33 other asteroids detected:

......#.#.
#..#.#....
..#######.
.#.#.###..
.#..#.....
..#....#.#
#..#....#.
.##.#..###
##...#..#.
.#....####
Best is 1,2 with 35 other asteroids detected:

#.#...#.#.
.###....#.
.#....#...
##.#.#.#.#
....#.#.#.
.##..###.#
..#...##..
..##....##
......#...
.####.###.
Best is 6,3 with 41 other asteroids detected:

.#..#..###
####.###.#
....###.#.
..###.##.#
##.##.#.#.
....###..#
..#.#..#.#
#..#.#.###
.##...##.#
.....#.#..
Best is 11,13 with 210 other asteroids detected:

.#..##.###...#######
##.############..##.
.#.######.########.#
.###.#######.####.#.
#####.##.#.##.###.##
..#####..#.#########
####################
#.####....###.#.#.##
##.#################
#####.##.###..####..
..######..##.#######
####.##.####...##..#
.#####..#.######.###
##...#.##########...
#.##########.#######
.####.#.###.###.#.##
....##.##.###..#####
.#.#.###########.###
#.#.#.#####.####.###
###.##.####.##.#..##
Find the best location for a new monitoring station.How many other asteroids can be detected from that location?

#.#.###.#.#....#..##.#....
.....#..#..#..#.#..#.....#
.##.##.##.##.##..#...#...#
#.#...#.#####...###.#.#.#.
.#####.###.#.#.####.#####.
#.#.#.##.#.##...####.#.##.
##....###..#.#..#..#..###.
..##....#.#...##.#.#...###
#.....#.#######..##.##.#..
#.###.#..###.#.#..##.....#
##.#.#.##.#......#####..##
#..##.#.##..###.##.###..##
#..#.###...#.#...#..#.##.#
.#..#.#....###.#.#..##.#.#
#.##.#####..###...#.###.##
#...##..#..##.##.#.##..###
#.#.###.###.....####.##..#
######....#.##....###.#..#
..##.#.####.....###..##.#.
#..#..#...#.####..######..
#####.##...#.#....#....#.#
.#####.##.#.#####..##.#...
#..##..##.#.##.##.####..##
.##..####..#..####.#######
#.#..#.##.#.######....##..
.#.##.##.####......#.##.##

--- Part Two ---

Once you give them the coordinates, the Elves quickly deploy an Instant Monitoring Station to the location and discover the worst: there are simply too many asteroids.

The only solution is complete vaporization by giant laser.

Fortunately, in addition to an asteroid scanner, the new monitoring station also comes equipped with a giant rotating laser perfect for vaporizing asteroids. The laser starts by pointing up and always rotates clockwise, vaporizing any asteroid it hits.

If multiple asteroids are exactly in line with the station, the laser only has enough power to vaporize one of them before continuing its rotation. In other words, the same asteroids that can be detected can be vaporized, but if vaporizing one asteroid makes another one detectable, the newly-detected asteroid won't be vaporized until the laser has returned to the same position by rotating a full 360 degrees.

For example, consider the following map, where the asteroid with the new monitoring station (and laser) is marked X:

.#....#####...#..
##...##.#####..##
##...#...#.#####.
..#.....X...###..
..#.#.....#....##
The first nine asteroids to get vaporized, in order, would be:

.#....###24...#..
##...##.13#67..9#
##...#...5.8####.
..#.....X...###..
..#.#.....#....##
Note that some asteroids (the ones behind the asteroids marked 1, 5, and 7) won't have a chance to be vaporized until the next full rotation. The laser continues rotating; the next nine to be vaporized are:

.#....###.....#..
##...##...#.....#
##...#......1234.
..#.....X...5##..
..#.9.....8....76
The next nine to be vaporized are then:

.8....###.....#..
56...9#...#.....#
34...7...........
..2.....X....##..
..1..............
Finally, the laser completes its first full rotation (1 through 3), a second rotation (4 through 8), and vaporizes the last asteroid (9) partway through its third rotation:

......234.....6..
......1...5.....7
.................
........X....89..
.................
In the large example above (the one with the best monitoring station location at 11,13):

The 1st asteroid to be vaporized is at 11,12.
The 2nd asteroid to be vaporized is at 12,1.
The 3rd asteroid to be vaporized is at 12,2.
The 10th asteroid to be vaporized is at 12,8.
The 20th asteroid to be vaporized is at 16,0.
The 50th asteroid to be vaporized is at 16,9.
The 100th asteroid to be vaporized is at 10,16.
The 199th asteroid to be vaporized is at 9,6.
The 200th asteroid to be vaporized is at 8,2.
The 201st asteroid to be vaporized is at 10,9.
The 299th and final asteroid to be vaporized is at 11,1.
The Elves are placing bets on which will be the 200th asteroid to be vaporized. Win the bet by determining which asteroid that will be; what do you get if you multiply its X coordinate by 100 and then add its Y coordinate? (For example, 8,2 becomes 802.)
*/

namespace Day10
{
    class Program
    {
        static bool[,] map;
        static List<Tuple<int, int>> asteroids;

        public static int Width { get; private set; }
        public static int Height { get; private set; }
        public static int AsteroidsCount
        {
            get { return asteroids.Count; }
        }

        public static bool Map(int x, int y)
        {
            return map[x, y];
        }

        private Program(string inputFile, bool part1)
        {
            var lines = ReadProgram(inputFile);
            ParseMap(lines);
            if (part1)
            {
                var bestAsteroid = ComputeBestAsteroid();
                Console.WriteLine($"Day10 : Result1 {bestAsteroid}");
                var result = CountVisible(bestAsteroid.Item1, bestAsteroid.Item2);
                Console.WriteLine($"Day10 : Result1 {result}");
            }
            else
            {
                var sortedAsteroids = SortAsteroidsFromPoint(13, 17);
                var result = sortedAsteroids[200 - 1];
                Console.WriteLine($"Day10 : Result2 {result}");
                Console.WriteLine($"Day10 : Result2 {result.Item1 * 100 + result.Item2}");
            }
        }

        public static Tuple<int, int> ComputeBestAsteroid()
        {
            Tuple<int, int> bestAsteroid = null;
            var mostVisible = int.MinValue;
            foreach (var asteroid in asteroids)
            {
                int fromX = asteroid.Item1;
                int fromY = asteroid.Item2;
                var countVisible = CountVisible(fromX, fromY);
                if (countVisible > mostVisible)
                {
                    mostVisible = countVisible;
                    bestAsteroid = asteroid;
                }
            }
            return bestAsteroid;
        }

        public static int CountVisible(int fromX, int fromY)
        {
            if (!map[fromX, fromY])
                return 0;

            var sortedAsteroids = SortAsteroidsFromPoint(fromX, fromY);
            int count = 0;
            foreach (var asteroid in asteroids)
            {
                int toX = asteroid.Item1;
                int toY = asteroid.Item2;
                if (IsVisibleFrom(sortedAsteroids, fromX, fromY, toX, toY))
                {
                    ++count;
                }
            }
            return count;
        }

        public static bool IsVisibleFrom(Tuple<int, int, double, double>[] sortedAsteroids, int fromX, int fromY, int toX, int toY)
        {
            double dx = toX - fromX;
            double dy = toY - fromY;
            if ((toX == fromX) && (toY == fromY))
            {
                return false;
            }
            double distanceSq = dx * dx + dy * dy;
            double angle = Math.PI - Math.Atan2(dx, dy);
            foreach (var sa in sortedAsteroids)
            {
                if (Math.Abs(sa.Item3 - angle) < 1.0e-6d)
                {
                    if (sa.Item4 < distanceSq)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static Tuple<int, int, double, double>[] SortAsteroidsFromPoint(int fromX, int fromY)
        {
            var sortedAsteroids = new List<Tuple<int, int, double, double>>();
            var unsortedAsteroids = new List<List<Tuple<int, int, double, double>>>();
            int maxDepth = int.MinValue;
            foreach (var asteroid in asteroids)
            {
                int aX = asteroid.Item1;
                int aY = asteroid.Item2;
                if ((aX == fromX) && (aY == fromY))
                {
                    continue;
                }
                double dx = aX - fromX;
                double dy = aY - fromY;
                double distanceSq = dx * dx + dy * dy;
                double angle = Math.PI - Math.Atan2(dx, dy);
                var newSA = new Tuple<int, int, double, double>(aX, aY, angle, distanceSq);
                bool added = false;
                var depth = int.MinValue;
                foreach (var sa in unsortedAsteroids)
                {
                    if (Math.Abs(sa[0].Item3 - angle) < 1.0e-6d)
                    {
                        bool inList = false;
                        for (int d = 0; d < sa.Count; ++d)
                        {
                            if (distanceSq < sa[d].Item4)
                            {
                                sa.Insert(d, newSA);
                                inList = true;
                                break;
                            }
                            if (inList)
                            {
                                break;
                            }
                        }
                        if (!inList)
                        {
                            sa.Add(newSA);
                        }
                        added = true;
                        depth = sa.Count;
                        maxDepth = Math.Max(maxDepth, depth);
                        break;
                    }
                    if (added)
                    {
                        break;
                    }
                }
                if (!added)
                {
                    var newList = new List<Tuple<int, int, double, double>>();
                    newList.Add(newSA);
                    unsortedAsteroids.Add(newList);
                    depth = newList.Count;
                    maxDepth = Math.Max(maxDepth, depth);
                }
            }
            for (int d = 0; d < maxDepth; ++d)
            {
                bool addOne = true;
                while (addOne)
                {
                    double smallestAngle = double.MaxValue;
                    double smallestDistance = double.MaxValue;
                    int bestAsteroidIndex = int.MaxValue;
                    addOne = false;
                    for (int i = 0; i < unsortedAsteroids.Count; ++i)
                    {
                        if (d < unsortedAsteroids[i].Count)
                        {
                            if (unsortedAsteroids[i][d] != null)
                            {
                                var angle = unsortedAsteroids[i][d].Item3;
                                var distanceSq = unsortedAsteroids[i][d].Item4;
                                if (angle < smallestAngle)
                                {
                                    smallestAngle = angle;
                                    smallestDistance = distanceSq;
                                    bestAsteroidIndex = i;
                                    addOne = true;
                                }
                                else if (Math.Abs(smallestAngle - angle) < 1.0e-6d)
                                {
                                    if (distanceSq < smallestDistance)
                                    {
                                        smallestAngle = angle;
                                        smallestDistance = distanceSq;
                                        bestAsteroidIndex = i;
                                        addOne = true;
                                    }
                                }
                            }
                        }
                    }
                    if (addOne)
                    {
                        sortedAsteroids.Add(unsortedAsteroids[bestAsteroidIndex][d]);
                        unsortedAsteroids[bestAsteroidIndex][d] = null;
                    }
                }
            }
            if (sortedAsteroids.Count + 1 != asteroids.Count)
            {
                throw new InvalidDataException($"sortedAsteroids Count is wrong {sortedAsteroids.Count} != {asteroids.Count}");
            }
            return sortedAsteroids.ToArray();
        }

        public static void ParseMap(string[] lines)
        {
            Width = lines[0].Length;
            Height = lines.Length;
            foreach (var line in lines)
            {
                if (line.Length != Width)
                {
                    throw new InvalidDataException($"width of lines must be the same {line.Length} != {Width}");
                }
            }
            map = new bool[Width, Height];
            asteroids = new List<Tuple<int, int>>();
            for (var y = 0; y < Height; ++y)
            {
                var line = lines[y];
                for (var x = 0; x < Width; ++x)
                {
                    var pixel = line[x];
                    if (pixel == '.')
                    {
                        map[x, y] = false;
                    }
                    else if (pixel == '#')
                    {
                        map[x, y] = true;
                        asteroids.Add(new Tuple<int, int>(x, y));
                    }
                    else
                    {
                        throw new InvalidDataException($"Invalid character {pixel}");
                    }
                }
            }
        }

        private string[] ReadProgram(string inputFile)
        {
            var lines = File.ReadAllLines(inputFile);
            return lines;
        }

        public static void Run()
        {
            Console.WriteLine("Day10 : Start");
            _ = new Program("Day10/input.txt", true);
            _ = new Program("Day10/input.txt", false);
            Console.WriteLine("Day10 : End");
        }
    }
}
