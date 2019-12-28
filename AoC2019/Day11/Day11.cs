using System;
using System.IO;

/*
--- Day 11: Space Police ---

On the way to Jupiter, you're pulled over by the Space Police.

"Attention, unmarked spacecraft! You are in violation of Space Law! All spacecraft must have a clearly visible registration identifier! You have 24 hours to comply or be sent to Space Jail!"

Not wanting to be sent to Space Jail, you radio back to the Elves on Earth for help. Although it takes almost three hours for their reply signal to reach you, they send instructions for how to power up the emergency hull painting robot and even provide a small Intcode program (your puzzle input) that will cause it to paint your ship appropriately.

There's just one problem: you don't have an emergency hull painting robot.

You'll need to build a new emergency hull painting robot. The robot needs to be able to move around on the grid of square panels on the side of your ship, detect the color of its current panel, and paint its current panel black or white. (All of the panels are currently black.)

The Intcode program will serve as the brain of the robot. The program uses input instructions to access the robot's camera: provide 0 if the robot is over a black panel or 1 if the robot is over a white panel. Then, the program will output two values:

First, it will output a value indicating the color to paint the panel the robot is over: 0 means to paint the panel black, and 1 means to paint the panel white.
Second, it will output a value indicating the direction the robot should turn: 0 means it should turn left 90 degrees, and 1 means it should turn right 90 degrees.
After the robot turns, it should always move forward exactly one panel. The robot starts facing up.

The robot will continue running for a while like this and halt when it is finished drawing. Do not restart the Intcode computer inside the robot during this process.

For example, suppose the robot is about to start running. Drawing black panels as ., white panels as #, and the robot pointing the direction it is facing (< ^ > v), the initial state and region near the robot looks like this:

.....
.....
..^..
.....
.....
The panel under the robot (not visible here because a ^ is shown instead) is also black, and so any input instructions at this point should be provided 0. Suppose the robot eventually outputs 1 (paint white) and then 0 (turn left). After taking these actions and moving forward one panel, the region now looks like this:

.....
.....
.<#..
.....
.....
Input instructions should still be provided 0. Next, the robot might output 0 (paint black) and then 0 (turn left):

.....
.....
..#..
.v...
.....
After more outputs (1,0, 1,0):

.....
.....
..^..
.##..
.....
The robot is now back where it started, but because it is now on a white panel, input instructions should be provided 1. After several more outputs (0,1, 1,0, 1,0), the area looks like this:

.....
..<#.
...#.
.##..
.....
Before you deploy the robot, you should probably have an estimate of the area it will cover: specifically, you need to know the number of panels it paints at least once, regardless of color. In the example above, the robot painted 6 panels at least once. (It painted its starting panel twice, but that panel is still only counted once; it also never painted the panel it ended on.)

Build a new emergency hull painting robot and run the Intcode program on it. How many panels does it paint at least once

--- Part Two ---

You're not sure what it's trying to paint, but it's definitely not a registration identifier. The Space Police are getting impatient.

Checking your external ship cameras again, you notice a white panel marked "emergency hull painting robot starting panel". The rest of the panels are still black, but it looks like the robot was expecting to start on a white panel, not a black one.

Based on the Space Law Space Brochure that the Space Police attached to one of your windows, a valid registration identifier is always eight capital letters. After starting the robot on a single white panel instead, what registration identifier does it paint on your hull?

*/

namespace Day11
{
    class Program
    {
        private Program(string inputFile, bool part1)
        {
            var source = ReadProgram(inputFile);
            if (part1)
            {
                var result = RunProgram(source, 0);
                Console.WriteLine($"Day11 : Result1 {result}");
                if (result != 1967)
                {
                    throw new InvalidProgramException($"Part1 result has been broken {result}");
                }
            }
            else
            {
                var result = RunProgram(source, 1);
                Console.WriteLine($"Day11 : Result2 {result}");
            }
        }

        private string ReadProgram(string inputFile)
        {
            var source = File.ReadAllText(inputFile);
            return source;
        }

        static private Int64[] ConvertSourceStringToInts(string source)
        {
            var sourceElements = source.Split(',');
            var data = new Int64[sourceElements.Length];
            var index = 0;
            foreach (var element in sourceElements)
            {
                data[index] = Int64.Parse(element);
                index++;
            }
            return data;
        }

        static private string ConvertIntsToResultString(Int64[] data)
        {
            var sourceElements = new string[data.Length];
            int index = 0;
            foreach (var element in data)
            {
                sourceElements[index] = element.ToString();
                index++;
            }
            var result = String.Join(',', sourceElements);
            return result;
        }

        static public int RunProgram(string source, int startingColour)
        {
            var data = ConvertSourceStringToInts(source);
            Int64 relativeBase = 0;
            Int64 pc = 0;
            bool halt = false;
            int MAX_HULL_SIZE = 1024;
            int[,] hull = new int[MAX_HULL_SIZE, MAX_HULL_SIZE];
            bool[,] paintedTiles = new bool[MAX_HULL_SIZE, MAX_HULL_SIZE];
            int robotX = MAX_HULL_SIZE / 2;
            int robotY = MAX_HULL_SIZE / 2;
            // Roobot starts facing up
            int robotDX = 0;
            int robotDY = -1;
            bool paint = true;
            bool turnAndMove = false;
            int paintedTilesCount = 0;
            hull[robotX, robotY] = startingColour;
            paintedTiles[robotX, robotY] = startingColour == 1 ? true : false;
            while (!halt)
            {
                var input = hull[robotX, robotY];
                bool hasOutput = false;
                var output = RunProgram(ref data, ref pc, ref relativeBase, input, ref halt, ref hasOutput);
                if (halt && hasOutput)
                {
                    throw new InvalidDataException($"halt and hasOutput can't be both true pc:{pc}");
                }
                if (halt)
                {
                    if (!paint)
                    {
                        throw new InvalidDataException($"paint should be true at the end of the program");
                    }
                    OutputHull(hull, MAX_HULL_SIZE);
                    return paintedTilesCount;
                }
                if (!hasOutput)
                {
                    throw new InvalidDataException($"hasOutput should be true pc:{pc}");
                }
                if (paint)
                {
                    // Output1 = paint colour : 0, 1
                    hull[robotX, robotY] = (int)output;
                    if (!paintedTiles[robotX, robotY])
                    {
                        ++paintedTilesCount;
                        paintedTiles[robotX, robotY] = true;
                    }
                    paint = false;
                    turnAndMove = true;
                }
                else if (turnAndMove)
                {
                    // Output2 = 0 = turn left , 1 = turn right :  then move forwards
                    TurnRobot(output, ref robotDX, ref robotDY);
                    robotX += robotDX;
                    robotY += robotDY;
                    turnAndMove = false;
                    paint = true;
                }
            }
            OutputHull(hull, MAX_HULL_SIZE);
            return paintedTilesCount;
        }

        static void OutputHull(int[,] hull, int hullSize)
        {
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;
            for (int y = 0; y < hullSize; ++y)
            {
                for (int x = 0; x < hullSize; ++x)
                {
                    if (hull[x, y] != 0)
                    {
                        minX = Math.Min(minX, x);
                        minY = Math.Min(minY, y);
                        maxX = Math.Max(maxX, x);
                        maxY = Math.Max(maxY, y);
                        //Console.WriteLine($"{minX},{minY} -> {maxX},{maxY}");
                    }
                }
            }
            Console.WriteLine($"{minX},{minY} -> {maxX},{maxY}");
            for (int y = minY; y <= maxY; ++y)
            {
                string line = "";
                for (int x = minX; x <= maxX; ++x)
                {
                    if (hull[x, y] == 0)
                    {
                        line += ' ';
                    }
                    else
                    {
                        line += '#';
                    }
                }
                Console.WriteLine(line);
            }
        }

        // turn : 0 = turn left , 1 = turn right
        static public void TurnRobot(Int64 turn, ref int robotDX, ref int robotDY)
        {
            // Turn 0 = Left
            //  0 , -1 => -1,  0
            // -1 ,  0 =>  0, +1
            //  0 , +1 => +1,  0
            // +1 ,  0 =>  0, -1
            //newDX = oldDY;
            //newDY = -oldDX;

            // Turn 1 = Right
            //  0 , -1 => +1,  0
            // +1 ,  0 =>  0, +1
            //  0 , +1 => -1,  0
            // -1 ,  0 =>  0, -1
            //newDX = -oldDY;
            //newDY = oldDX;

            int newDX = turn == 0 ? robotDY : -robotDY;
            int newDY = turn == 0 ? -robotDX : robotDX;

            robotDX = newDX;
            robotDY = newDY;
        }

        static public Int64 RunProgram(ref Int64[] data, ref Int64 pc, ref Int64 relativeBase, Int64 input, ref bool halt, ref bool hasOutput)
        {
            Int64 instruction = data[pc];
            Int64 result = -666;
            while (instruction != 99)
            {
                if (pc >= data.Length)
                {
                    throw new InvalidDataException($"Invalid pc:{pc}");
                }
                Int64 opcode = instruction % 100;
                Int64 param1Mode = (instruction / 100) % 10;
                Int64 param2Mode = (instruction / 1000) % 10;
                Int64 param3Mode = (instruction / 10000) % 10;

                if ((param1Mode != 0) && (param1Mode != 1) && (param1Mode != 2))
                {
                    throw new ArgumentOutOfRangeException("param1Mode", $"Invalid param1Mode:{param1Mode}");
                }
                if ((param2Mode != 0) && (param2Mode != 1) && (param2Mode != 2))
                {
                    throw new ArgumentOutOfRangeException("param2Mode", $"Invalid param1Mode:{param2Mode}");
                }
                if ((param3Mode != 0) && (param3Mode != 2))
                {
                    throw new ArgumentOutOfRangeException("param3Mode", $"Invalid param3Mode:{param3Mode}");
                }

                if (opcode == 1)
                {
                    Int64 param1Index = data[pc + 1];
                    Int64 param2Index = data[pc + 2];
                    Int64 param3Index = data[pc + 3];
                    Int64 param1 = GetParam(ref data, relativeBase, param1Mode, param1Index);
                    Int64 param2 = GetParam(ref data, relativeBase, param2Mode, param2Index);
                    Int64 param3 = GetOutputIndex(relativeBase, param3Mode, param3Index);
                    Int64 output = param1 + param2;
                    MakeDataBigEnough(ref data, param3);
                    data[param3] = output;
                    pc += 4;
                }
                else if (opcode == 2)
                {
                    Int64 param1Index = data[pc + 1];
                    Int64 param2Index = data[pc + 2];
                    Int64 param3Index = data[pc + 3];
                    Int64 param1 = GetParam(ref data, relativeBase, param1Mode, param1Index);
                    Int64 param2 = GetParam(ref data, relativeBase, param2Mode, param2Index);
                    Int64 param3 = GetOutputIndex(relativeBase, param3Mode, param3Index);
                    Int64 output = param1 * param2;
                    MakeDataBigEnough(ref data, param3);
                    data[param3] = output;
                    pc += 4;
                }
                else if (opcode == 3)
                {
                    Int64 param1Index = data[pc + 1];
                    Int64 index = GetOutputIndex(relativeBase, param1Mode, param1Index);
                    MakeDataBigEnough(ref data, index);
                    data[index] = input;
                    pc += 2;
                }
                else if (opcode == 4)
                {
                    Int64 param1Index = data[pc + 1];
                    Int64 param1 = GetParam(ref data, relativeBase, param1Mode, param1Index);
                    result = param1;
                    hasOutput = true;
                    pc += 2;
                    return result;
                }
                else if (opcode == 5)
                {
                    Int64 param1Index = data[pc + 1];
                    Int64 param2Index = data[pc + 2];
                    Int64 param1 = GetParam(ref data, relativeBase, param1Mode, param1Index);
                    Int64 param2 = GetParam(ref data, relativeBase, param2Mode, param2Index);
                    if (param1 != 0)
                    {
                        pc = param2;
                    }
                    else
                    {
                        pc += 3;
                    }
                }
                else if (opcode == 6)
                {
                    Int64 param1Index = data[pc + 1];
                    Int64 param2Index = data[pc + 2];
                    //Console.WriteLine($"rb {relativeBase} param2Mode {param2Mode} param2Index {param2Index}");
                    Int64 param1 = GetParam(ref data, relativeBase, param1Mode, param1Index);
                    Int64 param2 = GetParam(ref data, relativeBase, param2Mode, param2Index);
                    if (param1 == 0)
                    {
                        pc = param2;
                    }
                    else
                    {
                        pc += 3;
                    }
                }
                else if (opcode == 7)
                {
                    Int64 param1Index = data[pc + 1];
                    Int64 param2Index = data[pc + 2];
                    Int64 param3Index = data[pc + 3];
                    Int64 param1 = GetParam(ref data, relativeBase, param1Mode, param1Index);
                    Int64 param2 = GetParam(ref data, relativeBase, param2Mode, param2Index);
                    Int64 param3 = GetOutputIndex(relativeBase, param3Mode, param3Index);
                    Int64 output = 0;
                    if (param1 < param2)
                    {
                        output = 1;
                    }
                    MakeDataBigEnough(ref data, param3);
                    data[param3] = output;
                    pc += 4;
                }
                else if (opcode == 8)
                {
                    Int64 param1Index = data[pc + 1];
                    Int64 param2Index = data[pc + 2];
                    Int64 param3Index = data[pc + 3];
                    Int64 param1 = GetParam(ref data, relativeBase, param1Mode, param1Index);
                    Int64 param2 = GetParam(ref data, relativeBase, param2Mode, param2Index);
                    Int64 param3 = GetOutputIndex(relativeBase, param3Mode, param3Index);
                    Int64 output = 0;
                    if (param1 == param2)
                    {
                        output = 1;
                    }
                    MakeDataBigEnough(ref data, param3);
                    data[param3] = output;
                    pc += 4;
                }
                else if (opcode == 9)
                {
                    Int64 param1Index = data[pc + 1];
                    Int64 param1 = GetParam(ref data, relativeBase, param1Mode, param1Index);
                    //Console.WriteLine($"rb OLD {relativeBase}");
                    relativeBase += param1;
                    //Console.WriteLine($"rb NEW {relativeBase}");
                    pc += 2;
                }
                else
                {
                    throw new InvalidDataException($"Unknown opcode:{opcode}");
                }
                //Console.WriteLine(ConvertIntsToResultString(data));
                instruction = data[pc];
            }
            halt = true;
            return result;
        }

        static Int64 GetParam(ref Int64[] data, Int64 relativeBase, Int64 paramMode, Int64 paramIndex)
        {
            if (paramMode == 1)
            {
                return paramIndex;
            }
            Int64 index;
            if (paramMode == 0)
            {
                index = paramIndex;
            }
            else if (paramMode == 2)
            {
                index = paramIndex + relativeBase;
            }
            else
            {
                throw new ArgumentOutOfRangeException("paramMode", $"Invalid paramMode {paramMode}");
            }
            MakeDataBigEnough(ref data, index);
            return data[index];
        }

        static void MakeDataBigEnough(ref Int64[] data, Int64 index)
        {
            if (index < 0)
            {
                throw new InvalidDataException($"Invalid parameter index {index}");
            }
            if (index >= data.Length)
            {
                Int64[] newData = new Int64[index + 1];
                int i = 0;
                foreach (var d in data)
                {
                    newData[i] = d;
                    ++i;
                }
                for (; i < index; ++i)
                {
                    newData[i] = 0;
                }
                data = newData;
            }
        }

        static Int64 GetOutputIndex(Int64 relativeBase, Int64 paramMode, Int64 paramIndex)
        {
            if (paramMode == 0)
            {
                return paramIndex;
            }
            else if (paramMode == 2)
            {
                return paramIndex + relativeBase;
            }
            else
            {
                throw new InvalidDataException($"Invalid input paramMode {paramMode}");
            }
        }

        public static void Run()
        {
            Console.WriteLine("Day11 : Start");
            _ = new Program("Day11/input.txt", true);
            _ = new Program("Day11/input.txt", false);
            Console.WriteLine("Day11 : End");
        }
    }
}

