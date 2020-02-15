﻿using System;
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
                bool foundAnswer = false;
                long result1 = -123;
                while (!foundAnswer)
                {
                    for (var i = 0; i < COMPUTER_COUNT; ++i)
                    {
                        bool halt = false;
                        bool readInput = false;
                        bool hasOutput = false;
                        long result = sComputers[i].SingleStep(ref halt, ref hasOutput, ref readInput);
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
                            //Console.WriteLine($"Computer {i} Set Next Input {nextInput}");
                        }
                        if (hasOutput)
                        {
                            long destination = result;
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
                                foundAnswer = true;
                                result1 = Y;
                                break;
                            }
                            if ((destination < 0) || (destination >= COMPUTER_COUNT))
                            {
                                throw new InvalidDataException($"Invalid destination {destination} from computer {i}");
                            }
                            sInputs[destination].Add(X);
                            sInputs[destination].Add(Y);
                            //Console.WriteLine($"Message from {i} to {destination} {X} {Y}");
                        }
                        if (foundAnswer)
                        {
                            break;
                        }
                    }
                }
                Console.WriteLine($"Day23: Result1 {result1}");
                long expected = 23626;
                if (result1 != expected)
                {
                    throw new InvalidDataException($"Part1 result is broken {result1} != {expected}");
                }
            }
            else
            {
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
