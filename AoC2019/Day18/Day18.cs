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

Your puzzle answer was 4590.

--- Part Two ---

You arrive at the vault only to discover that there is not one vault, but four - each with its own entrance.

On your map, find the area in the middle that looks like this:

...
.@.
...
Update your map to instead use the correct data:

@#@
###
@#@
This change will split your map into four separate sections, each with its own entrance:

#######       #######
#a.#Cd#       #a.#Cd#
##...##       ##@#@##
##.@.##  -->  #######
##...##       ##@#@##
#cB#Ab#       #cB#Ab#
#######       #######
Because some of the keys are for doors in other vaults, it would take much too long to collect all of the keys by yourself. Instead, you deploy four remote-controlled robots. Each starts at one of the entrances (@).

Your goal is still to collect all of the keys in the fewest steps, but now, each robot has its own position and can move independently. You can only remotely control a single robot at a time. Collecting a key instantly unlocks any corresponding doors, regardless of the vault in which the key or door is found.

For example, in the map above, the top-left robot first collects key a, unlocking door A in the bottom-right vault:

#######
#@.#Cd#
##.#@##
#######
##@#@##
#cB#.b#
#######
Then, the bottom-right robot collects key b, unlocking door B in the bottom-left vault:

#######
#@.#Cd#
##.#@##
#######
##@#.##
#c.#.@#
#######
Then, the bottom-left robot collects key c:

#######
#@.#.d#
##.#@##
#######
##.#.##
#@.#.@#
#######
Finally, the top-right robot collects key d:

#######
#@.#.@#
##.#.##
#######
##.#.##
#@.#.@#
#######
In this example, it only took 8 steps to collect all of the keys.

Sometimes, multiple robots might have keys available, or a robot might have to wait for multiple keys to be collected:

###############
#d.ABC.#.....a#
######@#@######
###############
######@#@######
#b.....#.....c#
###############
First, the top-right, bottom-left, and bottom-right robots take turns collecting keys a, b, and c, a total of 6 + 6 + 6 = 18 steps. Then, the top-left robot can access key d, spending another 6 steps; collecting all of the keys here takes a minimum of 24 steps.

Here's a more complex example:

#############
#DcBa.#.GhKl#
#.###@#@#I###
#e#d#####j#k#
###C#@#@###J#
#fEbA.#.FgHi#
#############
Top-left robot collects key a.
Bottom-left robot collects key b.
Top-left robot collects key c.
Bottom-left robot collects key d.
Top-left robot collects key e.
Bottom-left robot collects key f.
Bottom-right robot collects key g.
Top-right robot collects key h.
Bottom-right robot collects key i.
Top-right robot collects key j.
Bottom-right robot collects key k.
Top-right robot collects key l.
In the above example, the fewest steps to collect all of the keys is 32.

Here's an example with more choices:

#############
#g#f.D#..h#l#
#F###e#E###.#
#dCba@#@BcIJ#
#############
#nK.L@#@G...#
#M###N#H###.#
#o#m..#i#jk.#
#############
One solution with the fewest steps is:

Top-left robot collects key e.
Top-right robot collects key h.
Bottom-right robot collects key i.
Top-left robot collects key a.
Top-left robot collects key b.
Top-right robot collects key c.
Top-left robot collects key d.
Top-left robot collects key f.
Top-left robot collects key g.
Bottom-right robot collects key k.
Bottom-right robot collects key j.
Top-right robot collects key l.
Bottom-left robot collects key n.
Bottom-left robot collects key m.
Bottom-left robot collects key o.
This example requires at least 72 steps to collect all keys.

After updating your map and using the remote-controlled robots, what is the fewest steps necessary to collect all of the keys?

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
        static (int w, int h) sMapSize;
        static List<int> sStartNodes;
        static Node[] sNodes;
        static int[] sKeys;
        static int[] sDoors;
        static List<int>[] sLinks;
        static Dictionary<(int, int, int), int> sVisibilityCache;
        static int[] sVisibleKeyStartNodes;
        static int[] sKeyToRobotIndex;
        static int sNumKeys;
        static int sCollectedAllKeys;

        Program(string inputFile, bool part1)
        {
            var mapSource = ReadProgram(inputFile);

            if (part1)
            {
                ParseMap(mapSource, false);
                OutputMap(false);
                var result = ShortestPath();
                Console.WriteLine($"Day18 Part1:{result}");
                if (4590 != result)
                {
                    throw new InvalidOperationException($"ShortestRoute for part1 is wrong {result} != 4590");
                }
            }
            else
            {
                ParseMap(mapSource, true);
                OutputMap(false);
                var result = ShortestPath();
                Console.WriteLine($"Day18 Part2:{result}");
                if (2086 != result)
                {
                    throw new InvalidOperationException($"ShortestRoute for part2 is wrong {result} != 2086");
                }
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
            sKeys = new int[MAX_NUM_KEYS];
            sDoors = new int[MAX_NUM_KEYS];
            for (int i = 0; i < MAX_NUM_KEYS; ++i)
            {
                sKeys[i] = -1;
                sDoors[i] = -1;
            }
            sLinks = new List<int>[sMapSize.w * sMapSize.h];
            sStartNodes = new List<int>(4);
            sNumKeys = 0;
            sCollectedAllKeys = 0;
            for (int y = 0; y < sMapSize.h; ++y)
            {
                var line = mapSource[y];
                for (int x = 0; x < sMapSize.w; ++x)
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
                        sStartNodes.Add(GetNodeIndex(x, y));
                        sMap[x, y] = 2;
                    }
                    else if ((cell >= 'a') && (cell <= 'z'))
                    {
                        int keyIndex = cell - 'a';
                        sMap[x, y] = TYPE_KEY_START + keyIndex;
                        sCollectedAllKeys |= 1 << sNumKeys;
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
            if (sStartNodes.Count != 1)
            {
                throw new InvalidDataException($"Map by default should have precisely one starting point {sStartNodes.Count}");
            }
            if (applyPart2Setup)
            {
                int startNode = sStartNodes[0];
                (int startX, int startY) = GetXYFromNodeIndex(startNode);
                /*
                FROM
                ...
                .@.
                ...
                TO
                @#@
                ###
                @#@
                */
                sMap[startX - 1, startY - 1] = 2;
                sMap[startX + 0, startY - 1] = 1;
                sMap[startX + 1, startY - 1] = 2;

                sMap[startX - 1, startY + 0] = 1;
                sMap[startX + 0, startY + 0] = 1;
                sMap[startX + 1, startY + 0] = 1;

                sMap[startX - 1, startY + 1] = 2;
                sMap[startX + 0, startY + 1] = 1;
                sMap[startX + 1, startY + 1] = 2;

                for (int y = startY - 1; y <= startY + 1; ++y)
                {
                    for (int x = startX - 1; x <= startX + 1; ++x)
                    {
                        var nodeIndex = GetNodeIndex(x, y);
                        sNodes[nodeIndex].type = sMap[x, y];
                    }
                }
                sStartNodes.Clear();
                sStartNodes.Add(GetNodeIndex(startX - 1, startY - 1));
                sStartNodes.Add(GetNodeIndex(startX + 1, startY - 1));
                sStartNodes.Add(GetNodeIndex(startX - 1, startY + 1));
                sStartNodes.Add(GetNodeIndex(startX + 1, startY + 1));
                if (sStartNodes.Count != 4)
                {
                    throw new InvalidDataException($"After modification the map should have precisely four starting points {sStartNodes.Count}");
                }
            }

            for (int y = 0; y < sMapSize.h; ++y)
            {
                for (int x = 0; x < sMapSize.w; ++x)
                {
                    var nodeIndex = x + y * sMapSize.w;
                    int cell = sMap[x, y];
                    sLinks[nodeIndex] = new List<int>();
                    sLinks[nodeIndex].Clear();
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
            // Compute which keys are visible from each starting point
            sVisibleKeyStartNodes = new int[sNumKeys];
            sKeyToRobotIndex = new int[sNumKeys];
            for (int k = 0; k < sNumKeys; ++k)
            {
                sVisibleKeyStartNodes[k] = -1;
                sKeyToRobotIndex[k] = -1;
            }
            for (int robotIndex = 0; robotIndex < sStartNodes.Count; ++robotIndex)
            {
                int startIndex = sStartNodes[robotIndex];
                (int x, int y) = GetXYFromNodeIndex(startIndex);
                for (int k = 0; k < sNumKeys; ++k)
                {
                    int endIndex = sKeys[k];
                    int numSteps = Navigate(startIndex, endIndex, sCollectedAllKeys);
                    if (numSteps != -1)
                    {
                        char key = (char)('a' + k);
                        Console.WriteLine($"{x},{y} Visible Key {key} {numSteps}");
                        if (sVisibleKeyStartNodes[k] >= 0)
                        {
                            throw new InvalidDataException($"Each key must belong to only one start location {key}");
                        }
                        sVisibleKeyStartNodes[k] = startIndex;
                        sKeyToRobotIndex[k] = robotIndex;
                    }
                }
            }
            if (sVisibleKeyStartNodes.Length != sNumKeys)
            {
                throw new InvalidDataException($"All keys must be visible {sVisibleKeyStartNodes.Length} != {sNumKeys}");
            }
            //<(int fromNode, int toNode, int collectedKeys), int numSteps>
            sVisibilityCache = new Dictionary<(int, int, int), int>(10000);
        }

        static bool SearchVisibilityCache(int fromIndex, int endIndex, int collectedKeys, out int numStepsFromCache)
        {
            int numSteps;
            if (sVisibilityCache.TryGetValue((fromIndex, endIndex, collectedKeys), out numSteps))
            {
                numStepsFromCache = numSteps;
                return true;
            }
            else
            {
                if (sVisibilityCache.TryGetValue((endIndex, fromIndex, collectedKeys), out numSteps))
                {
                    numStepsFromCache = numSteps;
                    return true;
                }
            }
            numStepsFromCache = -1;
            return false;
        }

        static void UpdateVisibilityCache(int fromIndex, int endIndex, int collectedKeys, int numSteps)
        {
            sVisibilityCache[(fromIndex, endIndex, collectedKeys)] = numSteps;
        }

        static void ShortestPath(int[] robotNodeIndexes, int inCollectedKeys, ref List<int> path, int totalSteps, ref int minTotalSteps)
        {
            for (int key = 0; key < sNumKeys; ++key)
            {
                int keyIndex = sKeys[key];
                if (keyIndex < 0)
                {
                    continue;
                }
                // Compute which robot we are trying to move and what location the robot is at
                int robotToMove = sKeyToRobotIndex[key];
                int fromIndex = robotNodeIndexes[robotToMove];
                if (keyIndex == fromIndex)
                {
                    continue;
                }

                if (inCollectedKeys == 0)
                {
                    Console.WriteLine($"{key}");
                }
                int collectedKeys = inCollectedKeys;
                if (((collectedKeys & (1 << key)) == 0) && (totalSteps <= minTotalSteps))
                {
                    int numSteps;
                    int numStepsFromCache;
                    bool isInCache = false;
                    if (SearchVisibilityCache(fromIndex, keyIndex, collectedKeys, out numStepsFromCache))
                    {
                        numSteps = numStepsFromCache;
                        isInCache = true;
                    }
                    else
                    {
                        numSteps = Navigate(fromIndex, keyIndex, collectedKeys);
                    }
                    if (!isInCache)
                    {
                        UpdateVisibilityCache(fromIndex, keyIndex, collectedKeys, numSteps);
                    }
                    if (numSteps != -1)
                    {
                        //(int fromX, int fromY) = GetXYFromNodeIndex(fromIndex);
                        int newTotalSteps = totalSteps + numSteps;
                        if (newTotalSteps < minTotalSteps)
                        {
                            //Console.WriteLine($"{fromX},{fromY} Visible Key {(char)('a' + key)}");
                            path.Add(key);
                            int newCollectedKeys = collectedKeys | (1 << key);
                            if (newCollectedKeys == sCollectedAllKeys)
                            {
                                if (newTotalSteps <= minTotalSteps)
                                {
                                    minTotalSteps = newTotalSteps;
                                    Console.WriteLine($"Found all keys New Best {newTotalSteps}");
                                    OutputRoute(path);
                                }
                                else
                                {
                                    Console.WriteLine($"Found all keys TOO LONG {newTotalSteps}");
                                }
                                path.Remove(key);
                                return;
                            }
                            int previousRobotIndex = robotNodeIndexes[robotToMove];
                            robotNodeIndexes[robotToMove] = keyIndex;
                            ShortestPath(robotNodeIndexes, newCollectedKeys, ref path, newTotalSteps, ref minTotalSteps);
                            robotNodeIndexes[robotToMove] = previousRobotIndex;
                            path.Remove(key);
                        }
                        else
                        {
                            //Console.WriteLine($"{fromX},{fromY} Visible Key {(char)('a' + key)} PRUNING {totalSteps} {minTotalSteps}");
                        }
                    }
                }
            }
        }

        static void OutputRoute(List<int> path)
        {
            int pathLen = 0;
            int collectedKeys = 0;
            var robotNodeIndexes = new int[sStartNodes.Count];
            for (int i = 0; i < sStartNodes.Count; ++i)
            {
                robotNodeIndexes[i] = sStartNodes[i];
            }
            foreach (var key in path)
            {
                int endIndex = sKeys[key];
                int robotToMove = sKeyToRobotIndex[key];
                int startIndex = robotNodeIndexes[robotToMove];
                int numSteps = Navigate(startIndex, endIndex, collectedKeys);
                //int numSteps = 666;
                Console.WriteLine($"Key {(char)('a' + key)} numSteps:{numSteps}");
                collectedKeys |= (1 << key);
                pathLen += numSteps;
                robotNodeIndexes[robotToMove] = endIndex;
            }
            Console.WriteLine($"pathLen:{pathLen}");
        }

        public static void OutputMap(bool detailed)
        {
            for (int y = 0; y < sMapSize.h; ++y)
            {
                string line = "";
                for (int x = 0; x < sMapSize.w; ++x)
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
            return x + y * sMapSize.w;
        }

        static (int, int) GetXYFromNodeIndex(int nodeIndex)
        {
            int x = nodeIndex % sMapSize.w;
            int y = nodeIndex / sMapSize.w;
            return (x, y);
        }

        public static int NavigateToKey(int fromX, int fromY, int toKey, int collectedKeys)
        {
            int startIndex = GetNodeIndex(fromX, fromY);
            return NavigateToKey(startIndex, toKey, collectedKeys);
        }

        static int NavigateToKey(int startIndex, int toKey, int collectedKeys)
        {
            int endIndex = sKeys[toKey];
            return Navigate(startIndex, endIndex, collectedKeys);
        }

        public static int Navigate(int fromX, int fromY, int toX, int toY, int collectedKeys)
        {
            int startIndex = GetNodeIndex(fromX, fromY);
            int endIndex = GetNodeIndex(toX, toY);
            return Navigate(startIndex, endIndex, collectedKeys);
        }

        static int Navigate(int startIndex, int endIndex, int collectedKeys)
        {
            //(int fromX, int fromY) = GetXYFromNodeIndex(startIndex);
            //Console.WriteLine($"Navigate Start {startIndex} {fromX},{fromY}");
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
                        bool validLink = linkType < TYPE_DOOR_START;
                        if ((linkType >= TYPE_DOOR_START) && (linkType < TYPE_DOOR_END))
                        {
                            int doorIndex = linkType - TYPE_DOOR_START;
                            if ((collectedKeys & (1 << doorIndex)) != 0)
                            {
                                validLink = true;
                            }
                        }
                        // Ignore link if it woud collect a new key
                        if ((linkType >= TYPE_KEY_START) && (linkType < TYPE_KEY_END))
                        {
                            int key = linkType - TYPE_KEY_START;
                            if ((collectedKeys & (1 << key)) == 0)
                            {
                                validLink = false;
                            }
                        }
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

        public static int ShortestPath()
        {
            int collectedKeys = 0;
            var path = new List<int>(MAX_NUM_KEYS);
            int minTotalSteps = int.MaxValue;
            var robotNodeIndexes = new int[sStartNodes.Count];
            for (int i = 0; i < sStartNodes.Count; ++i)
            {
                robotNodeIndexes[i] = sStartNodes[i];
            }
            ShortestPath(robotNodeIndexes, collectedKeys, ref path, 0, ref minTotalSteps);
            Console.WriteLine($"MinTotalSteps {minTotalSteps}");
            return minTotalSteps;
        }

        public static void Run()
        {
            Console.WriteLine("Day18 : Start");
            //_ = new Program("Day18/input.txt", true);
            _ = new Program("Day18/input.txt", false);
            Console.WriteLine("Day18 : End");
        }
    }
}
