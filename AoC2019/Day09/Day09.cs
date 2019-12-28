using System;
using System.IO;

/*

--- Day 9: Sensor Boost ---

You've just said goodbye to the rebooted rover and left Mars when you receive a faint distress signal coming from the asteroid belt. It must be the Ceres monitoring station!

In order to lock on to the signal, you'll need to boost your sensors. The Elves send up the latest BOOST program - Basic Operation Of System Test.

While BOOST (your puzzle input) is capable of boosting your sensors, for tenuous safety reasons, it refuses to do so until the computer it runs on passes some checks to demonstrate it is a complete Intcode computer.

Your existing Intcode computer is missing one key feature: it needs support for parameters in relative mode.

Parameters in mode 2, relative mode, behave very similarly to parameters in position mode: the parameter is interpreted as a position. Like position mode, parameters in relative mode can be read from or written to.

The important difference is that relative mode parameters don't count from address 0. Instead, they count from a value called the relative base. The relative base starts at 0.

The address a relative mode parameter refers to is itself plus the current relative base. When the relative base is 0, relative mode parameters and position mode parameters with the same value refer to the same address.

For example, given a relative base of 50, a relative mode parameter of -7 refers to memory address 50 + -7 = 43.

The relative base is modified with the relative base offset instruction:

Opcode 9 adjusts the relative base by the value of its only parameter. The relative base increases (or decreases, if the value is negative) by the value of the parameter.
For example, if the relative base is 2000, then after the instruction 109,19, the relative base would be 2019. If the next instruction were 204,-34, then the value at address 1985 would be output.

Your Intcode computer will also need a few other capabilities:

The computer's available memory should be much larger than the initial program. Memory beyond the initial program starts with the value 0 and can be read or written like any other memory. (It is invalid to try to access memory at a negative address, though.)
The computer should have support for large numbers. Some instructions near the beginning of the BOOST program will verify this capability.
Here are some example programs that use these features:

109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99 takes no input and produces a copy of itself as output.
1102,34915192,34915192,7,4,7,99,0 should output a 16-digit number.
104,1125899906842624,99 should output the large number in the middle.
The BOOST program will ask for a single input; run it in test mode by providing it the value 1. It will perfom a series of checks on each opcode, output any opcodes (and the associated parameter modes) that seem to be functioning incorrectly, and finally output a BOOST keycode.

Once your Intcode computer is fully functional, the BOOST program should report no malfunctioning opcodes when run in test mode; it should only output a single value, the BOOST keycode. What BOOST keycode does it produce?

--- Part Two ---

You now have a complete Intcode computer.

Finally, you can lock on to the Ceres distress signal! You just need to boost your sensors using the BOOST program.

The program runs in sensor boost mode by providing the input instruction the value 2. Once run, it will boost the sensors automatically, but it might take a few seconds to complete the operation on slower hardware. In sensor boost mode, the program will output a single value: the coordinates of the distress signal.

Run the BOOST program in sensor boost mode. What are the coordinates of the distress signal?

*/

namespace Day09
{
    class Program
    {
        private Program(string inputFile, bool part1)
        {
            var source = ReadProgram(inputFile);
            string allOutput = "";
            if (part1)
            {
                var result = RunProgram(source, 1, ref allOutput);
                Console.WriteLine($"Day09 : Result1 {result}");
                Console.WriteLine($"Day09 : Result1 {allOutput}");
            }
            else
            {
                var result = RunProgram(source, 2, ref allOutput);
                Console.WriteLine($"Day09 : Result2 {result}");
                Console.WriteLine($"Day09 : Result2 {allOutput}");
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

        static public Int64 RunProgram(string source, Int64 input, ref string allOutput)
        {
            var data = ConvertSourceStringToInts(source);
            Int64 pc = 0;
            var output = RunProgram(ref data, ref pc, input, ref allOutput);
            return output;
        }

        static public Int64 RunProgram(ref Int64[] data, ref Int64 pc, Int64 input, ref string allOutput)
        {
            Int64 instruction = data[pc];
            Int64 result = -666;
            Int64 relativeBase = 0;
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
                    if (!string.IsNullOrWhiteSpace(allOutput))
                    {
                        allOutput += ",";
                    }
                    allOutput += result.ToString();
                    pc += 2;
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
                    relativeBase += param1;
                    pc += 2;
                }
                else
                {
                    throw new InvalidDataException($"Unknown opcode:{opcode}");
                }
                //Console.WriteLine(ConvertIntsToResultString(data));
                instruction = data[pc];
            }
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
            Console.WriteLine("Day09 : Start");
            _ = new Program("Day09/input.txt", true);
            _ = new Program("Day09/input.txt", false);
            Console.WriteLine("Day09 : End");
        }
    }
}
