using System;
using System.Collections.Generic;
using System.IO;

/*

--- Day 25: Cryostasis ---

As you approach Santa's ship, your sensors report two important details:

First, that you might be too late: the internal temperature is -40 degrees.

Second, that one faint life signature is somewhere on the ship.

The airlock door is locked with a code; your best option is to send in a small droid to investigate the situation. You attach your ship to Santa's, break a small hole in the hull, and let the droid run in before you seal it up again. Before your ship starts freezing, you detach your ship and set it to automatically stay within range of Santa's ship.

This droid can follow basic instructions and report on its surroundings; you can communicate with it through an Intcode program (your puzzle input) running on an ASCII-capable computer.

As the droid moves through its environment, it will describe what it encounters. When it says Command?, you can give it a single instruction terminated with a newline (ASCII code 10). Possible instructions are:

Movement via north, south, east, or west.
To take an item the droid sees in the environment, use the command take <name of item>. For example, if the droid reports seeing a red ball, you can pick it up with take red ball.
To drop an item the droid is carrying, use the command drop <name of item>. For example, if the droid is carrying a green ball, you can drop it with drop green ball.
To get a list of all of the items the droid is currently carrying, use the command inv (for "inventory").
Extra spaces or other characters aren't allowed - instructions must be provided precisely.

Santa's ship is a Reindeer-class starship; these ships use pressure-sensitive floors to determine the identity of droids and crew members. The standard configuration for these starships is for all droids to weigh exactly the same amount to make them easier to detect. If you need to get past such a sensor, you might be able to reach the correct weight by carrying items from the environment.

Look around the ship and see if you can find the password for the main airlock.

*/

namespace Day25
{
    class Program
    {
        static IntProgram sComputer;
        static string[] sLastReplies;
        static bool[] sValidMoves;
        static string[] sItemsToPickup;
        static readonly int MAP_SIZE = 1024;
        static int[,] sMap;
        static int sRobotX;
        static int sRobotY;

        static readonly int North = 0;
        static readonly int South = 1;
        static readonly int West = 2;
        static readonly int East = 3;
        static readonly string[] sDirections = { "north", "south", "west", "east" };

        private Program(string inputFile, bool part1)
        {
            sComputer = new IntProgram();
            sComputer.LoadProgram(inputFile);
            sValidMoves = new bool[4];
            InitMap();
            ResetValidMoves();

            if (part1)
            {
                var result1 = -666;
                var expected = -123;
                WaitToEnterCommand();
                var r = new Random();
                for (int i = 0; i < 10000; ++i)
                {
                    int tryMove = r.Next(4);
                    if (sValidMoves[tryMove])
                    {
                        if (tryMove == North)
                        {
                            MoveNorth();
                        }
                        else if (tryMove == South)
                        {
                            MoveSouth();
                        }
                        else if (tryMove == West)
                        {
                            MoveWest();
                        }
                        else if (tryMove == East)
                        {
                            MoveEast();
                        }
                        OutputMap();
                    }
                }
                Console.WriteLine($"Day25: Part1 {result1}");
                if (result1 != expected)
                {
                    //throw new InvalidDataException($"Part1 is broken {result1} != {expected}");
                }
            }
            else
            {
                var result2 = -666;
                var expected = -123;
                Console.WriteLine($"Day25: Part2 {result2}");
                if (result2 != expected)
                {
                    throw new InvalidDataException($"Part2 is broken {result2} != {expected}");
                }
            }
        }

        void InitMap()
        {
            sMap = new int[MAP_SIZE, MAP_SIZE];
            for (var y = 0; y < MAP_SIZE; ++y)
            {
                for (var x = 0; x < MAP_SIZE; ++x)
                {
                    sMap[x, y] = -1;
                }
            }
            sRobotX = MAP_SIZE / 2;
            sRobotY = MAP_SIZE / 2;
        }

        void OutputMap()
        {
            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;
            for (var y = 0; y < MAP_SIZE; ++y)
            {
                for (var x = 0; x < MAP_SIZE; ++x)
                {
                    if (sMap[x, y] != -1)
                    {
                        minX = Math.Min(minX, x);
                        maxX = Math.Max(maxX, x);
                        minY = Math.Min(minY, y);
                        maxY = Math.Max(maxY, y);
                    }
                }
            }
            Console.WriteLine("----------------------------------");
            for (var y = minY; y <= maxY; ++y)
            {
                string line = "";
                for (var x = minX; x <= maxX; ++x)
                {
                    char c = ' ';
                    if (sMap[x, y] == 0)
                    {
                        c = '.';
                    }
                    else if (sMap[x, y] == 1)
                    {
                        c = '#';
                    }
                    if ((x == sRobotX) && (y == sRobotY))
                    {
                        c = '*';
                    }
                    line += c;
                }
                Console.WriteLine(line);
            }
            Console.WriteLine("----------------------------------");
        }

        void ResetValidMoves()
        {
            for (var i = 0; i < 4; ++i)
            {
                sValidMoves[i] = false;
            }
        }

        void MoveNorth()
        {
            if (EnterCommand("north"))
            {
                sRobotY -= 1;
                UpdateMap();
            }
        }

        void MoveSouth()
        {
            if (EnterCommand("south"))
            {
                sRobotY += 1;
                UpdateMap();
            }
        }

        void MoveWest()
        {
            if (EnterCommand("west"))
            {
                sRobotX -= 1;
                UpdateMap();
            }
        }

        void MoveEast()
        {
            if (EnterCommand("east"))
            {
                sRobotX += 1;
                UpdateMap();
            }
        }

        void Inventory()
        {
            EnterCommand("inv");
        }

        bool WaitToEnterCommand()
        {
            bool halt = false;
            bool hasOutput = false;
            List<string> replies = new List<string>();
            string line = "";
            while (!halt)
            {
                long result = sComputer.GetNextOutput(ref halt, ref hasOutput);
                if (result == 10)
                {
                    replies.Add(line);
                    if (line == "Command?")
                    {
                        break;
                    }
                    line = "";
                }
                else
                {
                    line += (char)result;
                }
            }
            sLastReplies = replies.ToArray();
            return ParseReply();
        }

        void UpdateMap()
        {
            if ((sRobotX <= 0) || (sRobotX >= MAP_SIZE))
            {
                throw new InvalidDataException($"Invalid RobotX {sRobotX} {0} - {MAP_SIZE}");
            }

            if ((sRobotY <= 0) || (sRobotY >= MAP_SIZE))
            {
                throw new InvalidDataException($"Invalid RobotY {sRobotY} {0} - {MAP_SIZE}");
            }

            if (sValidMoves[North])
            {
                sMap[sRobotX, sRobotY - 1] = 0;
            }
            if (sValidMoves[South])
            {
                sMap[sRobotX, sRobotY + 1] = 0;
            }
            if (sValidMoves[West])
            {
                sMap[sRobotX - 1, sRobotY] = 0;
            }
            if (sValidMoves[East])
            {
                sMap[sRobotX + 1, sRobotY] = 0;
            }
        }

        void OutputLastReplies()
        {
            foreach (var line in sLastReplies)
            {
                Console.WriteLine($"{line}");
            }
        }

        bool ParseReply()
        {
            /* 
            == <ROOM NAME> ==
            <DESCRIPTION>
            <CR>
            Doors here lead:
            - <direction> x N
            <CR>
            Items here:
            - <item name> x N
            <CR>
            Command?
            */

            /*
            OutputLastReplies();
            */
            bool parseDirections = false;
            bool parseItems = false;
            ResetValidMoves();
            sItemsToPickup = null;
            List<string> itemsToCollect = new List<string>(10);
            string roomName = null;
            string roomDescription = null;

            bool foundRoomLine = false;
            bool foundDescriptionLine = false;
            bool foundDoorsLine = false;
            bool foundItemsLine = false;

            foreach (var line in sLastReplies)
            {
                if (line.Length == 0)
                {
                    continue;
                }
                if (line == "You can't go that way.")
                {
                    OutputLastReplies();
                    return false;
                }
                if (!foundRoomLine)
                {
                    if (!line.StartsWith("== "))
                    {
                        OutputLastReplies();
                        throw new InvalidDataException($"Unknown reply format : first line did not start with '== '");
                    }
                    foundRoomLine = true;
                    var temp = line.Split("== ")[1];
                    roomName = temp.Split(" ==")[0];
                    foundDescriptionLine = false;
                }
                else if (!foundDescriptionLine)
                {
                    foundDescriptionLine = true;
                    roomDescription = line;
                    foundDoorsLine = false;
                    parseDirections = false;
                }
                else if (!foundDoorsLine)
                {
                    if (line == "Doors here lead:")
                    {
                        foundDoorsLine = true;
                        parseDirections = true;
                        ResetValidMoves();
                        foundItemsLine = false;
                    }
                    else
                    {
                        OutputLastReplies();
                        throw new InvalidDataException($"Failed to parse reply - did not find doors line");
                    }
                }
                else if (parseDirections)
                {
                    if (line.StartsWith("- "))
                    {
                        var direction = line.Split("- ")[1];
                        if (direction == "north")
                        {
                            sValidMoves[North] = true;
                        }
                        else if (direction == "south")
                        {
                            sValidMoves[South] = true;
                        }
                        else if (direction == "west")
                        {
                            sValidMoves[West] = true;
                        }
                        else if (direction == "east")
                        {
                            sValidMoves[East] = true;
                        }
                    }
                    else
                    {
                        parseDirections = false;
                        foundItemsLine = false;
                    }
                }
                if (!foundItemsLine)
                {
                    if (line == "Items here:")
                    {
                        foundItemsLine = true;
                        parseItems = true;
                        itemsToCollect.Clear();
                    }
                }
                else if (parseItems)
                {
                    if (line.StartsWith("- "))
                    {
                        var item = line.Split("- ")[1];
                        itemsToCollect.Add(item);
                    }
                    else if (line != "Command?")
                    {
                        OutputLastReplies();
                        throw new InvalidDataException($"Failed to parse line '{line}'");
                    }
                }
                if (line == "Command?")
                {
                    break;
                }
            }

            if (string.IsNullOrEmpty(roomName))
            {
                OutputLastReplies();
                throw new InvalidDataException("Failed to parse Room Name");
            }

            if (string.IsNullOrEmpty(roomDescription))
            {
                OutputLastReplies();
                throw new InvalidDataException("Failed to parse Room Description");
            }

            sItemsToPickup = itemsToCollect.ToArray();

            Console.WriteLine($"");
            Console.WriteLine($"Room: '{roomName}' '{roomDescription}'");
            Console.Write($"Exits:");
            for (var i = 0; i < 4; ++i)
            {
                if (sValidMoves[i])
                {
                    Console.Write($" {sDirections[i]}");
                }
            }
            Console.WriteLine($"");

            if (sItemsToPickup != null)
            {
                Console.Write($"Items:");
                foreach (var item in sItemsToPickup)
                {
                    Console.Write($" '{item}'");
                }
            }
            Console.WriteLine($"");
            Console.WriteLine($"");
            return true;
        }

        bool EnterCommand(string command)
        {
            List<long> inputs = new List<long>(command.Length + 100);
            foreach (var c in command)
            {
                inputs.Add((long)c);
            }
            inputs.Add(10);
            sComputer.SetInputData(inputs.ToArray());
            Console.WriteLine($"Command:{command}");
            return WaitToEnterCommand();
        }

        public static void Run()
        {
            Console.WriteLine("Day25 : Start");
            _ = new Program("Day25/input.txt", true);
            //_ = new Program("Day25/input.txt", false);
            Console.WriteLine("Day25 : End");
        }
    }
}
