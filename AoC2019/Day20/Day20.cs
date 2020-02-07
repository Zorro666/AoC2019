using System;
using System.Collections.Generic;
using System.IO;

/*

--- Day 20: Donut Maze ---

You notice a strange pattern on the surface of Pluto and land nearby to get a closer look. Upon closer inspection, you realize you've come across one of the famous space-warping mazes of the long-lost Pluto civilization!

Because there isn't much space on Pluto, the civilization that used to live here thrived by inventing a method for folding spacetime. Although the technology is no longer understood, mazes like this one provide a small glimpse into the daily life of an ancient Pluto citizen.

This maze is shaped like a donut. Portals along the inner and outer edge of the donut can instantly teleport you from one side to the other. For example:

         A           
         A           
  #######.#########  
  #######.........#  
  #######.#######.#  
  #######.#######.#  
  #######.#######.#  
  #####  B    ###.#  
BC...##  C    ###.#  
  ##.##       ###.#  
  ##...DE  F  ###.#  
  #####    G  ###.#  
  #########.#####.#  
DE..#######...###.#  
  #.#########.###.#  
FG..#########.....#  
  ###########.#####  
             Z       
             Z       
This map of the maze shows solid walls (#) and open passages (.). Every maze on Pluto has a start (the open tile next to AA) and an end (the open tile next to ZZ). Mazes on Pluto also have portals; this maze has three pairs of portals: BC, DE, and FG. When on an open tile next to one of these labels, a single step can take you to the other tile with the same label. (You can only walk on . tiles; labels and empty space are not traversable.)

One path through the maze doesn't require any portals. Starting at AA, you could go down 1, right 8, down 12, left 4, and down 1 to reach ZZ, a total of 26 steps.

However, there is a shorter path: You could walk from AA to the inner BC portal (4 steps), warp to the outer BC portal (1 step), walk to the inner DE (6 steps), warp to the outer DE (1 step), walk to the outer FG (4 steps), warp to the inner FG (1 step), and finally walk to ZZ (6 steps). In total, this is only 23 steps.

Here is a larger example:

                   A               
                   A               
  #################.#############  
  #.#...#...................#.#.#  
  #.#.#.###.###.###.#########.#.#  
  #.#.#.......#...#.....#.#.#...#  
  #.#########.###.#####.#.#.###.#  
  #.............#.#.....#.......#  
  ###.###########.###.#####.#.#.#  
  #.....#        A   C    #.#.#.#  
  #######        S   P    #####.#  
  #.#...#                 #......VT
  #.#.#.#                 #.#####  
  #...#.#               YN....#.#  
  #.###.#                 #####.#  
DI....#.#                 #.....#  
  #####.#                 #.###.#  
ZZ......#               QG....#..AS
  ###.###                 #######  
JO..#.#.#                 #.....#  
  #.#.#.#                 ###.#.#  
  #...#..DI             BU....#..LF
  #####.#                 #.#####  
YN......#               VT..#....QG
  #.###.#                 #.###.#  
  #.#...#                 #.....#  
  ###.###    J L     J    #.#.###  
  #.....#    O F     P    #.#...#  
  #.###.#####.#.#####.#####.###.#  
  #...#.#.#...#.....#.....#.#...#  
  #.#####.###.###.#.#.#########.#  
  #...#.#.....#...#.#.#.#.....#.#  
  #.###.#####.###.###.#.#.#######  
  #.#.........#...#.............#  
  #########.###.###.#############  
           B   J   C               
           U   P   P               
Here, AA has no direct path to ZZ, but it does connect to AS and CP. By passing through AS, QG, BU, and JO, you can reach ZZ in 58 steps.

In your maze, how many steps does it take to get from the open tile marked AA to the open tile marked ZZ?

*/

namespace Day20
{
    struct Node
    {
        public int x;
        public int y;
        public int type;
    };

    class Program
    {
        static readonly int PORTAL_START = 10;
        static int[,] sMap;
        static (int w, int h) sMapSize;
        static int sStartNode;
        static int sExitNode;
        static Node[] sNodes;
        static List<int>[] sLinks;

        private Program(string inputFile, bool part1)
        {
            var mapSource = ReadProgram(inputFile);
            if (part1)
            {
                ParseMap(mapSource, false);
                OutputMap(false);
            }
        }

        private string[] ReadProgram(string inputFile)
        {
            var mapSource = File.ReadAllLines(inputFile);
            return mapSource;
        }

        public static void ParseMap(string[] mapSource, bool applyPart2Setup)
        {
            if (mapSource.Length == 0)
            {
                throw new ArgumentException($"map input is empty", nameof(mapSource));
            }
            sMapSize.w = mapSource[0].Length;
            sMapSize.h = 0;
            foreach (var line in mapSource)
            {
                if (line.Length != sMapSize.w)
                {
                    throw new ArgumentException($"map input lines must be the same length {sMapSize.w} {line.Length}", nameof(mapSource));
                }
                sMapSize.h++;
            }
            sMap = new int[sMapSize.w, sMapSize.h];
            sNodes = new Node[sMapSize.w * sMapSize.h];
            sLinks = new List<int>[sMapSize.w * sMapSize.h];
            sStartNode = -1;
            sExitNode = -1;
            var portalCells = new List<(int id, int x, int y)>(100);
            for (int y = 0; y < sMapSize.h; ++y)
            {
                var line = mapSource[y];
                for (int x = 0; x < sMapSize.w; ++x)
                {
                    var nodeIndex = GetNodeIndex(x, y);
                    char cell = line[x];
                    if (cell == ' ')
                    {
                        sMap[x, y] = -1;
                    }
                    else if (cell == '.')
                    {
                        sMap[x, y] = 0;
                    }
                    else if (cell == '#')
                    {
                        sMap[x, y] = 1;
                    }
                    else if ((cell >= 'A') && (cell <= 'Z'))
                    {
                        int portalId = PORTAL_START + (cell - 'A');
                        sMap[x, y] = portalId;
                        portalCells.Add((portalId, x, y));
                    }
                    else
                    {
                        throw new InvalidDataException($"Unknown input map[{x},{y}] {cell}");
                    }
                    sNodes[nodeIndex].x = x;
                    sNodes[nodeIndex].y = y;
                    sNodes[nodeIndex].type = sMap[x, y];
                }
            }

            // Parse the portals and connect them together
            // Find the other portal letter, entry/exit point, is the map start or end
            var portals = new List<(int id0, int id1, int entryExitX, int entryExitY)>(50);
            foreach (var portal in portalCells)
            {
                // ? 
                //?P?
                // ? 
                int pId = portal.id;
                int pX = portal.x;
                int pY = portal.y;
                int countValidPortalId = 0;
                (int id, int x, int y) validPortal = (-1, -1, -1);

                var tryTargets = new (int x, int y)[] { (pX, pY - 1), (pX - 1, pY), (pX + 1, pY), (pX, pY + 1) };

                foreach ((int tryX, int tryY) in tryTargets)
                {
                    int targetId = GetMapCell(tryX, tryY);
                    if (targetId >= PORTAL_START)
                    {
                        ++countValidPortalId;
                        validPortal.id = targetId;
                        validPortal.x = tryX;
                        validPortal.y = tryY;
                    }
                }
                if (countValidPortalId != 1)
                {
                    throw new InvalidDataException($"Invalid portal at {(char)(pId - PORTAL_START + 'A')} {pX},{pY} wrong number of portalLetters {countValidPortalId}");
                }

                // Find the entry/exit point for this portal instance
                var startPoints = new (int x, int y)[] { (pX, pY), (validPortal.x, validPortal.y) };
                int countValidEntryExitPoints = 0;
                (int x, int y) entryExit = (-1, -1);
                foreach ((int sX, int sY) in startPoints)
                {
                    tryTargets = new (int x, int y)[] { (sX, sY - 1), (sX - 1, sY), (sX + 1, sY), (sX, sY + 1) };
                    foreach ((int tryX, int tryY) in tryTargets)
                    {
                        int targetId = GetMapCell(tryX, tryY);
                        if (targetId == 0)
                        {
                            ++countValidEntryExitPoints;
                            entryExit.x = tryX;
                            entryExit.y = tryY;
                        }
                    }
                }
                if (countValidEntryExitPoints != 1)
                {
                    throw new InvalidDataException($"Invalid portal at {(char)(pId - PORTAL_START + 'A')} {pX},{pY} wrong number of entry exit points {countValidEntryExitPoints}");
                }
                portals.Add((pId, validPortal.id, entryExit.x, entryExit.y));
            }
            // Find the start and end portal instances
            foreach (var portal in portals)
            {
                if (portal.id0 == portal.id1)
                {
                    if (portal.id0 == PORTAL_START + 'A' - 'A')
                    {
                        sStartNode = GetNodeIndex(portal.entryExitX, portal.entryExitY);
                    }
                    else if (portal.id0 == PORTAL_START + 'Z' - 'A')
                    {
                        sExitNode = GetNodeIndex(portal.entryExitX, portal.entryExitY);
                    }
                    else
                    {
                        throw new InvalidDataException($"Invalid portal type {(char)(portal.id0 - PORTAL_START + 'A')}, {(char)(portal.id1 - PORTAL_START + 'A')} at {portal.entryExitX},{portal.entryExitY}");
                    }
                }
            }
            // Find the connected portal to make : PortalId: enterIndex exitIndex

            if (sStartNode == -1)
            {
                throw new InvalidDataException($"Failed to find the start point {sStartNode}");
            }

            if (sExitNode == -1)
            {
                throw new InvalidDataException($"Failed to find the exit point {sExitNode}");
            }

            for (int y = 0; y < sMapSize.h; ++y)
            {
                for (int x = 0; x < sMapSize.w; ++x)
                {
                    var nodeIndex = x + y * sMapSize.w;
                    int cell = sMap[x, y];
                    sLinks[nodeIndex] = new List<int>();
                    sLinks[nodeIndex].Clear();
                    if (cell == -1)
                    {
                        continue;
                    }
                    if (cell == 1)
                    {
                        continue;
                    }
                    if ((x - 1 >= 0) && (sMap[x - 1, y] != 1))
                    {
                        var targetNodeIndex = x - 1 + y * sMapSize.w;
                        sLinks[nodeIndex].Add(targetNodeIndex);
                    }
                    if ((x + 1 < sMapSize.w) && (sMap[x + 1, y] != 1))
                    {
                        var targetNodeIndex = x + 1 + y * sMapSize.w;
                        sLinks[nodeIndex].Add(targetNodeIndex);
                    }
                    if ((y - 1 >= 0) && (sMap[x, y - 1] != 1))
                    {
                        var targetNodeIndex = x + (y - 1) * sMapSize.w;
                        sLinks[nodeIndex].Add(targetNodeIndex);
                    }
                    if ((y + 1 < sMapSize.h) && (sMap[x, y + 1] != 1))
                    {
                        var targetNodeIndex = x + (y + 1) * sMapSize.w;
                        sLinks[nodeIndex].Add(targetNodeIndex);
                    }
                }
            }
        }

        public static void OutputMap(bool detailed)
        {
            for (int y = 0; y < sMapSize.h; ++y)
            {
                string line = "";
                for (int x = 0; x < sMapSize.w; ++x)
                {
                    int cell = sMap[x, y];
                    if (cell == -1)
                    {
                        line += ' ';
                    }
                    else if (cell == 0)
                    {
                        line += '.';
                    }
                    else if (cell == 1)
                    {
                        line += '#';
                    }
                    else if (cell >= PORTAL_START)
                    {
                        line += (char)('A' + (cell - PORTAL_START));
                    }
                    else
                    {
                        throw new InvalidDataException($"Unknown map[{x},{y}] {cell}");
                    }
                }
                Console.WriteLine($"{line}");
            }

            if (detailed)
            {
                for (int nodeIndex = 0; nodeIndex < sNodes.Length; ++nodeIndex)
                {
                    var node = sNodes[nodeIndex];
                    Console.WriteLine($"Node:{node.x},{node.y} {node.type}");
                    foreach (var linkTargetIndex in sLinks[nodeIndex])
                    {
                        (int linkX, int linkY) = GetXYFromNodeIndex(linkTargetIndex);
                        Console.WriteLine($"Link:{linkTargetIndex} {linkX},{linkY}");
                    }
                }
            }
        }

        static int GetNodeIndex(int x, int y)
        {
            if ((x < 0) || (x >= sMapSize.w))
            {
                throw new ArgumentOutOfRangeException("x", $"Invalid value {x} out of range 0-{sMapSize.w}");
            }
            if ((y < 0) || (y >= sMapSize.h))
            {
                throw new ArgumentOutOfRangeException("y", $"Invalid value {y} out of range 0-{sMapSize.h}");
            }
            return x + y * sMapSize.w;
        }

        static (int, int) GetXYFromNodeIndex(int nodeIndex)
        {
            int x = nodeIndex % sMapSize.w;
            int y = nodeIndex / sMapSize.w;
            return (x, y);
        }

        static int GetMapCell(int x, int y)
        {
            if ((x >= 0) && (x < sMapSize.w) && (y >= 0) && (y < sMapSize.h))
            {
                return sMap[x, y];
            }
            return -666;
        }

        public static int ShortestPath()
        {
            return ShortestPath(sStartNode, sExitNode);
        }

        static int ShortestPath(int startIndex, int endIndex)
        {
            //(int fromX, int fromY) = GetXYFromNodeIndex(startIndex);
            //Console.WriteLine($"ShortestPath Start {startIndex} {fromX},{fromY}");
            Queue<int> nodesToVisit = new Queue<int>();
            nodesToVisit.Enqueue(startIndex);
            bool[] visited = new bool[sNodes.Length];
            int[] parents = new int[sNodes.Length];
            int minNumSteps = int.MaxValue;
            for (int i = 0; i < parents.Length; ++i)
            {
                parents[i] = -1;
            }
            while (nodesToVisit.Count > 0)
            {
                int nodeIndex = nodesToVisit.Dequeue();
                (int x, int y) = GetXYFromNodeIndex(nodeIndex);
                //Console.WriteLine($"Node:{nodeIndex} {x},{y}");
                if (nodeIndex == endIndex)
                {
                    //Console.WriteLine($"Solved");
                    int numSteps = 0;
                    int currentNodeIndex = endIndex;
                    while (currentNodeIndex != -1)
                    {
                        (x, y) = GetXYFromNodeIndex(currentNodeIndex);
                        //Console.WriteLine($"Node:{currentNodeIndex} {x},{y}");
                        numSteps++;
                        currentNodeIndex = parents[currentNodeIndex];
                        if (currentNodeIndex == startIndex)
                        {
                            break;
                        }
                    }
                    //Console.WriteLine($"numSteps:{numSteps}");
                    if (numSteps < minNumSteps)
                    {
                        minNumSteps = numSteps;
                    }
                }

                foreach (var link in sLinks[nodeIndex])
                {
                    if (visited[link] == false)
                    {
                        int linkType = sNodes[link].type;
                        bool validLink = true;
                        if (link == endIndex)
                        {
                            validLink = true;
                        }
                        if (validLink)
                        {
                            visited[link] = true;
                            nodesToVisit.Enqueue(link);
                            parents[link] = nodeIndex;
                        }
                    }
                }
            };

            if (minNumSteps < int.MaxValue)
            {
                return minNumSteps;
            }
            return -1;
        }

        public static void Run()
        {
            Console.WriteLine("Day20 : Start");
            _ = new Program("Day20/input.txt", true);
            _ = new Program("Day20/input.txt", false);
            Console.WriteLine("Day20 : End");
        }
    }
}
