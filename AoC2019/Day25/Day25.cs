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
        static readonly int MAX_MAP_SIZE = 2048;
        static readonly int[,] sMap = new int[MAX_MAP_SIZE, MAX_MAP_SIZE];
        static readonly bool[,] sMapExplored = new bool[MAX_MAP_SIZE, MAX_MAP_SIZE];
        static int sRobotX;
        static int sRobotY;
        string sRoomName;
        string sRoomDescription;

        static readonly int North = 0;
        static readonly int South = 1;
        static readonly int West = 2;
        static readonly int East = 3;
        static readonly string[] sDirections = { "north", "south", "west", "east" };

        enum Direction
        {
            North = 1,
            South = 2,
            West = 3,
            East = 4
        };

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
                OutputStatus();
                int posX = sRobotX;
                int posY = sRobotY;
                bool halt = false;
                while (!halt)
                {
                    Search(Direction.North, posX, posY);
                    Search(Direction.South, posX, posY);
                    Search(Direction.West, posX, posY);
                    Search(Direction.East, posX, posY);
                    halt = true;
                }
                OutputMap();
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
            for (var y = 0; y < MAX_MAP_SIZE; ++y)
            {
                for (var x = 0; x < MAX_MAP_SIZE; ++x)
                {
                    sMap[x, y] = -1;
                }
            }
            sRobotX = MAX_MAP_SIZE / 2;
            sRobotY = MAX_MAP_SIZE / 2;
        }

        void OutputStatus()
        {
            Console.WriteLine($"");
            Console.WriteLine($"Robot {sRobotX}, {sRobotY}");
            Console.WriteLine($"Room: '{sRoomName}' '{sRoomDescription}'");
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
        }

        void OutputMap()
        {
            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;
            for (var y = 0; y < MAX_MAP_SIZE; ++y)
            {
                for (var x = 0; x < MAX_MAP_SIZE; ++x)
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
                    else if (sMap[x, y] == 2)
                    {
                        c = 'S';
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
            Console.WriteLine($"North from {sRoomName}");
            if (sRoomName == "Engineering")
            {
                return;
            }
            if (EnterCommand("north"))
            {
                sRobotY -= 1;
                UpdateMap();
            }
        }

        void MoveSouth()
        {
            Console.WriteLine($"South from {sRoomName}");
            if (EnterCommand("south"))
            {
                sRobotY += 1;
                UpdateMap();
            }
        }

        void MoveWest()
        {
            Console.WriteLine($"West from {sRoomName}");
            if (EnterCommand("west"))
            {
                sRobotX -= 1;
                UpdateMap();
            }
        }

        void MoveEast()
        {
            Console.WriteLine($"East from {sRoomName}");
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

        void UpdateMapCell(int x, int y, int value)
        {
            sMap[x, y] = value;
        }

        void UpdateMap()
        {
            if ((sRobotX <= 0) || (sRobotX >= MAX_MAP_SIZE))
            {
                throw new InvalidDataException($"Invalid RobotX {sRobotX} {0} - {MAX_MAP_SIZE}");
            }

            if ((sRobotY <= 0) || (sRobotY >= MAX_MAP_SIZE))
            {
                throw new InvalidDataException($"Invalid RobotY {sRobotY} {0} - {MAX_MAP_SIZE}");
            }

            if (sValidMoves[North])
            {
                UpdateMapCell(sRobotX, sRobotY - 1, 0);
            }
            else
            {
                UpdateMapCell(sRobotX, sRobotY - 1, 1);
            }
            if (sValidMoves[South])
            {
                sMap[sRobotX, sRobotY + 1] = 0;
            }
            else
            {
                sMap[sRobotX, sRobotY + 1] = 1;
            }
            if (sValidMoves[West])
            {
                sMap[sRobotX - 1, sRobotY] = 0;
            }
            else
            {
                sMap[sRobotX - 1, sRobotY] = 1;
            }
            if (sValidMoves[East])
            {
                sMap[sRobotX + 1, sRobotY] = 0;
            }
            else
            {
                sMap[sRobotX + 1, sRobotY] = 1;
            }
            if (sRoomName == "Security Checkpoint")
            {
                sMap[sRobotX, sRobotY] = 2;
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
            if (sLastReplies.Length == 0)
            {
                return true;
            }
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
            sItemsToPickup = null;
            List<string> itemsToCollect = new List<string>(10);
            //sRoomName = null;
            //sRoomDescription = null;

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
                if (line == "A loud, robotic voice says \"Alert! Droids on this ship are heavier than the detected value!\" and you are ejected back to the checkpoint.")
                {
                    OutputLastReplies();
                    return false;
                }
                if (line == "A loud, robotic voice says \"Alert! Droids on this ship are lighter than the detected value!\" and you are ejected back to the checkpoint.")
                {
                    OutputLastReplies();
                    return false;
                }
                if (line.EndsWith("and you are ejected back to the checkpoint."))
                {
                    OutputLastReplies();
                    return false;
                }
                if (line.StartsWith("You take the "))
                {
                    OutputLastReplies();
                    return true;
                }
                if (line.StartsWith("Items in your inventory:"))
                {
                    OutputLastReplies();
                    return true;
                }
                if (line == "You aren't carrying any items.")
                {
                    OutputLastReplies();
                    return true;
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
                    sRoomName = temp.Split(" ==")[0];
                    foundDescriptionLine = false;
                }
                else if (!foundDescriptionLine)
                {
                    foundDescriptionLine = true;
                    sRoomDescription = line;
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

            if (string.IsNullOrEmpty(sRoomName))
            {
                OutputLastReplies();
                throw new InvalidDataException("Failed to parse Room Name");
            }

            if (string.IsNullOrEmpty(sRoomDescription))
            {
                OutputLastReplies();
                throw new InvalidDataException("Failed to parse Room Description");
            }

            sItemsToPickup = itemsToCollect.ToArray();
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
            //Console.WriteLine($"Command:{command}");
            return WaitToEnterCommand();
        }

        void PickUpAllItems()
        {
            if (sItemsToPickup == null)
            {
                return;
            }
            foreach (var item in sItemsToPickup)
            {
                if (item == "molten lava")
                {
                    continue;
                }
                if (item == "infinite loop")
                {
                    continue;
                }
                if (item == "giant electromagnet")
                {
                    continue;
                }
                //food ration : lighter
                if (item == "food ration")
                {
                    EnterCommand($"take {item}");
                    continue;
                }
                //cake : heavier
                if (item == "cake")
                {
                    //EnterCommand($"take {item}");
                    continue;
                }
                //astrolabe : lighter
                if (item == "astrolabe")
                {
                    //EnterCommand($"take {item}");
                    continue;
                }
                //coin : heavier
                if (item == "coin")
                {
                    //EnterCommand($"take {item}");
                    continue;
                }
                // "food ration" : lighter
                // astrolabe : lighter
                // cake : heavier
                // coin : heavier

                // coin + astrolabe : lighter
                // coin + astrolabe + cake : lighter
                // coin + astrolabe + cake + "food ration" : lighter
                //EnterCommand($"take {item}");
            }
            Inventory();
        }

        bool MoveRobot(Direction direction)
        {
            int currentX = sRobotX;
            int currentY = sRobotY;
            switch (direction)
            {
                case Direction.North:
                    MoveNorth();
                    break;
                case Direction.South:
                    MoveSouth();
                    break;
                case Direction.West:
                    MoveWest();
                    break;
                case Direction.East:
                    MoveEast();
                    break;
            }
            PickUpAllItems();
            OutputMap();
            OutputLastReplies();
            OutputStatus();
            return ((currentX != sRobotX) || (currentY != sRobotY));
        }

        bool Search(Direction direction, int currentX, int currentY)
        {
            switch (direction)
            {
                case Direction.North:
                    if (!sValidMoves[North])
                    {
                        return false;
                    }
                    break;
                case Direction.South:
                    if (!sValidMoves[South])
                    {
                        return false;
                    }
                    break;
                case Direction.West:
                    if (!sValidMoves[West])
                    {
                        return false;
                    }
                    break;
                case Direction.East:
                    if (!sValidMoves[East])
                    {
                        return false;
                    }
                    break;
            }
            (int newX, int newY) = GetNewPos(direction, currentX, currentY);
            bool moved = MoveRobot(direction);
            if (!moved)
            {
                return false;
            }

            // Valid move and already visited
            if (sMapExplored[newX, newY])
            {
                MoveBack(direction);
                return false;
            }
            // Valid moved and unknown map location
            sMapExplored[newX, newY] = true;

            if (Search(Direction.North, newX, newY))
            {
                return true;
            }
            if (Search(Direction.South, newX, newY))
            {
                return true;
            }
            if (Search(Direction.West, newX, newY))
            {
                return true;
            }
            if (Search(Direction.East, newX, newY))
            {
                return true;
            }

            MoveBack(direction);
            return false;
        }

        void MoveBack(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    MoveRobot(Direction.South);
                    break;
                case Direction.South:
                    MoveRobot(Direction.North);
                    break;
                case Direction.West:
                    MoveRobot(Direction.East);
                    break;
                case Direction.East:
                    MoveRobot(Direction.West);
                    break;
                default:
                    throw new InvalidDataException($"Invalid direction {direction}");
            }
        }

        (int, int) GetNewPos(Direction direction, int posX, int posY)
        {
            return direction switch
            {
                Direction.North => (posX, posY - 1),
                Direction.South => (posX, posY + 1),
                Direction.West => (posX - 1, posY),
                Direction.East => (posX + 1, posY),
                _ => throw new InvalidDataException($"Invalid input {direction}"),
            };
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
