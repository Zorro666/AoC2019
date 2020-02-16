using System;
using System.Collections.Generic;
using System.IO;

/*

--- Day 23: Category Six ---

The droids have finished repairing as much of the ship as they can. Their report indicates that this was a Category 6 disaster - not because it was that bad, but because it destroyed the stockpile of Category 6 network cables as well as most of the ship's network infrastructure.

You'll need to rebuild the network from scratch.

The computers on the network are standard Intcode computers that communicate by sending packets to each other. There are 50 of them in total, each running a copy of the same Network Interface Controller (NIC) software (your puzzle input). The computers have network addresses 0 through 49; when each computer boots up, it will request its network address via a single input instruction. Be sure to give each computer a unique network address.

Once a computer has received its network address, it will begin doing work and communicating over the network by sending and receiving packets. All packets contain two values named X and Y. Packets sent to a computer are queued by the recipient and read in the order they are received.

To send a packet to another computer, the NIC will use three output instructions that provide the destination address of the packet followed by its X and Y values. For example, three output instructions that provide the values 10, 20, 30 would send a packet with X=20 and Y=30 to the computer with address 10.

To receive a packet from another computer, the NIC will use an input instruction. If the incoming packet queue is empty, provide -1. Otherwise, provide the X value of the next packet; the computer will then use a second input instruction to receive the Y value for the same packet. Once both values of the packet are read in this way, the packet is removed from the queue.

Note that these input and output instructions never block. Specifically, output instructions do not wait for the sent packet to be received - the computer might send multiple packets before receiving any. Similarly, input instructions do not wait for a packet to arrive - if no packet is waiting, input instructions should receive -1.

Boot up all 50 computers and attach them to your network. What is the Y value of the first packet sent to address 255?
    
Your puzzle answer was 23626.

--- Part Two ---

Packets sent to address 255 are handled by a device called a NAT (Not Always Transmitting). The NAT is responsible for managing power consumption of the network by blocking certain packets and watching for idle periods in the computers.

If a packet would be sent to address 255, the NAT receives it instead. The NAT remembers only the last packet it receives; that is, the data in each packet it receives overwrites the NAT's packet memory with the new packet's X and Y values.

The NAT also monitors all computers on the network. If all computers have empty incoming packet queues and are continuously trying to receive packets without sending packets, the network is considered idle.

Once the network is idle, the NAT sends only the last packet it received to address 0; this will cause the computers on the network to resume activity. In this way, the NAT can throttle power consumption of the network when the ship needs power in other areas.

Monitor packets released to the computer at address 0 by the NAT. What is the first Y value delivered by the NAT to the computer at address 0 twice in a row?

*/

namespace Day23
{
    class Program
    {
        static readonly int COMPUTER_COUNT = 50;
        static readonly IntProgram[] sComputers = new IntProgram[COMPUTER_COUNT];
        static readonly List<long>[] sInputs = new List<long>[COMPUTER_COUNT];

        private Program(string inputFile, bool part1)
        {
            var program = ReadProgram(inputFile);
            for (var i = 0; i < COMPUTER_COUNT; ++i)
            {
                sComputers[i].CreateProgram(program);
                sComputers[i].SetNextInput(i);
                sInputs[i] = new List<long>();
                sInputs[i].Clear();
            }

            if (part1)
            {
                long result1 = RunSimulation(part1);
                Console.WriteLine($"Day23: Result1 {result1}");
                long expected = 23626;
                if (result1 != expected)
                {
                    throw new InvalidDataException($"Part1 result is broken {result1} != {expected}");
                }
            }
            else
            {
                long result1 = RunSimulation(part1);
                Console.WriteLine($"Day23: Result2 {result1}");
                long expected = 19019;
                if (result1 != expected)
                {
                    throw new InvalidDataException($"Part1 result is broken {result1} != {expected}");
                }
            }
        }

        long RunSimulation(bool part1)
        {
            long natXValue = long.MinValue;
            long natYValue = long.MinValue;
            long lastNATYValue = long.MinValue;
            long cyclesSinceOutput = 0;
            while (true)
            {
                bool areInputsEmpty = true;
                for (var i = 0; i < COMPUTER_COUNT; ++i)
                {
                    if (sInputs[i].Count != 0)
                    {
                        areInputsEmpty = false;
                    }
                }
                bool hadOutput = false;
                for (var i = 0; i < COMPUTER_COUNT; ++i)
                {
                    bool halt = false;
                    bool readInput = false;
                    bool hasOutput = false;
                    long computerOutput = sComputers[i].SingleStep(ref halt, ref hasOutput, ref readInput);
                    if (halt)
                    {
                        if (readInput)
                        {
                            throw new InvalidProgramException($"halt when sending input to computer {i}");
                        }
                        if (hasOutput)
                        {
                            throw new InvalidProgramException($"halt when reading output from computer {i}");
                        }
                        break;
                    }
                    if (readInput && hasOutput)
                    {
                        throw new InvalidProgramException($"reading input and writing output from computer {i}");
                    }
                    if (readInput)
                    {
                        long nextInput = -1;
                        if (sInputs[i].Count > 0)
                        {
                            nextInput = sInputs[i][0];
                            sInputs[i].RemoveAt(0);
                        }
                        sComputers[i].SetNextInput(nextInput);
                        if ((i == 0) && (nextInput != -1))
                        {
                            //Console.WriteLine($"Computer {i} Set Next Input {nextInput}");
                        }
                    }
                    if (hasOutput)
                    {
                        hadOutput = true;
                        long destination = computerOutput;
                        hasOutput = false;
                        long X = sComputers[i].GetNextOutput(ref halt, ref hasOutput);
                        if (halt)
                        {
                            throw new InvalidProgramException($"halt when reading X value from computer {i}");
                        }
                        hasOutput = false;
                        long Y = sComputers[i].GetNextOutput(ref halt, ref hasOutput);
                        if (halt)
                        {
                            throw new InvalidProgramException($"halt when reading Y value from computer {i}");
                        }
                        if (destination == 255)
                        {
                            natXValue = X;
                            natYValue = Y;
                        }
                        else if ((destination < 0) || (destination >= COMPUTER_COUNT))
                        {
                            throw new InvalidDataException($"Invalid destination {destination} from computer {i}");
                        }
                        else
                        {
                            sInputs[destination].Add(X);
                            sInputs[destination].Add(Y);
                        }
                        //Console.WriteLine($"Message from {i} to {destination} {X} {Y}");
                    }
                    if (part1 && (natYValue != long.MinValue))
                    {
                        return natYValue;
                    }
                }
                if (hadOutput)
                {
                    cyclesSinceOutput = 0;
                }
                else
                {
                    ++cyclesSinceOutput;
                }
                if (areInputsEmpty && (cyclesSinceOutput > 10000) && (natXValue != long.MinValue) && (natYValue != long.MinValue))
                {
                    //Console.WriteLine($"Nat X:{natXValue} Y:{natYValue} LastY:{lastNATYValue}");
                    sInputs[0].Add(natXValue);
                    sInputs[0].Add(natYValue);
                    if (lastNATYValue == natYValue)
                    {
                        return natYValue;
                    }
                    lastNATYValue = natYValue;
                    cyclesSinceOutput = 0;
                }
            }
        }

        private string ReadProgram(string inputFile)
        {
            var program = File.ReadAllText(inputFile);
            return program;
        }

        public static void Run()
        {
            Console.WriteLine("Day23 : Start");
            _ = new Program("Day23/input.txt", true);
            _ = new Program("Day23/input.txt", false);
            Console.WriteLine("Day23 : End");
        }
    }
}
