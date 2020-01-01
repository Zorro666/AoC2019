using System;
using System.IO;

/*

--- Day 17: Set and Forget ---

An early warning system detects an incoming solar flare and automatically activates the ship's electromagnetic shield. Unfortunately, this has cut off the Wi-Fi for many small robots that, unaware of the impending danger, are now trapped on exterior scaffolding on the unsafe side of the shield. To rescue them, you'll have to act quickly!

The only tools at your disposal are some wired cameras and a small vacuum robot currently asleep at its charging station.The video quality is poor, but the vacuum robot has a needlessly bright LED that makes it easy to spot no matter where it is.

An Intcode program, the Aft Scaffolding Control and Information Interface(ASCII, your puzzle input), provides access to the cameras and the vacuum robot.Currently, because the vacuum robot is asleep, you can only access the cameras.

Running the ASCII program on your Intcode computer will provide the current view of the scaffolds.This is output, purely coincidentally, as ASCII code: 35 means #, 46 means ., 10 starts a new line of output below the current one, and so on. (Within a line, characters are drawn left-to-right.)

In the camera output, # represents a scaffold and . represents open space. The vacuum robot is visible as ^, v, <, or > depending on whether it is facing up, down, left, or right respectively. When drawn like this, the vacuum robot is always on a scaffold; if the vacuum robot ever walks off of a scaffold and begins tumbling through space uncontrollably, it will instead be visible as X.

In general, the scaffold forms a path, but it sometimes loops back onto itself.For example, suppose you can see the following view from the cameras:

..#..........
..#..........
#######...###
#.#...#...#.#
#############
..#...#...#..
..#####...^..
Here, the vacuum robot, ^ is facing up and sitting at one end of the scaffold near the bottom-right of the image. The scaffold continues up, loops across itself several times, and ends at the top-left of the image.


The first step is to calibrate the cameras by getting the alignment parameters of some well-defined points. Locate all scaffold intersections; for each, its alignment parameter is the distance between its left edge and the left edge of the view multiplied by the distance between its top edge and the top edge of the view.Here, the intersections from the above image are marked O:

..#..........
..#..........
##O####...###
#.#...#...#.#
##O###O###O##
..#...#...#..
..#####...^..
For these intersections:


The top-left intersection is 2 units from the left of the image and 2 units from the top of the image, so its alignment parameter is 2 * 2 = 4.
The bottom-left intersection is 2 units from the left and 4 units from the top, so its alignment parameter is 2 * 4 = 8.
The bottom-middle intersection is 6 from the left and 4 from the top, so its alignment parameter is 24.
The bottom-right intersection's alignment parameter is 40.
To calibrate the cameras, you need the sum of the alignment parameters. In the above example, this is 76.


Run your ASCII program. What is the sum of the alignment parameters for the scaffold intersections?

*/

namespace Day17
{
    class Program
    {
        static readonly int MAX_MAP_SIZE = 1024;
        static StatusCodes[,] sMap = new StatusCodes[MAX_MAP_SIZE, MAX_MAP_SIZE];
        static IntProgram sIntProgram = new IntProgram();

        enum StatusCodes
        {
            Unknown = 0,
            Scaffold = 1,
            Empty = 2,
            RobotUp = 3,
            RobotDown = 4,
            RobotLeft = 5,
            RobotRight = 6,
            RobotTumble = 7
        };

        private Program(string inputFile, bool part1)
        {
            sIntProgram.LoadProgram(inputFile);
            RunProgram();

            if (part1)
            {
                var result = 100;
                Console.WriteLine($"Day15 : Result1 {result}");
                if (result != 250)
                {
                    throw new InvalidProgramException($"Part1 result has been broken {result}");
                }
            }
            else
            {
                var result = 200;
                Console.WriteLine($"Day15 : Result2 {result}");
            }
        }

        static public void RunProgram()
        {
            var x = 0;
            var y = 0;
            bool halt = false;
            while (!halt)
            {
                var output = GetOutput();
                switch (output)
                {
                    /*
                    Running the ASCII program on your Intcode computer will provide the current view of the scaffolds.This is output, purely coincidentally, as ASCII code: 35 means #, 46 means ., 10 starts a new line of output below the current one, and so on. (Within a line, characters are drawn left-to-right.)
                    In the camera output, # represents a scaffold and . represents open space. The vacuum robot is visible as ^, v, <, or > depending on whether it is facing up, down, left, or right respectively. When drawn like this, the vacuum robot is always on a scaffold; if the vacuum robot ever walks off of a scaffold and begins tumbling through space uncontrollably, it will instead be visible as X.
                    */
                    case '#':
                        sMap[x++, y] = StatusCodes.Scaffold;
                        break;
                    case '.':
                        sMap[x++, y] = StatusCodes.Empty;
                        break;
                    case '^':
                        sMap[x++, y] = StatusCodes.RobotUp;
                        break;
                    case 'v':
                        sMap[x++, y] = StatusCodes.RobotDown;
                        break;
                    case '<':
                        sMap[x++, y] = StatusCodes.RobotLeft;
                        break;
                    case '>':
                        sMap[x++, y] = StatusCodes.RobotRight;
                        break;
                    case 'X':
                        sMap[x++, y] = StatusCodes.RobotTumble;
                        break;
                    case 10:
                        y++;
                        x = 0;
                        break;
                    default:
                        halt = true;
                        break;
                }
            }
            OutputMap();
        }

        static long GetOutput()
        {
            bool halt = false;
            bool hasOutput = false;
            var output = sIntProgram.RunProgram(0, ref halt, ref hasOutput);
            if (halt && hasOutput)
            {
                throw new InvalidDataException($"halt and hasOutput can't be both true");
            }
            if (halt)
            {
                return -666;
            }
            if (!hasOutput)
            {
                throw new InvalidDataException($"hasOutput should be true");
            }
            return output;
        }

        static void OutputMap()
        {
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;
            for (int y = 0; y < MAX_MAP_SIZE; ++y)
            {
                for (int x = 0; x < MAX_MAP_SIZE; ++x)
                {
                    if (sMap[x, y] != StatusCodes.Unknown)
                    {
                        minX = Math.Min(minX, x);
                        minY = Math.Min(minY, y);
                        maxX = Math.Max(maxX, x);
                        maxY = Math.Max(maxY, y);
                    }
                }
            }
            string topBottom = "|";
            for (int x = minX; x <= maxX; ++x)
            {
                topBottom += "-";
            }
            topBottom += "|";
            Console.WriteLine(topBottom);
            for (int y = minY; y <= maxY; ++y)
            {
                string line = "|";
                for (int x = minX; x <= maxX; ++x)
                {
                    switch (sMap[x, y])
                    {
                        case StatusCodes.Unknown:
                            line += ' ';
                            break;
                        case StatusCodes.Scaffold:
                            line += '#';
                            break;
                        case StatusCodes.Empty:
                            line += '.';
                            break;
                        case StatusCodes.RobotUp:
                            line += '^';
                            break;
                        case StatusCodes.RobotDown:
                            line += 'v';
                            break;
                        case StatusCodes.RobotLeft:
                            line += '<';
                            break;
                        case StatusCodes.RobotRight:
                            line += '>';
                            break;
                        case StatusCodes.RobotTumble:
                            line += 'X';
                            break;
                        default:
                            throw new InvalidDataException($"Invalid map value {sMap[x, y]}");
                    }
                }
                line += "|";
                Console.WriteLine(line);
            }
            Console.WriteLine(topBottom);
        }

        public static void Run()
        {
            Console.WriteLine("Day17 : Start");
            _ = new Program("Day17/input.txt", true);
            _ = new Program("Day17/input.txt", false);
            Console.WriteLine("Day17 : End");
        }
    }
}
