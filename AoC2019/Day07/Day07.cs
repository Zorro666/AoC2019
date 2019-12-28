using System;
using System.IO;

/*
--- Day 7: Amplification Circuit ---

Based on the navigational maps, you're going to need to send more power to your ship's thrusters to reach Santa in time.To do this, you'll need to configure a series of amplifiers already installed on the ship.


There are five amplifiers connected in series; each one receives an input signal and produces an output signal.They are connected such that the first amplifier's output leads to the second amplifier's input, the second amplifier's output leads to the third amplifier's input, and so on.The first amplifier's input value is 0, and the last amplifier's output leads to your ship's thrusters.


   O-------O O-------O O-------O O-------O O-------O
0 ->| Amp A |->| Amp B |->| Amp C |->| Amp D |->| Amp E |-> (to thrusters)
   O-------O O-------O O-------O O-------O O-------O
The Elves have sent you some Amplifier Controller Software (your puzzle input), a program that should run on your existing Intcode computer. Each amplifier will need to run a copy of the program.

When a copy of the program starts running on an amplifier, it will first use an input instruction to ask the amplifier for its current phase setting (an integer from 0 to 4). Each phase setting is used exactly once, but the Elves can't remember which amplifier needs which phase setting.

The program will then call another input instruction to get the amplifier's input signal, compute the correct output signal, and supply it back to the amplifier with an output instruction. (If the amplifier has not yet received an input signal, it waits until one arrives.)

Your job is to find the largest output signal that can be sent to the thrusters by trying every possible combination of phase settings on the amplifiers. Make sure that memory is not shared or reused between copies of the program.

For example, suppose you want to try the phase setting sequence 3,1,2,4,0, which would mean setting amplifier A to phase setting 3, amplifier B to setting 1, C to 2, D to 4, and E to 0.Then, you could determine the output signal that gets sent from amplifier E to the thrusters with the following steps:

Start the copy of the amplifier controller software that will run on amplifier A. At its first input instruction, provide it the amplifier's phase setting, 3. At its second input instruction, provide it the input signal, 0. After some calculations, it will use an output instruction to indicate the amplifier's output signal.
Start the software for amplifier B. Provide it the phase setting(1) and then whatever output signal was produced from amplifier A.It will then produce a new output signal destined for amplifier C.
Start the software for amplifier C, provide the phase setting(2) and the value from amplifier B, then collect its output signal.
Run amplifier D's software, provide the phase setting (4) and input value, and collect its output signal.
Run amplifier E's software, provide the phase setting (0) and input value, and collect its output signal.
The final output signal from amplifier E would be sent to the thrusters.However, this phase setting sequence may not have been the best one; another sequence might have sent a higher signal to the thrusters.

Here are some example programs:

Max thruster signal 43210(from phase setting sequence 4, 3, 2, 1, 0):
3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0

Max thruster signal 54321(from phase setting sequence 0, 1, 2, 3, 4):
3,23,3,24,1002,24,10,24,1002,23,-1,23,
101,5,23,23,1,24,23,23,4,23,99,0,0

Max thruster signal 65210(from phase setting sequence 1, 0, 4, 3, 2):
3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,
1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0

Try every combination of phase settings on the amplifiers. What is the highest signal that can be sent to the thrusters?

--- Part Two ---

It's no good - in this configuration, the amplifiers can't generate a large enough output signal to produce the thrust you'll need. The Elves quickly talk you through rewiring the amplifiers into a feedback loop:

      O-------O  O-------O  O-------O  O-------O  O-------O
0 -+->| Amp A |->| Amp B |->| Amp C |->| Amp D |->| Amp E |-.
   |  O-------O  O-------O  O-------O  O-------O  O-------O |
   |                                                        |
   '--------------------------------------------------------+
                                                            |
                                                            v
                                                     (to thrusters)
Most of the amplifiers are connected as they were before; amplifier A's output is connected to amplifier B's input, and so on. However, the output from amplifier E is now connected into amplifier A's input. This creates the feedback loop: the signal will be sent through the amplifiers many times.

In feedback loop mode, the amplifiers need totally different phase settings: integers from 5 to 9, again each used exactly once. These settings will cause the Amplifier Controller Software to repeatedly take input and produce output many times before halting. Provide each amplifier its phase setting at its first input instruction; all further input/output instructions are for signals.

Don't restart the Amplifier Controller Software on any amplifier during this process. Each one should continue receiving and sending signals until it halts.

All signals sent or received in this process will be between pairs of amplifiers except the very first signal and the very last signal. To start the process, a 0 signal is sent to amplifier A's input exactly once.

Eventually, the software on the amplifiers will halt after they have processed the final loop. When this happens, the last output signal from amplifier E is sent to the thrusters. Your job is to find the largest output signal that can be sent to the thrusters using the new phase settings and feedback loop arrangement.

Here are some example programs:

Max thruster signal 139629729 (from phase setting sequence 9,8,7,6,5):
3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,
27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5

Max thruster signal 18216 (from phase setting sequence 9,7,8,5,6):
3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,
-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,
53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10

Try every combination of the new phase settings on the amplifier feedback loop. What is the highest signal that can be sent to the thrusters?

*/

namespace Day07
{
    class Program
    {
        private Program(string inputFile, bool part1)
        {
            var source = ReadProgram(inputFile);
            if (part1)
            {
                int maxSignal = int.MinValue;
                for (int phase1 = 0; phase1 <= 4; ++phase1)
                {
                    for (int phase2 = 0; phase2 <= 4; ++phase2)
                    {
                        if (phase2 == phase1)
                        {
                            continue;
                        }
                        for (int phase3 = 0; phase3 <= 4; ++phase3)
                        {
                            if ((phase3 == phase2) || (phase3 == phase1))
                            {
                                continue;
                            }
                            for (int phase4 = 0; phase4 <= 4; ++phase4)
                            {
                                if ((phase4 == phase3) || (phase4 == phase2) || (phase4 == phase1))
                                {
                                    continue;
                                }
                                for (int phase5 = 0; phase5 <= 4; ++phase5)
                                {
                                    if ((phase5 == phase4) || (phase5 == phase3) || (phase5 == phase2) || (phase5 == phase1))
                                    {
                                        continue;
                                    }
                                    var signal = RunPrograms(source, phase1, phase2, phase3, phase4, phase5, false);
                                    if (signal > maxSignal)
                                    {
                                        maxSignal = signal;
                                    }
                                }
                            }
                        }
                    }
                }
                Console.WriteLine($"Day07 : Result1 {maxSignal}");
            }
            else
            {
                int maxSignal = int.MinValue;
                for (int phase1 = 5; phase1 <= 9; ++phase1)
                {
                    for (int phase2 = 5; phase2 <= 9; ++phase2)
                    {
                        if (phase2 == phase1)
                        {
                            continue;
                        }
                        for (int phase3 = 5; phase3 <= 9; ++phase3)
                        {
                            if ((phase3 == phase2) || (phase3 == phase1))
                            {
                                continue;
                            }
                            for (int phase4 = 5; phase4 <= 9; ++phase4)
                            {
                                if ((phase4 == phase3) || (phase4 == phase2) || (phase4 == phase1))
                                {
                                    continue;
                                }
                                for (int phase5 = 5; phase5 <= 9; ++phase5)
                                {
                                    if ((phase5 == phase4) || (phase5 == phase3) || (phase5 == phase2) || (phase5 == phase1))
                                    {
                                        continue;
                                    }
                                    var signal = RunPrograms(source, phase1, phase2, phase3, phase4, phase5, true);
                                    if (signal > maxSignal)
                                    {
                                        maxSignal = signal;
                                    }
                                }
                            }
                        }
                    }
                }
                Console.WriteLine($"Day07 : Result2 {maxSignal}");
            }
        }

        private string ReadProgram(string inputFile)
        {
            var source = File.ReadAllText(inputFile);
            return source;
        }

        static private int[] ConvertSourceStringToInts(string source)
        {
            var sourceElements = source.Split(',');
            var data = new int[sourceElements.Length];
            var index = 0;
            foreach (var element in sourceElements)
            {
                data[index] = Int32.Parse(element);
                index++;
            }
            return data;
        }

        static private string ConvertIntsToResultString(int[] data)
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

        static public int RunPrograms(string source, int phase1, int phase2, int phase3, int phase4, int phase5, bool useFeedback)
        {
            var data1 = ConvertSourceStringToInts(source);
            var data2 = ConvertSourceStringToInts(source);
            var data3 = ConvertSourceStringToInts(source);
            var data4 = ConvertSourceStringToInts(source);
            var data5 = ConvertSourceStringToInts(source);
            int input1 = phase1;
            int input2 = phase2;
            int input3 = phase3;
            int input4 = phase4;
            int input5 = phase5;
            int output1 = 0;
            int output2 = 0;
            int output3 = 0;
            int output4 = 0;
            int output5 = 0;
            int pc1 = 0;
            int pc2 = 0;
            int pc3 = 0;
            int pc4 = 0;
            int pc5 = 0;
            bool hasOutput = false;
            bool halt1 = false;
            bool halt2 = false;
            bool halt3 = false;
            bool halt4 = false;
            bool halt5 = false;
            bool allHalted = false;
            bool warmup = true;
            while (!allHalted)
            {
                allHalted = true;
                if (!halt1)
                {
                    allHalted = false;
                    if (!warmup)
                    {
                        if (useFeedback)
                        {
                            input1 = output5;
                        }
                        else
                        {
                            input1 = 0;
                        }
                    }
                    var output = RunProgram(ref data1, ref pc1, ref hasOutput, ref halt1, input1, warmup);
                    if (hasOutput)
                    {
                        output1 = output;
                    }
                }
                if (!halt2)
                {
                    allHalted = false;
                    if (!warmup)
                    {
                        input2 = output1;
                    }
                    var output = RunProgram(ref data2, ref pc2, ref hasOutput, ref halt2, input2, warmup);
                    if (hasOutput)
                    {
                        output2 = output;
                    }
                }
                if (!halt3)
                {
                    allHalted = false;
                    if (!warmup)
                    {
                        input3 = output2;
                    }
                    var output = RunProgram(ref data3, ref pc3, ref hasOutput, ref halt3, input3, warmup);
                    if (hasOutput)
                    {
                        output3 = output;
                    }
                }
                if (!halt4)
                {
                    allHalted = false;
                    if (!warmup)
                    {
                        input4 = output3;
                    }
                    var output = RunProgram(ref data4, ref pc4, ref hasOutput, ref halt4, input4, warmup);
                    if (hasOutput)
                    {
                        output4 = output;
                    }
                }
                if (!halt5)
                {
                    allHalted = false;
                    if (!warmup)
                    {
                        input5 = output4;
                    }
                    var output = RunProgram(ref data5, ref pc5, ref hasOutput, ref halt5, input5, warmup);
                    if (hasOutput)
                    {
                        output5 = output;
                    }
                }
                warmup = false;
            }
            return output5;
        }

        static public int RunProgram(ref int[] data, ref int pc, ref bool hasOutput, ref bool halt, int input, bool stopOnInput)
        {
            var instruction = data[pc];
            var result = 0;
            hasOutput = false;
            halt = false;
            while (instruction != 99)
            {
                if (pc >= data.Length)
                {
                    throw new InvalidDataException($"Invalid pc:{pc}");
                }
                var opcode = instruction % 100;
                var param1Mode = (instruction / 100) % 10;
                var param2Mode = (instruction / 1000) % 10;
                var param3Mode = (instruction / 10000) % 10;

                if ((param1Mode != 0) && (param1Mode != 1))
                {
                    throw new ArgumentOutOfRangeException("param1Mode", $"Invalid param1Mode:{param1Mode}");
                }
                if ((param2Mode != 0) && (param2Mode != 1))
                {
                    throw new ArgumentOutOfRangeException("param2Mode", $"Invalid param1Mode:{param2Mode}");
                }
                if (param3Mode != 0)
                {
                    throw new ArgumentOutOfRangeException("param3Mode", $"Invalid param3Mode:{param3Mode}");
                }

                if (opcode == 1)
                {
                    var param1Index = data[pc + 1];
                    var param2Index = data[pc + 2];
                    var param1 = (param1Mode == 0) ? data[param1Index] : param1Index;
                    var param2 = (param2Mode == 0) ? data[param2Index] : param2Index;
                    var outputIndex = data[pc + 3];
                    var output = param1 + param2;
                    data[outputIndex] = output;
                    pc += 4;
                }
                else if (opcode == 2)
                {
                    var param1Index = data[pc + 1];
                    var param2Index = data[pc + 2];
                    var param1 = (param1Mode == 0) ? data[param1Index] : param1Index;
                    var param2 = (param2Mode == 0) ? data[param2Index] : param2Index;
                    var outputIndex = data[pc + 3];
                    var output = param1 * param2;
                    data[outputIndex] = output;
                    pc += 4;
                }
                else if (opcode == 3)
                {
                    var param1Index = data[pc + 1];
                    data[param1Index] = input;
                    pc += 2;
                    if (stopOnInput)
                    {
                        return 0;
                    }
                }
                else if (opcode == 4)
                {
                    var param1Index = data[pc + 1];
                    var param1 = (param1Mode == 0) ? data[param1Index] : param1Index;
                    result = param1;
                    pc += 2;
                    hasOutput = true;
                    return result;
                }
                else if (opcode == 5)
                {
                    var param1Index = data[pc + 1];
                    var param2Index = data[pc + 2];
                    var param1 = (param1Mode == 0) ? data[param1Index] : param1Index;
                    var param2 = (param2Mode == 0) ? data[param2Index] : param2Index;
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
                    var param1Index = data[pc + 1];
                    var param2Index = data[pc + 2];
                    var param1 = (param1Mode == 0) ? data[param1Index] : param1Index;
                    var param2 = (param2Mode == 0) ? data[param2Index] : param2Index;
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
                    var param1Index = data[pc + 1];
                    var param2Index = data[pc + 2];
                    var param1 = (param1Mode == 0) ? data[param1Index] : param1Index;
                    var param2 = (param2Mode == 0) ? data[param2Index] : param2Index;
                    var outputIndex = data[pc + 3];
                    int output = 0;
                    if (param1 < param2)
                    {
                        output = 1;
                    }
                    data[outputIndex] = output;
                    pc += 4;
                }
                else if (opcode == 8)
                {
                    var param1Index = data[pc + 1];
                    var param2Index = data[pc + 2];
                    var param1 = (param1Mode == 0) ? data[param1Index] : param1Index;
                    var param2 = (param2Mode == 0) ? data[param2Index] : param2Index;
                    var outputIndex = data[pc + 3];
                    int output = 0;
                    if (param1 == param2)
                    {
                        output = 1;
                    }
                    data[outputIndex] = output;
                    pc += 4;
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

        public static void Run()
        {
            Console.WriteLine("Day07 : Start");
            _ = new Program("Day07/input.txt", true);
            _ = new Program("Day07/input.txt", false);
            Console.WriteLine("Day07 : End");
        }
    }
}
