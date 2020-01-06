using System;
using System.Collections.Generic;
using System.IO;

/*

--- Day 18: Many-Worlds Interpretation ---

As you approach Neptune, a planetary security system detects you and activates a giant tractor beam on Triton! You have no choice but to land.

A scan of the local area reveals only one interesting feature: a massive underground vault. You generate a map of the tunnels (your puzzle input). The tunnels are too narrow to move diagonally.

Only one entrance (marked @) is present among the open passages (marked .) and stone walls (#), but you also detect an assortment of keys (shown as lowercase letters) and doors (shown as uppercase letters). Keys of a given letter open the door of the same letter: a opens A, b opens B, and so on. You aren't sure which key you need to disable the tractor beam, so you'll need to collect all of them.

For example, suppose you have the following map:

#########
#b.A.@.a#
#########
Starting from the entrance (@), you can only access a large door (A) and a key (a). Moving toward the door doesn't help you, but you can move 2 steps to collect the key, unlocking A in the process:

#########
#b.....@#
#########
Then, you can move 6 steps to collect the only other key, b:

#########
#@......#
#########
So, collecting every key took a total of 8 steps.

Here is a larger example:

########################
#f.D.E.e.C.b.A.@.a.B.c.#
######################.#
#d.....................#
########################
The only reasonable move is to take key a and unlock door A:

########################
#f.D.E.e.C.b.....@.B.c.#
######################.#
#d.....................#
########################
Then, do the same with key b:

########################
#f.D.E.e.C.@.........c.#
######################.#
#d.....................#
########################
...and the same with key c:

########################
#f.D.E.e.............@.#
######################.#
#d.....................#
########################
Now, you have a choice between keys d and e. While key e is closer, collecting it now would be slower in the long run than collecting key d first, so that's the best choice:

########################
#f...E.e...............#
######################.#
#@.....................#
########################
Finally, collect key e to unlock door E, then collect key f, taking a grand total of 86 steps.

Here are a few more examples:

########################
#...............b.C.D.f#
#.######################
#.....@.a.B.c.d.A.e.F.g#
########################
Shortest path is 132 steps: b, a, c, d, f, e, g

#################
#i.G..c...e..H.p#
########.########
#j.A..b...f..D.o#
########@########
#k.E..a...g..B.n#
########.########
#l.F..d...h..C.m#
#################
Shortest paths are 136 steps;
one is: a, f, b, j, g, n, h, d, l, o, e, p, c, i, k, m

########################
#@..............ac.GI.b#
###d#e#f################
###A#B#C################
###g#h#i################
########################
Shortest paths are 81 steps; one is: a, c, f, i, d, g, b, e, h

How many steps is the shortest path that collects all of the keys?

*/
namespace Day18
{
    struct Node
    {
        public int x;
        public int y;
        public int type;
    };

    class Program
    {
        static readonly int MAX_NUM_KEYS = 26;
        static readonly int TYPE_KEY_START = 10;
        static readonly int TYPE_KEY_END = 10 + MAX_NUM_KEYS;
        static readonly int TYPE_DOOR_START = 100;
        static readonly int TYPE_DOOR_END = 100 + MAX_NUM_KEYS;
        static int[,] sMap;
        static int sMapSizeX;
        static int sMapSizeY;
        static int sStartX;
        static int sStartY;
        static Node[] sNodes;
        static int[] sKeys;
        static int[] sDoors;
        static List<int>[] sLinks;
        static int sNumKeys;

        Program(string inputFile, bool part1)
        {
            var mapSource = ReadProgram(inputFile);
            ParseMap(mapSource);

            if (part1)
            {
                var result = ShortestPath();
                Console.WriteLine($"Day18 Part1:{result}");
            }
        }

        private string[] ReadProgram(string inputFile)
        {
            var mapSource = File.ReadAllLines(inputFile);
            return mapSource;
        }

        public static void ParseMap(string[] mapSource)
        {
            if (mapSource.Length == 0)
            {
                throw new ArgumentException($"map input is empty", nameof(mapSource));
            }
            sMapSizeX = mapSource[0].Length;
            sMapSizeY = 0;
            foreach (var line in mapSource)
            {
                if (line.Length != sMapSizeX)
                {
                    throw new ArgumentException($"map input lines must be the same length {sMapSizeX} {line.Length}", nameof(mapSource));
                }
                sMapSizeY++;
            }
            sMap = new int[sMapSizeX, sMapSizeY];
            sNodes = new Node[sMapSizeX * sMapSizeY];
            sKeys = new int[MAX_NUM_KEYS];
            sDoors = new int[MAX_NUM_KEYS];
            for (int i = 0; i < MAX_NUM_KEYS; ++i)
            {
                sKeys[i] = -1;
                sDoors[i] = -1;
            }
            sLinks = new List<int>[sMapSizeX * sMapSizeY];
            sStartX = -1;
            sStartY = -1;
            sNumKeys = 0;
            for (int y = 0; y < sMapSizeY; ++y)
            {
                var line = mapSource[y];
                for (int x = 0; x < sMapSizeX; ++x)
                {
                    var nodeIndex = GetNodeIndex(x, y);
                    char cell = line[x];
                    if (cell == '.')
                    {
                        sMap[x, y] = 0;
                    }
                    else if (cell == '#')
                    {
                        sMap[x, y] = 1;
                    }
                    else if (cell == '@')
                    {
                        sStartX = x;
                        sStartY = y;
                        sMap[x, y] = 2;
                    }
                    else if ((cell >= 'a') && (cell <= 'z'))
                    {
                        int keyIndex = cell - 'a';
                        sMap[x, y] = TYPE_KEY_START + keyIndex;
                        sNumKeys++;
                        sKeys[keyIndex] = nodeIndex;
                    }
                    else if ((cell >= 'A') && (cell <= 'Z'))
                    {
                        int doorIndex = cell - 'A';
                        sMap[x, y] = TYPE_DOOR_START + doorIndex;
                        sDoors[doorIndex] = nodeIndex;
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
            for (int y = 0; y < sMapSizeY; ++y)
            {
                for (int x = 0; x < sMapSizeX; ++x)
                {
                    var nodeIndex = x + y * sMapSizeX;
                    int cell = sMap[x, y];
                    sLinks[nodeIndex] = new List<int>();
                    sLinks[nodeIndex].Clear();
                    if (cell == 1)
                    {
                        continue;
                    }
                    if ((x - 1 >= 0) && (sMap[x - 1, y] != 1))
                    {
                        var targetNodeIndex = x - 1 + y * sMapSizeX;
                        sLinks[nodeIndex].Add(targetNodeIndex);
                    }
                    if ((x + 1 < sMapSizeX) && (sMap[x + 1, y] != 1))
                    {
                        var targetNodeIndex = x + 1 + y * sMapSizeX;
                        sLinks[nodeIndex].Add(targetNodeIndex);
                    }
                    if ((y - 1 >= 0) && (sMap[x, y - 1] != 1))
                    {
                        var targetNodeIndex = x + (y - 1) * sMapSizeX;
                        sLinks[nodeIndex].Add(targetNodeIndex);
                    }
                    if ((y + 1 < sMapSizeY) && (sMap[x, y + 1] != 1))
                    {
                        var targetNodeIndex = x + (y + 1) * sMapSizeX;
                        sLinks[nodeIndex].Add(targetNodeIndex);
                    }
                }
            }
        }

        public static void OutputMap()
        {
            for (int y = 0; y < sMapSizeY; ++y)
            {
                string line = "";
                for (int x = 0; x < sMapSizeX; ++x)
                {
                    int cell = sMap[x, y];
                    if (cell == 0)
                    {
                        line += '.';
                    }
                    else if (cell == 1)
                    {
                        line += '#';
                    }
                    else if (cell == 2)
                    {
                        line += '@';
                    }
                    else if ((cell >= TYPE_KEY_START) && (cell < TYPE_KEY_END))
                    {
                        line += (char)('a' + (cell - TYPE_KEY_START));
                    }
                    else if ((cell >= TYPE_DOOR_START) && (cell < TYPE_DOOR_END))
                    {
                        line += (char)('A' + (cell - TYPE_DOOR_START));
                    }
                    else
                    {
                        throw new InvalidDataException($"Unknown map[{x},{y}] {cell}");
                    }
                }
                Console.WriteLine($"{line}");
            }

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
            for (int key = 0; key < sKeys.Length; ++key)
            {
                var nodeIndex = sKeys[key];
                if (nodeIndex != -1)
                {
                    (int x, int y) = GetXYFromNodeIndex(nodeIndex);
                    Console.WriteLine($"Key[{key}]:{nodeIndex} {x},{y}");
                }
            }
            for (int door = 0; door < sDoors.Length; ++door)
            {
                var nodeIndex = sDoors[door];
                if (nodeIndex != -1)
                {
                    (int x, int y) = GetXYFromNodeIndex(nodeIndex);
                    Console.WriteLine($"Door[{door}] {nodeIndex} {x},{y}");
                }
            }
        }

        static int GetNodeIndex(int x, int y)
        {
            return x + y * sMapSizeX;
        }

        static (int, int) GetXYFromNodeIndex(int nodeIndex)
        {
            int x = nodeIndex % sMapSizeX;
            int y = nodeIndex / sMapSizeX;
            return (x, y);
        }

        public static int NavigateToKey(int fromX, int fromY, int toKeyIndex, int collectedKeys)
        {
            int keyNodexIndex = sKeys[toKeyIndex];
            (int toX, int toY) = GetXYFromNodeIndex(keyNodexIndex);
            return Navigate(fromX, fromY, toX, toY, collectedKeys);
        }

        public static int Navigate(int fromX, int fromY, int toX, int toY, int collectedKeys)
        {
            int startIndex = GetNodeIndex(fromX, fromY);
            int endIndex = GetNodeIndex(toX, toY);
            Console.WriteLine($"Navigate Start {startIndex} {fromX},{fromY}");
            Queue<int> nodesToVisit = new Queue<int>();
            nodesToVisit.Enqueue(startIndex);
            bool[] visited = new bool[sNodes.Length];
            int[] parents = new int[sNodes.Length];
            for (int i = 0; i < parents.Length; ++i)
            {
                parents[i] = -1;
            }
            while (nodesToVisit.Count > 0)
            {
                int nodeIndex = nodesToVisit.Dequeue();
                (int x, int y) = GetXYFromNodeIndex(nodeIndex);
                Console.WriteLine($"Node:{nodeIndex} {x},{y}");
                if (nodeIndex == endIndex)
                {
                    Console.WriteLine($"Solved");
                    int numSteps = 0;
                    int currentNodeIndex = endIndex;
                    while (currentNodeIndex != -1)
                    {
                        (x, y) = GetXYFromNodeIndex(currentNodeIndex);
                        Console.WriteLine($"Node:{currentNodeIndex} {x},{y}");
                        numSteps++;
                        currentNodeIndex = parents[currentNodeIndex];
                        if (currentNodeIndex == startIndex)
                        {
                            break;
                        }
                    }
                    return numSteps;
                }

                foreach (var link in sLinks[nodeIndex])
                {
                    if (visited[link] == false)
                    {
                        int linkType = sNodes[link].type;
                        bool validLink = linkType < TYPE_DOOR_START;
                        if ((linkType >= TYPE_DOOR_START) && (linkType < TYPE_DOOR_END))
                        {
                            int doorIndex = linkType - TYPE_DOOR_START;
                            if ((collectedKeys & (1 << doorIndex)) != 0)
                            {
                                validLink = true;
                            }
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
            return -1;
        }

        public static int ShortestPath()
        {
            int startIndex = GetNodeIndex(sStartX, sStartY);
            Console.WriteLine($"ShortestPath Start {startIndex} {sStartX},{sStartY}");
            Queue<int> nodesToVisit = new Queue<int>();
            nodesToVisit.Enqueue(startIndex);
            bool[] visited = new bool[sNodes.Length];
            HashSet<int> keysCollected = new HashSet<int>();
            int loops = 0;
            List<int> route = new List<int>();
            int jake = 0;

            while (nodesToVisit.Count > 0)
            {
                loops++;
                if (loops > 1000000)
                {
                    Console.WriteLine($"Failed");
                    return -1;
                }
                int nodeIndex = nodesToVisit.Dequeue();
                (int x, int y) = GetXYFromNodeIndex(nodeIndex);
                Console.WriteLine($"Node:{nodeIndex} {x},{y}");
                route.Add(nodeIndex);
                int type = sNodes[nodeIndex].type;
                if ((type >= TYPE_KEY_START) && (type < TYPE_KEY_END))
                {
                    int key = type;
                    if (!keysCollected.Contains(key))
                    {
                        keysCollected.Add(key);
                        for (int i = 0; i < visited.Length; i++)
                        {
                            visited[i] = false;
                        }
                        visited[nodeIndex] = true;
                    }
                }
                if (keysCollected.Count == sNumKeys)
                {
                    Console.WriteLine($"Solved: {jake}");
                    int numSteps = 0;
                    foreach (int routeNode in route)
                    {
                        (x, y) = GetXYFromNodeIndex(routeNode);
                        Console.WriteLine($"Node:{routeNode} {x},{y}");
                        numSteps++;
                    }
                    return numSteps;
                }

                foreach (var link in sLinks[nodeIndex])
                {
                    if (visited[link] == false)
                    {
                        int linkType = sNodes[link].type;
                        bool validLink = linkType < TYPE_DOOR_START;
                        if ((linkType >= TYPE_DOOR_START) && (linkType < TYPE_DOOR_END))
                        {
                            int doorIndex = linkType - TYPE_DOOR_START;
                            int key = TYPE_KEY_START + doorIndex;
                            if (!keysCollected.Contains(key))
                            {
                                validLink = true;
                            }
                        }
                        if (validLink)
                        {
                            visited[link] = true;
                            nodesToVisit.Enqueue(link);
                        }
                    }
                }
                jake++;
            };
            return -1;
        }

        public static void Run()
        {
            Console.WriteLine("Day18 : Start");
            _ = new Program("Day18/input.txt", true);
            _ = new Program("Day18/input.txt", false);
            Console.WriteLine("Day18 : End");
        }
    }
}
