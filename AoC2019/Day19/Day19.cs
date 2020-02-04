using System;
using System.IO;

/*
--- Day 19: Tractor Beam ---

Unsure of the state of Santa's ship, you borrowed the tractor beam technology from Triton. Time to test it out.

When you're safely away from anything else, you activate the tractor beam, but nothing happens. It's hard to tell whether it's working if there's nothing to use it on. Fortunately, your ship's drone system can be configured to deploy a drone to specific coordinates and then check whether it's being pulled. There's even an Intcode program (your puzzle input) that gives you access to the drone system.

The program uses two input instructions to request the X and Y position to which the drone should be deployed. Negative numbers are invalid and will confuse the drone; all numbers should be zero or positive.

Then, the program will output whether the drone is stationary (0) or being pulled by something (1). For example, the coordinate X=0, Y=0 is directly in front of the tractor beam emitter, so the drone control program will always report 1 at that location.

To better understand the tractor beam, it is important to get a good picture of the beam itself. For example, suppose you scan the 10x10 grid of points closest to the emitter:

       X
  0->      9
 0#.........
 |.#........
 v..##......
  ...###....
  ....###...
Y .....####.
  ......####
  ......####
  .......###
 9........##
In this example, the number of points affected by the tractor beam in the 10x10 area closest to the emitter is 27.

However, you'll need to scan a larger area to understand the shape of the beam. How many points are affected by the tractor beam in the 50x50 area closest to the emitter? (For each of X and Y, this will be 0 through 49.)

--- Part Two ---

You aren't sure how large Santa's ship is. You aren't even sure if you'll need to use this thing on Santa's ship, but it doesn't hurt to be prepared. You figure Santa's ship might fit in a 100x100 square.

The beam gets wider as it travels away from the emitter; you'll need to be a minimum distance away to fit a square of that size into the beam fully. (Don't rotate the square; it should be aligned to the same axes as the drone grid.)

For example, suppose you have the following tractor beam readings:

#.......................................
.#......................................
..##....................................
...###..................................
....###.................................
.....####...............................
......#####.............................
......######............................
.......#######..........................
........########........................
.........#########......................
..........#########.....................
...........##########...................
...........############.................
............############................
.............#############..............
..............##############............
...............###############..........
................###############.........
................#################.......
.................########OOOOOOOOOO.....
..................#######OOOOOOOOOO#....
...................######OOOOOOOOOO###..
....................#####OOOOOOOOOO#####
.....................####OOOOOOOOOO#####
.....................####OOOOOOOOOO#####
......................###OOOOOOOOOO#####
.......................##OOOOOOOOOO#####
........................#OOOOOOOOOO#####
.........................OOOOOOOOOO#####
..........................##############
..........................##############
...........................#############
............................############
.............................###########
In this example, the 10x10 square closest to the emitter that fits entirely within the tractor beam has been marked O. Within it, the point closest to the emitter (the only highlighted O) is at X=25, Y=20.

Find the 100x100 square closest to the emitter that fits entirely within the tractor beam; within that square, find the point closest to the emitter. What value do you get if you take that point's X coordinate, multiply it by 10000, then add the point's Y coordinate? (In the example above, this would be 250020.)

*/

namespace Day19
{
    class Program
    {
        static IntProgram sProgram = new IntProgram();
        static int sMapSize;
        static int[] sLeftEdge;
        static int[] sRightEdge;
        static char[,] sMap;

        private Program(string inputFile, bool part1)
        {
            sProgram.LoadProgram(inputFile);

            if (part1)
            {
                sMapSize = 50;
                GenerateMap();
                //OutputMap();
                var result = CountAffected();
                Console.WriteLine($"Day19 : Result1 {result}");
                if (116 != result)
                {
                    throw new InvalidDataException($"Part1 result has been broken {result} != 116");
                }
            }
            else
            {
                sMapSize = 10000;
                var bottomEdge = FindSantaSquare();
                var topEdge = bottomEdge - 99;
                var leftTopEdge = sLeftEdge[topEdge];
                var rightTopEdge = sRightEdge[topEdge];
                var leftBottomEdge = sLeftEdge[bottomEdge];
                var rightBottomEdge = sRightEdge[bottomEdge];
                Console.WriteLine($"Top[{topEdge}] {leftTopEdge} -> {rightTopEdge} Bottom[{bottomEdge}] {leftBottomEdge} -> {rightBottomEdge}");
                var x = rightTopEdge - 100;
                var y = topEdge;
                var result = x * 10000 + y;
                Console.WriteLine($"Day19 : Result2 {result}");
                if (10311666 != result)
                {
                    throw new InvalidDataException($"Part2 result has been broken {result} != 10311666");
                }
            }
        }

        static long CountAffected()
        {
            long count = 0;
            for (int y = 0; y < sMapSize; ++y)
            {
                for (int x = 0; x < sMapSize; ++x)
                {
                    count += sMap[x, y] == '.' ? 0 : 1;
                }
            }
            return count;
        }

        static int FindSantaSquare()
        {
            bool halt = false;
            bool hasOutput = false;
            InitMap();
            var inputs = new long[2];
            int xLeft = 0;
            int xRight = -1;
            for (int y = 0; y < sMapSize; ++y)
            {
                bool beamStarted = false;
                int beamStartX = -1;
                int beamEndX = -1;
                for (int x = xLeft; x < sMapSize; ++x)
                {
                    long result;
                    inputs[0] = x;
                    inputs[1] = y;
                    sProgram.Reset();
                    sProgram.SetInputData(inputs);
                    result = sProgram.RunProgram(ref halt, ref hasOutput);
                    if (result == 1)
                    {
                        if (!beamStarted)
                        {
                            sLeftEdge[y] = x;
                            beamStartX = x;
                            xLeft = x - 2;
                            if (xLeft < 0)
                            {
                                xLeft = 0;
                            }
                            x = xRight;
                            if (x < beamStartX)
                            {
                                x = beamStartX;
                            }
                        }
                        beamStarted = true;
                    }
                    else if (beamStarted && (result == 0))
                    {
                        sRightEdge[y] = x;
                        beamEndX = x;
                        for (int i = beamStartX; i < beamEndX; ++i)
                        {
                            sMap[i, y] = '#';
                        }
                        xRight = x - 1;
                        if (xRight < 0)
                        {
                            xRight = 0;
                        }
                        break;
                    }
                }
                if ((beamEndX - beamStartX) >= 100)
                {
                    if (y >= 100)
                    {
                        Console.WriteLine($"[{y}] {beamStartX} -> {beamEndX}");
                        int rightTopEdge = sRightEdge[y - 99];
                        int leftBottomEdge = sLeftEdge[y];
                        if ((leftBottomEdge + 99) < rightTopEdge)
                        {
                            return y;
                        }
                    }
                }
            }
            return -1;
        }

        static void InitMap()
        {
            sMap = new char[sMapSize, sMapSize];
            sLeftEdge = new int[sMapSize];
            sRightEdge = new int[sMapSize];
            for (int y = 0; y < sMapSize; ++y)
            {
                for (int x = 0; x < sMapSize; ++x)
                {
                    sMap[x, y] = '.';
                }
            }
        }

        static void GenerateMap()
        {
            bool halt = false;
            bool hasOutput = false;
            InitMap();

            var inputs = new long[2];
            for (int y = 0; y < sMapSize; ++y)
            {
                for (int x = 0; x < sMapSize; ++x)
                {
                    sMap[x, y] = '.';
                }
            }

            int xLeft = 0;
            int xRight = -1;
            int beamStartX = -1;
            for (int y = 0; y < sMapSize; ++y)
            {
                bool beamStarted = false;
                for (int x = xLeft; x < sMapSize; ++x)
                {
                    long result;
                    inputs[0] = x;
                    inputs[1] = y;
                    sProgram.Reset();
                    sProgram.SetInputData(inputs);
                    result = sProgram.RunProgram(ref halt, ref hasOutput);
                    char output = result == 1 ? '#' : '.';
                    sMap[x, y] = output;
                    if (result == 1)
                    {
                        if (!beamStarted)
                        {
                            beamStartX = x;
                            xLeft = x - 2;
                            if (xLeft < 0)
                            {
                                xLeft = 0;
                            }
                            x = xRight;
                            if (x < beamStartX)
                            {
                                x = beamStartX;
                            }
                        }
                        beamStarted = true;
                    }
                    else if (beamStarted && (result == 0))
                    {
                        xRight = x - 1;
                        if (xRight < 0)
                        {
                            xRight = 0;
                        }
                        for (int i = beamStartX; i < x; ++i)
                        {
                            sMap[i, y] = '#';
                        }
                        break;
                    }
                }
            }
        }

        static void OutputMap()
        {

            for (int y = 0; y < sMapSize; ++y)
            {
                string line = "";
                for (int x = 0; x < sMapSize; ++x)
                {
                    line += sMap[x, y];
                }
                Console.WriteLine(line);
            }
        }
        public static void Run()
        {
            Console.WriteLine("Day19 : Start");
            _ = new Program("Day19/input.txt", true);
            _ = new Program("Day19/input.txt", false);
            Console.WriteLine("Day19 : End");
        }
    }
}
