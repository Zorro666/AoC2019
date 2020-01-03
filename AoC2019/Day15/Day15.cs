using System;
using System.Collections.Generic;
using System.IO;

/*
--- Day 15: Oxygen System ---

Out here in deep space, many things can go wrong.Fortunately, many of those things have indicator lights.Unfortunately, one of those lights is lit: the oxygen system for part of the ship has failed!

According to the readouts, the oxygen system must have failed days ago after a rupture in oxygen tank two; that section of the ship was automatically sealed once oxygen levels went dangerously low.A single remotely-operated repair droid is your only option for fixing the oxygen system.

The Elves' care package included an Intcode program (your puzzle input) that you can use to remotely control the repair droid. By running that program, you can direct the repair droid to the oxygen system and fix the problem.

The remote control program executes the following steps in a loop forever:

Accept a movement command via an input instruction.
Send the movement command to the repair droid.
Wait for the repair droid to finish the movement operation.
Report on the status of the repair droid via an output instruction.
Only four movement commands are understood: north (1), south(2), west(3), and east(4). Any other command is invalid.The movements differ in direction, but not in distance: in a long enough east-west hallway, a series of commands like 4,4,4,4,3,3,3,3 would leave the repair droid back where it started.

The repair droid can reply with any of the following status codes:

0: The repair droid hit a wall.Its position has not changed.
1: The repair droid has moved one step in the requested direction.
2: The repair droid has moved one step in the requested direction; its new position is the location of the oxygen system.
You don't know anything about the area around the repair droid, but you can figure it out by watching the status codes.

For example, we can draw the area using D for the droid, # for walls, . for locations the droid can traverse, and empty space for unexplored locations. Then, the initial state looks like this:

      
      
   D



To make the droid go north, send it 1. If it replies with 0, you know that location is a wall and that the droid didn't move:

      
   #  
   D


To move east, send 4; a reply of 1 means the movement was successful:

      
   #  
   .D


Then, perhaps attempts to move north(1), south(2), and east(4) are all met with replies of 0:

      
   ## 
   .D#
    # 
      
Now, you know the repair droid is in a dead end.Backtrack with 3 (which you already know will get a reply of 1 because you already know that location is open):

      
   ## 
   D.#
    # 
      
Then, perhaps west(3) gets a reply of 0, south(2) gets a reply of 1, south again(2) gets a reply of 0, and then west(3) gets a reply of 2:

      
   ## 
  #..#
  D.# 
   #  
Now, because of the reply of 2, you know you've found the oxygen system! In this example, it was only 2 moves away from the repair droid's starting position.

What is the fewest number of movement commands required to move the repair droid from its starting position to the location of the oxygen system?

--- Part Two ---

You quickly repair the oxygen system; oxygen gradually fills the area.

Oxygen starts in the location containing the repaired oxygen system. It takes one minute for oxygen to spread to all open locations that are adjacent to a location that already contains oxygen. Diagonal locations are not adjacent.

In the example above, suppose you've used the droid to explore the area fully and have the following map (where locations that currently contain oxygen are marked O):

 ##   
#..## 
#.#..#
#.O.# 
 ###  
Initially, the only location which contains oxygen is the location of the repaired oxygen system. However, after one minute, the oxygen spreads to all open (.) locations that are adjacent to a location containing oxygen:

 ##   
#..## 
#.#..#
#OOO# 
 ###  
After a total of two minutes, the map looks like this:

 ##   
#..## 
#O#O.#
#OOO# 
 ###  
After a total of three minutes:

 ##   
#O.## 
#O#OO#
#OOO# 
 ###  
And finally, the whole region is full of oxygen after a total of four minutes:

 ##   
#OO## 
#O#OO#
#OOO# 
 ###  
So, in this example, all locations contain oxygen after 4 minutes.

Use the repair droid to get a complete map of the area. How many minutes will it take to fill with oxygen?

*/

namespace Day15
{
    class Program
    {
        static readonly int MAX_MAP_SIZE = 1024;
        static readonly int sStartX = MAX_MAP_SIZE / 2;
        static readonly int sStartY = MAX_MAP_SIZE / 2;
        static StatusCodes[,] sMap = new StatusCodes[MAX_MAP_SIZE, MAX_MAP_SIZE];
        static readonly bool[,] sMapExplored = new bool[MAX_MAP_SIZE, MAX_MAP_SIZE];
        static int sDistance;
        static int sNumberOfSteps;
        static int numberOfMinutes;
        static IntProgram sIntProgram = new IntProgram();
        enum Direction
        {
            North = 1,
            South = 2,
            West = 3,
            East = 4
        };
        enum StatusCodes
        {
            Unknown = 0,
            EmptyRoom = 1,
            Oxygen = 2,
            Wall = 3
        };

        private Program(string inputFile, bool part1)
        {
            sIntProgram.LoadProgram(inputFile);
            var result = RunProgram();

            if (part1)
            {
                Console.WriteLine($"Day15 : Result1 {result}");
                if (result != 250)
                {
                    throw new InvalidProgramException($"Part1 result has been broken {result}");
                }
            }
            else
            {
                var numberOfMinutesToFillWithOxygen = FillWithOxygen();
                Console.WriteLine($"Day15 : Result2 {numberOfMinutesToFillWithOxygen}");
            }
        }

        static public int RunProgram()
        {
            sMap[sStartX, sStartY] = 0;
            int posX = sStartX;
            int posY = sStartY;
            bool halt = false;
            while (!halt)
            {
                Search(Direction.North, posX, posY);
                Search(Direction.South, posX, posY);
                Search(Direction.West, posX, posY);
                Search(Direction.East, posX, posY);
                halt = true;
            }
            OutputMap(posX, posY);
            return sDistance;
        }

        static bool MoveRobot(Direction direction, int currentX, int currentY)
        {
            (int newX, int newY) = GetNewPos(direction, currentX, currentY);
            long input = (long)direction;
            long output = GetOutput(input);
            bool moved = true;
            switch (output)
            {
                case 1:
                    sMap[newX, newY] = StatusCodes.EmptyRoom;
                    break;
                case 2:
                    sMap[newX, newY] = StatusCodes.Oxygen;
                    break;
                case 0:
                    sMap[newX, newY] = StatusCodes.Wall;
                    newX = currentX;
                    newY = currentY;
                    moved = false;
                    break;
                default:
                    throw new InvalidDataException($"Invalid output {output}");
            }

            OutputMap(newX, newY);
            return moved;
        }

        static bool Search(Direction direction, int currentX, int currentY)
        {
            (int newX, int newY) = GetNewPos(direction, currentX, currentY);
            bool moved = MoveRobot(direction, currentX, currentY);
            if (!moved)
            {
                return false;
            }

            ++sNumberOfSteps;
            if (sMap[newX, newY] == StatusCodes.Oxygen)
            {
                sDistance = sNumberOfSteps;
            }

            // Valid move and already visited
            if (sMapExplored[newX, newY])
            {
                MoveBack(direction, newX, newY);
                --sNumberOfSteps;
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

            --sNumberOfSteps;
            MoveBack(direction, newX, newY);
            return false;
        }

        static void MoveBack(Direction direction, int currentX, int currentY)
        {
            switch (direction)
            {
                case Direction.North:
                    MoveRobot(Direction.South, currentX, currentY);
                    break;
                case Direction.South:
                    MoveRobot(Direction.North, currentX, currentY);
                    break;
                case Direction.West:
                    MoveRobot(Direction.East, currentX, currentY);
                    break;
                case Direction.East:
                    MoveRobot(Direction.West, currentX, currentY);
                    break;
                default:
                    throw new InvalidDataException($"Invalid direction {direction}");
            }
        }

        static (int, int) GetNewPos(Direction direction, int posX, int posY)
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

        static long GetOutput(Int64 input)
        {
            bool halt = false;
            bool hasOutput = false;
            sIntProgram.SetNextInput(input);
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

        static void OutputMap(int posX, int posY)
        {
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;
            for (int y = 0; y < MAX_MAP_SIZE; ++y)
            {
                for (int x = 0; x < MAX_MAP_SIZE; ++x)
                {
                    if (sMap[x, y] != StatusCodes.Unknown)
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
                    if ((x == sStartX) && (y == sStartY))
                    {
                        line += 'S';
                    }
                    // Unknown
                    else if (sMap[x, y] == StatusCodes.Unknown)
                    {
                        line += ' ';
                    }
                    // Clear
                    else if (sMap[x, y] == StatusCodes.EmptyRoom)
                    {
                        if ((x == posX) && (y == posY))
                        {
                            line += '*';
                        }
                        else
                        {
                            line += '.';
                        }
                    }
                    // Oxygen
                    else if (sMap[x, y] == StatusCodes.Oxygen)
                    {
                        if ((x == posX) && (y == posY))
                        {
                            line += '@';
                        }
                        else
                        {
                            line += 'O';
                        }
                    }
                    // Wall
                    else if (sMap[x, y] == StatusCodes.Wall)
                    {
                        line += '#';
                    }
                    else
                    {
                        throw new InvalidDataException($"Invalid map value {sMap[x, y]}");
                    }
                }
                line += "|";
                Console.WriteLine(line);
            }
            Console.WriteLine(topBottom);
        }

        public static void ParseInput(string[] mapInputRows)
        {
            sMap = new StatusCodes[MAX_MAP_SIZE, MAX_MAP_SIZE];
            numberOfMinutes = 0;
            if (mapInputRows.Length == 0)
                throw new ArgumentException("There must be at least one map input row");

            var mapWidth = mapInputRows[0].Length;
            for (var y = 0; y < mapInputRows.Length; y++)
            {
                var row = mapInputRows[y];
                if (row.Length != mapWidth)
                {
                    throw new ArgumentException("All map input rows must be the same width");
                }
                for (var x = 0; x < mapWidth; x++)
                {
                    var character = row[x];
                    sMap[x, y] = character switch
                    {
                        ' ' => StatusCodes.Unknown,
                        '.' => StatusCodes.EmptyRoom,
                        'O' => StatusCodes.Oxygen,
                        '#' => StatusCodes.Wall,
                        _ => throw new InvalidDataException($"Invalid map input {character}")
                    };
                }
            }
        }

        public static int FillWithOxygen()
        {
            var numberOfEmptyRooms = CountNumberOfEmptyRooms();
            if (numberOfEmptyRooms == 0)
            {
                return numberOfMinutes;
            }

            numberOfMinutes++;

            var newOxygenSpaces = new List<(int intX, int intY)>();

            for (int y = 0; y < MAX_MAP_SIZE; ++y)
            {
                for (int x = 0; x < MAX_MAP_SIZE; ++x)
                {
                    if (sMap[x, y] == StatusCodes.Oxygen)
                    {
                        if (sMap[x, y + 1] == StatusCodes.EmptyRoom)
                            newOxygenSpaces.Add((x, y + 1));

                        if (sMap[x + 1, y] == StatusCodes.EmptyRoom)
                            newOxygenSpaces.Add((x + 1, y));

                        if (sMap[x, y - 1] == StatusCodes.EmptyRoom)
                            newOxygenSpaces.Add((x, y - 1));

                        if (sMap[x - 1, y] == StatusCodes.EmptyRoom)
                            newOxygenSpaces.Add((x - 1, y));
                    }
                }
            }
            foreach (var (x, y) in newOxygenSpaces)
            {
                sMap[x, y] = StatusCodes.Oxygen;
            }

            OutputMap(0, 0);

            return FillWithOxygen();
        }

        private static int CountNumberOfEmptyRooms()
        {
            var count = 0;
            for (int y = 0; y < MAX_MAP_SIZE; ++y)
            {
                for (int x = 0; x < MAX_MAP_SIZE; ++x)
                {
                    if (sMap[x, y] == StatusCodes.EmptyRoom)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public static void Run()
        {
            Console.WriteLine("Day15 : Start");
            _ = new Program("Day15/input.txt", true);
            _ = new Program("Day15/input.txt", false);
            Console.WriteLine("Day15 : End");
        }
    }
}
