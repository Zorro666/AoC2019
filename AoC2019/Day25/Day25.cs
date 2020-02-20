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

Your puzzle answer was 4362.

--- Part Two ---

As you move through the main airlock, the air inside the ship is already heating up to reasonable levels. Santa explains that he didn't notice you coming because he was just taking a quick nap. The ship wasn't frozen; he just had the thermostat set to "North Pole".

You make your way over to the navigation console. It beeps. "Status: Stranded. Please supply measurements from 49 stars to recalibrate."

"49 stars? But the Elves told me you needed fifty--"

Santa just smiles and nods his head toward the window. There, in the distance, you can see the center of the Solar System: the Sun!

The navigation console beeps again.

*/

namespace Day25
{
    class Program
    {
        struct Node
        {
            public int type;
            public string name;
            public string description;
        };

        static IntProgram sComputer;
        static string[] sLastReplies;
        static bool[] sValidMoves;
        static string[] sItemsToPickup;
        static List<string> sInventory = new List<string>(128);
        static readonly int MAX_NUM_ROOMS = 1024;
        static readonly bool[] sMapExplored = new bool[MAX_NUM_ROOMS];
        static int sRobotRoom = -1;
        static int sLastRoom = -1;
        static int sNextFreeRoom = 0;
        static int sSecurityRoomNodeIndex = -1;
        string sRoomName;
        string sRoomDescription;
        static Direction sMoveDirection = Direction.Unknown;
        static readonly Node[] sNodes = new Node[MAX_NUM_ROOMS];
        static readonly List<int>[] sLinks = new List<int>[MAX_NUM_ROOMS];
        static readonly Dictionary<string, int> sRoomNames = new Dictionary<string, int>(MAX_NUM_ROOMS);
        static readonly Dictionary<(int from, int to), Direction> sRoutes = new Dictionary<(int, int), Direction>(MAX_NUM_ROOMS * MAX_NUM_ROOMS);

        static readonly int North = 0;
        static readonly int South = 1;
        static readonly int West = 2;
        static readonly int East = 3;
        static readonly string[] sDirections = { "north", "south", "west", "east" };

        enum Direction
        {
            Unknown = 0,
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
            InitRooms();
            ResetValidMoves();

            if (part1)
            {
                WaitToEnterCommand();
                UpdateMap();
                OutputStatus();
                bool halt = false;
                while (!halt)
                {
                    Search(Direction.North);
                    Navigate(sRobotRoom, 0);
                    Search(Direction.South);
                    Navigate(sRobotRoom, 0);
                    Search(Direction.West);
                    Navigate(sRobotRoom, 0);
                    Search(Direction.East);
                    halt = true;
                }
                Inventory();
                OutputRooms();
                if (sSecurityRoomNodeIndex == -1)
                {
                    throw new InvalidDataException($"Failed to find the security room");
                }
                var startIndex = sRobotRoom;
                var endIndex = sSecurityRoomNodeIndex;
                var routeToSecurity = Navigate(startIndex, endIndex);
                Console.WriteLine($"");
                Console.Write($"Route {startIndex} ");
                Console.WriteLine($" -> {endIndex}");
                var currentRoom = startIndex;
                foreach (var node in routeToSecurity)
                {
                    var direction = sRoutes[(currentRoom, node)];
                    var oldRobotRoom = sRobotRoom;
                    MoveRobot(direction);
                    Console.WriteLine($"{node} {direction} {oldRobotRoom} -> {sRobotRoom}");
                    currentRoom = node;
                }
                var result1 = FindDoorCombination();
                var expected = 4362;
                Console.WriteLine($"");
                Console.WriteLine($"Day25: Part1 {result1}");
                if (result1 != expected)
                {
                    throw new InvalidDataException($"Part1 is broken {result1} != {expected}");
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

        void InitRooms()
        {
            sLastRoom = -1;
            sNextFreeRoom = 0;
            sRobotRoom = 0;
            for (var r = 0; r < sNodes.Length; ++r)
            {
                sNodes[r].type = -1;
            }
        }

        void OutputStatus()
        {
            Console.WriteLine($"");
            Console.WriteLine($"Robot {sRobotRoom}");
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

        void OutputRooms()
        {
            Console.WriteLine("----------------------------------");
            int r = 0;
            foreach (var room in sNodes)
            {
                if (room.type != -1)
                {
                    Console.WriteLine($"Room[{r}] '{room.name}' Desc:'{room.description}' Type:{room.type}");
                }
                ++r;
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
            sMoveDirection = Direction.North;
            Console.WriteLine($"North from {sRoomName}");
            if (EnterCommand("north"))
            {
                UpdateMap();
            }
        }

        void MoveSouth()
        {
            sMoveDirection = Direction.South;
            Console.WriteLine($"South from {sRoomName}");
            if (EnterCommand("south"))
            {
                UpdateMap();
            }
        }

        void MoveWest()
        {
            sMoveDirection = Direction.West;
            Console.WriteLine($"West from {sRoomName}");
            if (EnterCommand("west"))
            {
                UpdateMap();
            }
        }

        void MoveEast()
        {
            sMoveDirection = Direction.East;
            if (sRoomName == "Passages")
            {
                Console.WriteLine($"Not going East from {sRoomName}");
                return;
            }

            Console.WriteLine($"East from {sRoomName}");
            if (EnterCommand("east"))
            {
                UpdateMap();
            }
        }

        void Inventory()
        {
            EnterCommand("inv");
            bool getItems = false;
            foreach (var line in sLastReplies)
            {
                if (line.StartsWith("Items in your inventory:"))
                {
                    getItems = true;
                    sInventory.Clear();
                }
                else if (getItems)
                {
                    if (line.StartsWith("- "))
                    {
                        var item = line.Split("- ")[1];
                        sInventory.Add(item);
                    }
                }
            }
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
            //OutputLastReplies();
            return ParseReply();
        }

        void UpdateMap()
        {
            if (sRoomNames.ContainsKey(sRoomName))
            {
                sRobotRoom = sRoomNames[sRoomName];
            }
            else
            {
                sRobotRoom = sNextFreeRoom;
                sRoomNames[sRoomName] = sRobotRoom;
                ++sNextFreeRoom;
            }

            if (sRobotRoom < 0)
            {
                throw new InvalidDataException($"Invalid RobotRoom {sRobotRoom}");
            }

            var room = sRobotRoom;
            var type = 0;

            if (sRoomName == "Security Checkpoint")
            {
                sSecurityRoomNodeIndex = room;
                type = 2;
            }
            sNodes[room].type = type;
            sNodes[room].name = sRoomName;
            sNodes[room].description = sRoomDescription;

            if (sLinks[room] == null)
            {
                sLinks[room] = new List<int>();
                sLinks[room].Clear();
            }
            if ((sLastRoom != -1) && (sLastRoom != room))
            {
                sLinks[room].Add(sLastRoom);
                sLinks[sLastRoom].Add(room);
            }
            if (sLastRoom != room)
            {
                var route = (sLastRoom, room);
                if (!sRoutes.TryGetValue(route, out Direction existingDirection))
                {
                    sRoutes[(sLastRoom, room)] = sMoveDirection;
                }
                else
                {
                    if (existingDirection != sMoveDirection)
                    {
                        throw new InvalidDataException($"existingDirection {existingDirection} != sMoveDirection {sMoveDirection}");
                    }
                }
                sLastRoom = room;
                sMoveDirection = Direction.Unknown;
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
            List<string> itemsToCollect = new List<string>(10);

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
                    //OutputLastReplies();
                    Console.WriteLine(line);
                    return false;
                }
                if (line == "A loud, robotic voice says \"Alert! Droids on this ship are heavier than the detected value!\" and you are ejected back to the checkpoint.")
                {
                    Console.WriteLine(line);
                    //OutputLastReplies();
                    return false;
                }
                if (line == "A loud, robotic voice says \"Alert! Droids on this ship are lighter than the detected value!\" and you are ejected back to the checkpoint.")
                {
                    Console.WriteLine(line);
                    //OutputLastReplies();
                    return false;
                }
                if (line.EndsWith("and you are ejected back to the checkpoint."))
                {
                    Console.WriteLine(line);
                    //OutputLastReplies();
                    return false;
                }
                if (line.StartsWith("You take the "))
                {
                    Console.WriteLine(line);
                    //OutputLastReplies();
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
                if (line.StartsWith("You drop the "))
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
                else
                {
                    if (line == "Command?")
                    {
                        break;
                    }
                    else
                    {
                        OutputLastReplies();
                        throw new InvalidDataException($"Failed to parse line '{line}'");
                    }
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

            if (foundItemsLine)
            {
                sItemsToPickup = itemsToCollect.ToArray();
            }
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
                Inventory();
                return;
            }
            foreach (var item in sItemsToPickup)
            {
                bool pickup = true;
                if (item == "molten lava")
                {
                    pickup = false;
                }
                else if (item == "infinite loop")
                {
                    pickup = false;
                }
                else if (item == "giant electromagnet")
                {
                    pickup = false;
                }
                else if (item == "escape pod")
                {
                    pickup = false;
                }
                //food ration : lighter
                else if (item == "food ration")
                {
                    pickup = true;
                }
                //cake : heavier
                else if (item == "cake")
                {
                    pickup = true;
                }
                //astrolabe : lighter
                else if (item == "astrolabe")
                {
                    pickup = true;
                }
                //coin : heavier
                else if (item == "coin")
                {
                    pickup = true;
                }
                else if (item == "hologram")
                {
                    pickup = true;
                }

                if (pickup)
                {
                    EnterCommand($"take {item}");
                }
            }
            Inventory();
        }

        bool MoveRobot(Direction direction)
        {
            int currentRoom = sRobotRoom;
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
            OutputLastReplies();
            OutputStatus();
            sItemsToPickup = null;
            return (currentRoom != sRobotRoom);
        }

        bool Search(Direction direction)
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
            bool moved = MoveRobot(direction);
            if (!moved)
            {
                return false;
            }

            // Valid move and already visited
            if (sMapExplored[sRobotRoom])
            {
                MoveBack(direction);
                return false;
            }
            // Valid moved and unknown map location
            sMapExplored[sRobotRoom] = true;

            if (Search(Direction.North))
            {
                return true;
            }
            if (Search(Direction.South))
            {
                return true;
            }
            if (Search(Direction.West))
            {
                return true;
            }
            if (Search(Direction.East))
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

        static List<int> Navigate(int startIndex, int endIndex)
        {
            Queue<int> nodesToVisit = new Queue<int>();
            nodesToVisit.Enqueue(startIndex);
            List<int> visited = new List<int>(sNodes.Length);
            Dictionary<int, int> parents = new Dictionary<int, int>(sNodes.Length);
            while (nodesToVisit.Count > 0)
            {
                var nodeIndex = nodesToVisit.Dequeue();
                if (nodeIndex == endIndex)
                {
                    var route = new List<int>(100);
                    int currentNode = endIndex;
                    bool foundParent = true;
                    while (foundParent)
                    {
                        route.Insert(0, currentNode);
                        foundParent = parents.TryGetValue(currentNode, out var parentNode);
                        if (parentNode == startIndex)
                        {
                            break;
                        }
                        currentNode = parentNode;
                    }
                    return route;
                }

                foreach (var link in sLinks[nodeIndex])
                {
                    if (!visited.Contains(link))
                    {
                        visited.Add(link);
                        nodesToVisit.Enqueue(link);
                        parents[link] = nodeIndex;
                    }
                }
            };
            return null;
        }

        void DropAllItems()
        {
            Inventory();
            foreach (var item in sInventory)
            {
                EnterCommand($"drop {item}");
            }
            Inventory();
        }

        int FindDoorCombination()
        {
            var numItems = sInventory.Count;
            var itemNames = sInventory.ToArray();

            if (sRobotRoom != sSecurityRoomNodeIndex)
            {
                throw new InvalidDataException($"Robot not at security {sRobotRoom} != {sSecurityRoomNodeIndex}");
            }
            var itemCombination = (1 << numItems) - 1;
            while (itemCombination > 0)
            {
                if (sRobotRoom == sSecurityRoomNodeIndex)
                {
                    DropAllItems();
                    for (var i = 0; i < numItems; ++i)
                    {
                        if ((itemCombination & (1 << i)) == (1 << i))
                        {
                            EnterCommand($"take {itemNames[i]}");
                        }
                    }
                    Inventory();
                    MoveSouth();
                }
                if (sRobotRoom != sSecurityRoomNodeIndex)
                {
                    OutputLastReplies();
                    foreach (var line in sLastReplies)
                    {
                        foreach (var word in line.Split(" "))
                        {
                            if (int.TryParse(word, out int keyCode))
                            {
                                return keyCode;
                            }
                        }
                    }
                    throw new InvalidDataException($"Failed to find the passcode");
                }
                --itemCombination;
            }
            throw new InvalidDataException($"Did not get through the security door");
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
