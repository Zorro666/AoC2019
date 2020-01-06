using System;
using System.Collections.Generic;
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

--- Part Two ---

Now for the tricky part: notifying all the other robots about the solar flare. The vacuum robot can do this automatically if it gets into range of a robot. However, you can't see the other robots on the camera, so you need to be thorough instead: you need to make the vacuum robot visit every part of the scaffold at least once.

The vacuum robot normally wanders randomly, but there isn't time for that today. Instead, you can override its movement logic with new rules.

Force the vacuum robot to wake up by changing the value in your ASCII program at address 0 from 1 to 2. When you do this, you will be automatically prompted for the new movement rules that the vacuum robot should use. The ASCII program will use input instructions to receive them, but they need to be provided as ASCII code; end each line of logic with a single newline, ASCII code 10.

First, you will be prompted for the main movement routine. The main routine may only call the movement functions: A, B, or C. Supply the movement functions to use as ASCII text, separating them with commas (,, ASCII code 44), and ending the list with a newline (ASCII code 10). For example, to call A twice, then alternate between B and C three times, provide the string A,A,B,C,B,C,B,C and then a newline.

Then, you will be prompted for each movement function. Movement functions may use L to turn left, R to turn right, or a number to move forward that many units. Movement functions may not call other movement functions. Again, separate the actions with commas and end the list with a newline. For example, to move forward 10 units, turn left, move forward 8 units, turn right, and finally move forward 6 units, provide the string 10,L,8,R,6 and then a newline.

Finally, you will be asked whether you want to see a continuous video feed; provide either y or n and a newline. Enabling the continuous video feed can help you see what's going on, but it also requires a significant amount of processing power, and may even cause your Intcode computer to overheat.

Due to the limited amount of memory in the vacuum robot, the ASCII definitions of the main routine and the movement functions may each contain at most 20 characters, not counting the newline.

For example, consider the following camera feed:

#######...#####
#.....#...#...#
#.....#...#...#
......#...#...#
......#...###.#
......#.....#.#
^########...#.#
......#.#...#.#
......#########
........#...#..
....#########..
....#...#......
....#...#......
....#...#......
....#####......
In order for the vacuum robot to visit every part of the scaffold at least once, one path it could take is:

R,8,R,8,R,4,R,4,R,8,L,6,L,2,R,4,R,4,R,8,R,8,R,8,L,6,L,2
Without the memory limit, you could just supply this whole string to function A and have the main routine call A once. However, you'll need to split it into smaller parts.

One approach is:

Main routine: A,B,C,B,A,C
(ASCII input: 65, 44, 66, 44, 67, 44, 66, 44, 65, 44, 67, 10)
Function A:   R,8,R,8
(ASCII input: 82, 44, 56, 44, 82, 44, 56, 10)
Function B:   R,4,R,4,R,8
(ASCII input: 82, 44, 52, 44, 82, 44, 52, 44, 82, 44, 56, 10)
Function C:   L,6,L,2
(ASCII input: 76, 44, 54, 44, 76, 44, 50, 10)
Visually, this would break the desired path into the following parts:

A,        B,            C,        B,            A,        C
R,8,R,8,  R,4,R,4,R,8,  L,6,L,2,  R,4,R,4,R,8,  R,8,R,8,  L,6,L,2

CCCCCCA...BBBBB
C.....A...B...B
C.....A...B...B
......A...B...B
......A...CCC.B
......A.....C.B
^AAAAAAAA...C.B
......A.A...C.B
......AAAAAA#AB
........A...C..
....BBBB#BBBB..
....B...A......
....B...A......
....B...A......
....BBBBA......
Of course, the scaffolding outside your ship is much more complex.

As the vacuum robot finds other robots and notifies them of the impending solar flare, it also can't help but leave them squeaky clean, collecting any space dust it finds. Once it finishes the programmed set of movements, assuming it hasn't drifted off into space, the cleaning robot will return to its docking station and report the amount of space dust it collected as a large, non-ASCII value in a single output instruction.

After visiting every part of the scaffold at least once, how much dust does the vacuum robot report it has collected?

*/

namespace Day17
{
    class Program
    {
        static IntProgram sIntProgram = new IntProgram();
        static readonly int MAX_MAP_SIZE = 1024;
        static readonly int MOVE_LEFT = -1;
        static readonly int MOVE_RIGHT = -2;
        static readonly int SUB_A = -3;
        static readonly int SUB_B = -4;
        static readonly int SUB_C = -5;
        static readonly long[,] sMap = new long[MAX_MAP_SIZE, MAX_MAP_SIZE];
        static readonly List<(int X, int Y)> sIntersections = new List<(int X, int Y)>();
        static readonly List<int> sMoves = new List<int>();
        static List<int> sFinalMoves = null;
        static List<int> sPatternA = null;
        static List<int> sPatternB = null;
        static List<int> sPatternC = null;
        static bool sFoundSolution = false;

        static readonly long Unknown = 0;
        static readonly long Scaffold = '#';
        static readonly long RobotUp = '^';
        static readonly long RobotDown = 'v';
        static readonly long RobotLeft = '<';
        static readonly long RobotRight = '>';
        static readonly long RobotTumble = 'X';

        enum Direction
        {
            North = 0,
            South = 1,
            West = 2,
            East = 3
        };

        private Program(string inputFile, bool part1)
        {
            sIntProgram.LoadProgram(inputFile);
            sIntProgram.SetData(0, 1);
            RunProgram();
            if (part1)
            {
                FindIntersections();

                var result = 0;
                foreach (var intersection in sIntersections)
                {
                    result += intersection.X * intersection.Y;
                }
                Console.WriteLine($"Day17 : Result1 {result}");
                if (result != 4864)
                {
                    throw new InvalidProgramException($"Part1 result has been broken {result}");
                }
            }
            else
            {
                WalkRobot();
                OutputMoves(sMoves);
                sFoundSolution = false;
                FindPattern(sMoves, SUB_A);

                sIntProgram.LoadProgram(inputFile);
                sIntProgram.SetData(0, 2);
                var result = RunProgramWithInput();
                Console.WriteLine($"Day17 : Result2 {result}");
                if (result != 840248)
                {
                    throw new InvalidProgramException($"Part2 result has been broken {result}");
                }
            }
        }

        static void FindPattern(List<int> inMoves, int patternName)
        {
            if (patternName == SUB_A)
            {
                sFoundSolution = false;
            }
            var movesCount = inMoves.Count;
            for (int patternStartPos = 0; patternStartPos < movesCount; patternStartPos++)
            {
                int maxPatternLength = Math.Min(10, movesCount - patternStartPos);
                for (int patternLength = 2; patternLength < maxPatternLength; patternLength++)
                {
                    List<int> pattern = new List<int>();
                    for (int i = 0; i < patternLength; ++i)
                    {
                        var move = inMoves[patternStartPos + i];
                        if (move > SUB_A)
                        {
                            pattern.Add(move);
                        }
                    }
                    var patternCharCount = GetPatternCharCount(pattern);
                    if ((patternCharCount > 0) && (patternCharCount <= 20))
                    {
                        if (patternName == SUB_A)
                        {
                            sPatternA = pattern;
                        }
                        else if (patternName == SUB_B)
                        {
                            sPatternB = pattern;
                        }
                        else if (patternName == SUB_C)
                        {
                            sPatternC = pattern;
                        }
                        var newMoves = SubstitutePattern(inMoves, pattern, patternName);
                        var movesString = MovesToString(newMoves);
                        //Console.WriteLine($"Moves:{movesString} Len:{movesString.Length}");
                        if (movesString.Length <= 20)
                        {
                            sFoundSolution = true;
                            sFinalMoves = newMoves;
                            Console.WriteLine($"Found solution");
                            OutputMoves(sFinalMoves);
                            OutputPattern(SUB_A, sPatternA);
                            OutputPattern(SUB_B, sPatternB);
                            OutputPattern(SUB_C, sPatternC);
                            return;
                        }
                        if (patternName == SUB_A)
                        {
                            //Console.WriteLine($"Pattern A");
                            //OutputPattern(pattern);
                            FindPattern(newMoves, SUB_B);
                        }
                        else if (patternName == SUB_B)
                        {
                            //Console.WriteLine($"Pattern B");
                            //OutputPattern(pattern);
                            FindPattern(newMoves, SUB_C);
                        }
                        else if (patternName == SUB_C)
                        {
                            //Console.WriteLine($"Pattern C");
                            //OutputPattern(pattern);
                        }
                    }
                    if (sFoundSolution)
                    {
                        return;
                    }
                }
                if (sFoundSolution)
                {
                    return;
                }
            }
        }

        static int GetPatternCharCount(List<int> pattern)
        {
            var moves = MovesToString(pattern);
            return moves.Length;
        }

        static List<int> SubstitutePattern(List<int> inMoves, List<int> pattern, int patternName)
        {
            List<int> moves = new List<int>();
            int patternLen = pattern.Count;
            for (int m = 0; m < inMoves.Count;)
            {
                bool match = true;
                for (int p = 0; p < patternLen; ++p)
                {
                    if (m + p >= inMoves.Count)
                    {
                        match = false;
                        break;
                    }
                    if (inMoves[m + p] != pattern[p])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    moves.Add(patternName);
                    m += patternLen;
                }
                else
                {
                    moves.Add(inMoves[m]);
                    m++;
                }
            }
            return moves;
        }

        static string MovesToString(List<int> intMoves)
        {
            string moves = "";
            foreach (var move in intMoves)
            {
                string thisMove = "";
                if (move == MOVE_LEFT)
                {
                    thisMove = "L";
                }
                else if (move == MOVE_RIGHT)
                {
                    thisMove = "R";
                }
                else if (move == SUB_A)
                {
                    thisMove = "A";
                }
                else if (move == SUB_B)
                {
                    thisMove = "B";
                }
                else if (move == SUB_C)
                {
                    thisMove = "C";
                }
                else
                {
                    thisMove = move.ToString();
                }
                if (moves.Length > 0)
                {
                    moves += ",";
                }
                moves += thisMove;
            }
            return moves;
        }

        static void OutputMoves(List<int> intMoves)
        {
            var moves = MovesToString(intMoves);
            Console.WriteLine($"Moves:{moves} Len:{moves.Length}");
        }

        static void OutputPattern(int patternName, List<int> intMoves)
        {
            var moves = MovesToString(intMoves);
            var patternString = "";
            if (patternName == SUB_A)
            {
                patternString = "A";
            }
            else if (patternName == SUB_B)
            {
                patternString = "B";
            }
            else if (patternName == SUB_C)
            {
                patternString = "C";
            }
            Console.WriteLine($"Pattern {patternString}:{moves} Len:{moves.Length}");
        }

        static void ConvertMovePatternToInput(ref List<long> inputList, List<int> moves)
        {
            for (int i = 0; i < moves.Count; i++)
            {
                if (i > 0)
                {
                    inputList.Add(',');
                }
                int m = moves[i];
                if (m == MOVE_LEFT)
                {
                    inputList.Add('L');
                }
                else if (m == MOVE_RIGHT)
                {
                    inputList.Add('R');
                }
                else if (m == SUB_A)
                {
                    inputList.Add('A');
                }
                else if (m == SUB_B)
                {
                    inputList.Add('B');
                }
                else if (m == SUB_C)
                {
                    inputList.Add('C');
                }
                else if ((m >= 1) && (m <= 9))
                {
                    inputList.Add(m + '0');
                }
                else if ((m >= 10) && (m <= 19))
                {
                    inputList.Add((m / 10) + '0');
                    inputList.Add(m % 10 + '0');
                }
            }
            inputList.Add(10);
        }

        static public long RunProgramWithInput()
        {
            List<long> inputList = new List<long>();
            ConvertMovePatternToInput(ref inputList, sFinalMoves);
            ConvertMovePatternToInput(ref inputList, sPatternA);
            ConvertMovePatternToInput(ref inputList, sPatternB);
            ConvertMovePatternToInput(ref inputList, sPatternC);
            inputList.Add('n');
            inputList.Add(10);
            string line = "";
            foreach (var i in inputList)
            {
                line += i;
                if (i == 10)
                {
                    Console.WriteLine($"{line}");
                    line = "";
                }
                else
                {
                    line += " ";
                }
            }
            if (line.Length > 0)
            {
                Console.WriteLine($"{line}");
            }
            sIntProgram.SetInputData(inputList.ToArray());
            RunProgram();
            bool halt = false;
            long result = 0;
            while (!halt)
            {
                var output = GetOutput();
                if (output == -666)
                {
                    halt = true;
                }
                else
                {
                    result = output;
                }
            }
            return result;
        }

        static public void RunProgram()
        {
            var x = 0;
            var y = 0;
            bool halt = false;
            while (!halt)
            {
                var output = GetOutput();
                halt = ParseOutput(output, ref x, ref y);
                if (output == '?')
                {
                    halt = true;
                }
            }
            OutputMap();
        }

        static bool ParseOutput(long output, ref int x, ref int y)
        {
            if (output == -666)
            {
                return true;
            }
            if (output == 10)
            {
                y++;
                x = 0;
            }
            else
            {
                sMap[x++, y] = output;
            }
            return false;
        }

        static void FindIntersections()
        {
            sIntersections.Clear();
            for (int y = 0; y < MAX_MAP_SIZE; ++y)
            {
                for (int x = 0; x < MAX_MAP_SIZE; ++x)
                {
                    // Neighbours
                    int numNeighbourCount = 0;
                    if (sMap[x, y] == Scaffold)
                    {
                        if (x > 0 && sMap[x - 1, y] == Scaffold)
                        {
                            ++numNeighbourCount;
                        }
                        if (x + 1 < MAX_MAP_SIZE && sMap[x + 1, y] == Scaffold)
                        {
                            ++numNeighbourCount;
                        }
                        if (y > 0 && sMap[x, y - 1] == Scaffold)
                        {
                            ++numNeighbourCount;
                        }
                        if (y + 1 < MAX_MAP_SIZE && sMap[x, y + 1] == Scaffold)
                        {
                            ++numNeighbourCount;
                        }
                        if (numNeighbourCount >= 3)
                        {
                            // Found an intersection
                            Console.WriteLine($"Intersection at {x},{y} {numNeighbourCount}");
                            sIntersections.Add((x, y));
                        }
                    }
                }
            }
        }

        static (int X, int Y, Direction Facing) FindRobot()
        {
            for (int y = 0; y < MAX_MAP_SIZE; ++y)
            {
                for (int x = 0; x < MAX_MAP_SIZE; ++x)
                {
                    if (sMap[x, y] == RobotUp)
                        return (x, y, Direction.North);
                    else if (sMap[x, y] == RobotDown)
                        return (x, y, Direction.South);
                    else if (sMap[x, y] == RobotLeft)
                        return (x, y, Direction.West);
                    else if (sMap[x, y] == RobotRight)
                        return (x, y, Direction.East);
                    else if (sMap[x, y] == RobotTumble)
                        throw new InvalidDataException($"Robot has fallen off {x},{y}");
                }
            }
            throw new InvalidDataException($"Robot was not found");
        }

        static (int X, int Y) GetNextPosition(int x, int y, Direction facing)
        {
            switch (facing)
            {
                case Direction.North:
                    return (x, y - 1);
                case Direction.South:
                    return (x, y + 1);
                case Direction.West:
                    return (x - 1, y);
                case Direction.East:
                    return (x + 1, y);
            }
            throw new InvalidDataException($"Invalid direction {facing}");
        }

        static bool IsScaffold(int x, int y)
        {
            if (x < 0)
            {
                return false;
            }
            if (y < 0)
            {
                return false;
            }
            if (x >= MAX_MAP_SIZE)
            {
                return false;
            }
            if (y >= MAX_MAP_SIZE)
            {
                return false;
            }
            return sMap[x, y] == Scaffold;
        }

        static void WalkRobot()
        {
            (int robotX, int robotY, Direction robotFacing) = FindRobot();
            bool finished = false;
            int newX;
            int newY;
            int moved = 0;
            bool triedLeft = false;
            bool triedRight = false;
            Direction lastFacing = robotFacing;
            sMoves.Clear();
            while (!finished)
            {
                (newX, newY) = GetNextPosition(robotX, robotY, robotFacing);
                bool validMove = IsScaffold(newX, newY);
                if (validMove)
                {
                    lastFacing = robotFacing;
                    if (moved == 0)
                    {
                        //Output Turn
                        if (triedLeft && !triedRight)
                        {
                            sMoves.Add(MOVE_LEFT);
                        }
                        else if (triedLeft && triedRight)
                        {
                            sMoves.Add(MOVE_RIGHT);
                        }
                    }
                    robotX = newX;
                    robotY = newY;
                    moved++;
                    triedLeft = false;
                    triedRight = false;
                }
                else
                {
                    // Output Movement
                    if (moved > 0)
                    {
                        sMoves.Add(moved);
                        moved = 0;
                    }
                    if (!triedLeft)
                    {
                        triedLeft = true;
                        triedRight = false;
                        switch (lastFacing)
                        {
                            case Direction.North:
                                robotFacing = Direction.West;
                                break;
                            case Direction.South:
                                robotFacing = Direction.East;
                                break;
                            case Direction.West:
                                robotFacing = Direction.South;
                                break;
                            case Direction.East:
                                robotFacing = Direction.North;
                                break;
                        }
                    }
                    else if (!triedRight)
                    {
                        triedRight = true;
                        switch (lastFacing)
                        {
                            case Direction.North:
                                robotFacing = Direction.East;
                                break;
                            case Direction.South:
                                robotFacing = Direction.West;
                                break;
                            case Direction.West:
                                robotFacing = Direction.North;
                                break;
                            case Direction.East:
                                robotFacing = Direction.South;
                                break;
                        }
                    }
                    else
                    {
                        finished = true;
                    }
                }
            };
        }

        static long GetOutput()
        {
            bool halt = false;
            bool hasOutput = false;
            var output = sIntProgram.RunProgram(ref halt, ref hasOutput);
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
                    if (sMap[x, y] != Unknown)
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
                    line += (char)sMap[x, y];
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
