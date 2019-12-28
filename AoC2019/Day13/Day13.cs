using System;
using System.IO;

/*

--- Day 13: Care Package ---

As you ponder the solitude of space and the ever-increasing three-hour roundtrip for messages between you and Earth, you notice that the Space Mail Indicator Light is blinking. To help keep you sane, the Elves have sent you a care package.

It's a new game for the ship's arcade cabinet! Unfortunately, the arcade is all the way on the other end of the ship. Surely, it won't be hard to build your own - the care package even comes with schematics.

The arcade cabinet runs Intcode software like the game the Elves sent (your puzzle input). It has a primitive screen capable of drawing square tiles on a grid. The software draws tiles to the screen with output instructions: every three output instructions specify the x position (distance from the left), y position (distance from the top), and tile id. The tile id is interpreted as follows:

0 is an empty tile. No game object appears in this tile.
1 is a wall tile. Walls are indestructible barriers.
2 is a block tile. Blocks can be broken by the ball.
3 is a horizontal paddle tile. The paddle is indestructible.
4 is a ball tile. The ball moves diagonally and bounces off objects.
For example, a sequence of output values like 1,2,3,6,5,4 would draw a horizontal paddle tile (1 tile from the left and 2 tiles from the top) and a ball tile (6 tiles from the left and 5 tiles from the top).

Start the game. How many block tiles are on the screen when the game exits?

--- Part Two ---

The game didn't run because you didn't put in any quarters. Unfortunately, you did not bring any quarters. Memory address 0 represents the number of quarters that have been inserted; set it to 2 to play for free.

The arcade cabinet has a joystick that can move left and right. The software reads the position of the joystick with input instructions:

If the joystick is in the neutral position, provide 0.
If the joystick is tilted to the left, provide -1.
If the joystick is tilted to the right, provide 1.
The arcade cabinet also has a segment display capable of showing a single number that represents the player's current score. When three output instructions specify X=-1, Y=0, the third output instruction is not a tile; the value instead specifies the new score to show in the segment display. For example, a sequence of output values like -1,0,12345 would show 12345 as the player's current score.

Beat the game by breaking all the blocks. What is your score after the last block is broken?

*/

namespace Day13
{
    class Program
    {
        static int MAX_SCREEN_SIZE = 1024;
        static int[,] screen;
        static int score;

        private Program(string inputFile, bool part1)
        {
            var source = ReadProgram(inputFile);
            if (part1)
            {
                RunProgram(source, false);
                var result = CountBlocks();
                Console.WriteLine($"Day13 : Result1 {result}");
                if (result != 296)
                {
                    throw new InvalidProgramException($"Part1 result has been broken {result}");
                }
            }
            else
            {
                RunProgram(source, true);
                Console.WriteLine($"Day13 : BlockCount {CountBlocks()}");
                Console.WriteLine($"Day13 : Result2 {score}");
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

        static public void RunProgram(string source, bool part2)
        {
            screen = new int[MAX_SCREEN_SIZE, MAX_SCREEN_SIZE];
            var data = ConvertSourceStringToInts(source);
            if (part2)
            {
                data[0] = 2;
            }
            Int64 relativeBase = 0;
            Int64 pc = 0;
            bool halt = false;
            int outputPhase = 0;
            int screenX = MAX_SCREEN_SIZE / 2;
            int screenY = MAX_SCREEN_SIZE / 2;
            int paddleX = -1;
            int paddleY = -1;
            int ballX = -1;
            int ballY = -1;
            int joystick = 0;
            while (!halt)
            {
                bool hasOutput = false;
                int input = joystick;
                var output = RunProgram(ref data, ref pc, ref relativeBase, input, ref halt, ref hasOutput);
                if (halt && hasOutput)
                {
                    throw new InvalidDataException($"halt and hasOutput can't be both true pc:{pc}");
                }
                if (halt)
                {
                    if (outputPhase != 0)
                    {
                        throw new InvalidDataException($"outputPhase should be 0 at the end of the program");
                    }
                    OutputScreen();
                    return;
                }
                if (!hasOutput)
                {
                    throw new InvalidDataException($"hasOutput should be true pc:{pc}");
                }
                if (outputPhase == 0)
                {
                    screenX = (int)output;
                    ++outputPhase;
                }
                else if (outputPhase == 1)
                {
                    screenY = (int)output;
                    ++outputPhase;
                }
                else if (outputPhase == 2)
                {
                    if ((screenX == -1) && (screenY == 0))
                    {
                        score = (int)output;
                    }
                    else
                    {
                        screen[screenX, screenY] = (int)output;
                        //3 is a horizontal paddle tile. The paddle is indestructible.
                        if (output == 3)
                        {
                            paddleX = screenX;
                            paddleY = screenY;
                            OutputScreen();
                        }
                        //4 is a ball tile. The ball moves diagonally and bounces off objects.
                        if (output == 4)
                        {
                            ballX = screenX;
                            ballY = screenY;
                            OutputScreen();
                        }
                        /*
						If the joystick is in the neutral position, provide 0.
						If the joystick is tilted to the left, provide -1.
						If the joystick is tilted to the right, provide 1.
						*/
                        if (ballX < paddleX)
                        {
                            joystick = -1;
                        }
                        else if (ballX > paddleX)
                        {
                            joystick = +1;
                        }
                        else if (ballX == paddleX)
                        {
                            joystick = 0;
                        }
                        if (ballY > paddleY)
                        {
                            joystick = 0;
                        }
                    }
                    outputPhase = 0;
                }
            }
            OutputScreen();
            return;
        }

        static int CountBlocks()
        {
            int blockCount = 0;
            for (int y = 0; y < MAX_SCREEN_SIZE; ++y)
            {
                for (int x = 0; x < MAX_SCREEN_SIZE; ++x)
                {
                    if (screen[x, y] == 2)
                    {
                        ++blockCount;
                    }
                }
            }
            return blockCount;
        }

        static void OutputScreen()
        {
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;
            for (int y = 0; y < MAX_SCREEN_SIZE; ++y)
            {
                for (int x = 0; x < MAX_SCREEN_SIZE; ++x)
                {
                    if (screen[x, y] != 0)
                    {
                        minX = Math.Min(minX, x);
                        minY = Math.Min(minY, y);
                        maxX = Math.Max(maxX, x);
                        maxY = Math.Max(maxY, y);
                    }
                }
            }
            for (int y = minY; y <= maxY; ++y)
            {
                string line = "";
                for (int x = minX; x <= maxX; ++x)
                {
                    //0 is an empty tile. No game object appears in this tile.
                    if (screen[x, y] == 0)
                    {
                        line += ' ';
                    }
                    //1 is a wall tile. Walls are indestructible barriers.
                    else if (screen[x, y] == 1)
                    {
                        line += '@';
                    }
                    //2 is a block tile. Blocks can be broken by the ball.
                    else if (screen[x, y] == 2)
                    {
                        line += '#';
                    }
                    //3 is a horizontal paddle tile. The paddle is indestructible.
                    else if (screen[x, y] == 3)
                    {
                        line += '_';
                    }
                    //4 is a ball tile. The ball moves diagonally and bounces off objects.
                    else if (screen[x, y] == 4)
                    {
                        line += 'o';
                    }
                    else
                    {
                        throw new InvalidDataException($"Invalid screen[{x},{y}] value {screen[x, y]}");
                    }
                }
                Console.WriteLine(line);
            }
            Console.WriteLine($"Score {score}");
        }

        static public Int64 RunProgram(ref Int64[] data, ref Int64 pc, ref Int64 relativeBase, int input, ref bool halt, ref bool hasOutput)
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
            Console.WriteLine("Day13 : Start");
            _ = new Program("Day13/input.txt", true);
            _ = new Program("Day13/input.txt", false);
            Console.WriteLine("Day13 : End");
        }
    }
}
