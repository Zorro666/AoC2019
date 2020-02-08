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

--- Part Two ---

Strangely, the exit isn't open when you reach it. Then, you remember: the ancient Plutonians were famous for building recursive spaces.

The marked connections in the maze aren't portals: they physically connect to a larger or smaller copy of the maze. Specifically, the labeled tiles around the inside edge actually connect to a smaller copy of the same maze, and the smaller copy's inner labeled tiles connect to yet a smaller copy, and so on.

When you enter the maze, you are at the outermost level; when at the outermost level, only the outer labels AA and ZZ function (as the start and end, respectively); all other outer labeled tiles are effectively walls. At any other level, AA and ZZ count as walls, but the other outer labeled tiles bring you one level outward.

Your goal is to find a path through the maze that brings you back to ZZ at the outermost level of the maze.

In the first example above, the shortest path is now the loop around the right side. If the starting level is 0, then taking the previously-shortest path would pass through BC (to level 1), DE (to level 2), and FG (back to level 1). Because this is not the outermost level, ZZ is a wall, and the only option is to go back around to BC, which would only send you even deeper into the recursive maze.

In the second example above, there is no path that brings you to ZZ at the outermost level.

Here is a more interesting example:

             Z L X W       C                 
             Z P Q B       K                 
  ###########.#.#.#.#######.###############  
  #...#.......#.#.......#.#.......#.#.#...#  
  ###.#.#.#.#.#.#.#.###.#.#.#######.#.#.###  
  #.#...#.#.#...#.#.#...#...#...#.#.......#  
  #.###.#######.###.###.#.###.###.#.#######  
  #...#.......#.#...#...#.............#...#  
  #.#########.#######.#.#######.#######.###  
  #...#.#    F       R I       Z    #.#.#.#  
  #.###.#    D       E C       H    #.#.#.#  
  #.#...#                           #...#.#  
  #.###.#                           #.###.#  
  #.#....OA                       WB..#.#..ZH
  #.###.#                           #.#.#.#  
CJ......#                           #.....#  
  #######                           #######  
  #.#....CK                         #......IC
  #.###.#                           #.###.#  
  #.....#                           #...#.#  
  ###.###                           #.#.#.#  
XF....#.#                         RF..#.#.#  
  #####.#                           #######  
  #......CJ                       NM..#...#  
  ###.#.#                           #.###.#  
RE....#.#                           #......RF
  ###.###        X   X       L      #.#.#.#  
  #.....#        F   Q       P      #.#.#.#  
  ###.###########.###.#######.#########.###  
  #.....#...#.....#.......#...#.....#.#...#  
  #####.#.###.#######.#######.###.###.#.#.#  
  #.......#.......#.#.#.#.#...#...#...#.#.#  
  #####.###.#####.#.#.#.#.###.###.#.###.###  
  #.......#.....#.#...#...............#...#  
  #############.#.#.###.###################  
               A O F   N                     
               A A D   M                     
One shortest path through the maze is the following:

Walk from AA to XF (16 steps)
Recurse into level 1 through XF (1 step)
Walk from XF to CK (10 steps)
Recurse into level 2 through CK (1 step)
Walk from CK to ZH (14 steps)
Recurse into level 3 through ZH (1 step)
Walk from ZH to WB (10 steps)
Recurse into level 4 through WB (1 step)
Walk from WB to IC (10 steps)
Recurse into level 5 through IC (1 step)
Walk from IC to RF (10 steps)
Recurse into level 6 through RF (1 step)
Walk from RF to NM (8 steps)
Recurse into level 7 through NM (1 step)
Walk from NM to LP (12 steps)
Recurse into level 8 through LP (1 step)
Walk from LP to FD (24 steps)
Recurse into level 9 through FD (1 step)
Walk from FD to XQ (8 steps)
Recurse into level 10 through XQ (1 step)
Walk from XQ to WB (4 steps)
Return to level 9 through WB (1 step)
Walk from WB to ZH (10 steps)
Return to level 8 through ZH (1 step)
Walk from ZH to CK (14 steps)
Return to level 7 through CK (1 step)
Walk from CK to XF (10 steps)
Return to level 6 through XF (1 step)
Walk from XF to OA (14 steps)
Return to level 5 through OA (1 step)
Walk from OA to CJ (8 steps)
Return to level 4 through CJ (1 step)
Walk from CJ to RE (8 steps)
Return to level 3 through RE (1 step)
Walk from RE to IC (4 steps)
Recurse into level 4 through IC (1 step)
Walk from IC to RF (10 steps)
Recurse into level 5 through RF (1 step)
Walk from RF to NM (8 steps)
Recurse into level 6 through NM (1 step)
Walk from NM to LP (12 steps)
Recurse into level 7 through LP (1 step)
Walk from LP to FD (24 steps)
Recurse into level 8 through FD (1 step)
Walk from FD to XQ (8 steps)
Recurse into level 9 through XQ (1 step)
Walk from XQ to WB (4 steps)
Return to level 8 through WB (1 step)
Walk from WB to ZH (10 steps)
Return to level 7 through ZH (1 step)
Walk from ZH to CK (14 steps)
Return to level 6 through CK (1 step)
Walk from CK to XF (10 steps)
Return to level 5 through XF (1 step)
Walk from XF to OA (14 steps)
Return to level 4 through OA (1 step)
Walk from OA to CJ (8 steps)
Return to level 3 through CJ (1 step)
Walk from CJ to RE (8 steps)
Return to level 2 through RE (1 step)
Walk from RE to XQ (14 steps)
Return to level 1 through XQ (1 step)
Walk from XQ to FD (8 steps)
Return to level 0 through FD (1 step)
Walk from FD to ZZ (18 steps)
This path takes a total of 396 steps to move from AA at the outermost layer to ZZ at the outermost layer.

In your maze, when accounting for recursion, how many steps does it take to get from the open tile marked AA to the open tile marked ZZ, both at the outermost layer?

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
        static (int minX, int minY, int maxX, int maxY) sOuterWallBBOX;
        static (int minX, int minY, int maxX, int maxY) sInnerWallBBOX;
        static int sStartNode;
        static int sExitNode;
        static Node[] sNodes;
        static List<(int nodeIndex, int levelDelta)>[] sLinks;
        static List<(int id0, int id1, int entryIndex, int exitIndex, int levelDelta)> sPortals;

        private Program(string inputFile, bool part1)
        {
            var mapSource = ReadProgram(inputFile);
            if (part1)
            {
                ParseMap(mapSource);
                //OutputMap(false);
                var result = ShortestPath(false);
                Console.WriteLine($"Day20 : Result1 {result}");
                if (result != 714)
                {
                    throw new InvalidDataException($"Part1 has been broken {result} != 714");
                }
            }
            else
            {
                ParseMap(mapSource);
                OutputMap(false);
                var result = ShortestPath(true);
                Console.WriteLine($"Day20 : Result2 {result}");
                if (result != 7876)
                {
                    throw new InvalidDataException($"Part2 has been broken {result} != 7876");
                }
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
            sLinks = new List<(int, int)>[sMapSize.w * sMapSize.h];
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

            // Find the inner and outer wall bounding box
            sOuterWallBBOX = (int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
            foreach (var node in sNodes)
            {
                if (node.type != 1)
                {
                    continue;
                }
                var x = node.x;
                var y = node.y;
                if (GetMapCell(x - 1, y) == -1)
                {
                    sOuterWallBBOX.minX = Math.Min(sOuterWallBBOX.minX, x);
                }
                if (GetMapCell(x + 1, y) == -1)
                {
                    sOuterWallBBOX.maxX = Math.Max(sOuterWallBBOX.maxX, x);
                }
                if (GetMapCell(x, y - 1) == -1)
                {
                    sOuterWallBBOX.minY = Math.Min(sOuterWallBBOX.minY, y);
                }
                if (GetMapCell(x, y + 1) == -1)
                {
                    sOuterWallBBOX.maxY = Math.Max(sOuterWallBBOX.maxY, y);
                }
            }
            if ((sOuterWallBBOX.minX == int.MaxValue) || (sOuterWallBBOX.minY == int.MaxValue) ||
                (sOuterWallBBOX.maxX == int.MinValue) || (sOuterWallBBOX.maxY == int.MinValue))
            {
                throw new InvalidDataException($"Invalid OuterWallBBOX {sOuterWallBBOX.minX},{sOuterWallBBOX.minY} -> {sOuterWallBBOX.maxX},{sOuterWallBBOX.maxY}");
            }

            sInnerWallBBOX = (int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
            foreach (var node in sNodes)
            {
                if (node.type != 1)
                {
                    continue;
                }
                var x = node.x;
                var y = node.y;
                if ((x == sOuterWallBBOX.minX) || (x == sOuterWallBBOX.maxX))
                {
                    continue;
                }
                if ((y == sOuterWallBBOX.minY) || (y == sOuterWallBBOX.maxY))
                {
                    continue;
                }
                if (GetMapCell(x + 1, y) == -1)
                {
                    sInnerWallBBOX.minX = Math.Min(sInnerWallBBOX.minX, x);
                }
                if (GetMapCell(x - 1, y) == -1)
                {
                    sInnerWallBBOX.maxX = Math.Max(sInnerWallBBOX.maxX, x);
                }
                if (GetMapCell(x, y + 1) == -1)
                {
                    sInnerWallBBOX.minY = Math.Min(sInnerWallBBOX.minY, y);
                }
                if (GetMapCell(x, y - 1) == -1)
                {
                    sInnerWallBBOX.maxY = Math.Max(sInnerWallBBOX.maxY, y);
                }
            }
            if ((sInnerWallBBOX.minX == int.MaxValue) || (sInnerWallBBOX.minY == int.MaxValue) ||
                (sInnerWallBBOX.maxX == int.MinValue) || (sInnerWallBBOX.maxY == int.MinValue))
            {
                throw new InvalidDataException($"Invalid InnerWallBBOX {sInnerWallBBOX.minX},{sInnerWallBBOX.minY} -> {sInnerWallBBOX.maxX},{sInnerWallBBOX.maxY}");
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

                // Find the entry/exit pont for this portal instance
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
                bool exists = false;
                foreach (var p in portals)
                {
                    if ((p.id0 == validPortal.id) && (p.id1 == pId) && (p.entryExitX == entryExit.x) && (p.entryExitY == entryExit.y))
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                {
                    portals.Add((pId, validPortal.id, entryExit.x, entryExit.y));
                }
            }
            // Find the start and end portal instances
            foreach (var portal in portals)
            {
                if (portal.id0 == portal.id1)
                {
                    var nodeIndex = GetNodeIndex(portal.entryExitX, portal.entryExitY);
                    if (portal.id0 == PORTAL_START + 'A' - 'A')
                    {
                        if (sStartNode == -1)
                        {
                            sStartNode = nodeIndex;
                        }
                        else
                        {
                            throw new InvalidDataException($"Found more than one start points this:{nodeIndex} existing:{sStartNode}");
                        }
                    }
                    else if (portal.id0 == PORTAL_START + 'Z' - 'A')
                    {
                        if (sExitNode == -1)
                        {
                            sExitNode = GetNodeIndex(portal.entryExitX, portal.entryExitY);
                        }
                        else
                        {
                            throw new InvalidDataException($"Found more than one exit points this:{nodeIndex} existing:{sExitNode}");
                        }
                    }
                }
            }
            // Find the connected portal to make : PortalId: enterIndex exitIndex
            sPortals = new List<(int id0, int id1, int entryIndex, int exitIndex, int levelDelta)>(50);
            foreach (var portal in portals)
            {
                var nodeIndex = GetNodeIndex(portal.entryExitX, portal.entryExitY);
                int id0 = portal.id0;
                int id1 = portal.id1;
                int countConnections = 0;
                int targetIndex = -1;
                foreach (var targetPortal in portals)
                {
                    if (targetPortal != portal)
                    {
                        if ((id0 == targetPortal.id0) && (id1 == targetPortal.id1))
                        {
                            ++countConnections;
                            targetIndex = GetNodeIndex(targetPortal.entryExitX, targetPortal.entryExitY);
                        }
                    }
                }
                if (nodeIndex == sStartNode)
                {
                    if (countConnections != 0)
                    {
                        throw new InvalidDataException($"Invalid for start point to have connections {countConnections}");
                    }
                }
                else if (nodeIndex == sExitNode)
                {
                    if (countConnections != 0)
                    {
                        throw new InvalidDataException($"Invalid for exit point to have connections {countConnections}");
                    }
                }
                else if (countConnections == 1)
                {
                    int levelDelta = 0;
                    // From inner edge -> outer edge : +1
                    // From outer edge -> inner edge : -1
                    (int exitX, int exitY) = GetXYFromNodeIndex(targetIndex);
                    bool startIsInner = IsInnerEdge(portal.entryExitX, portal.entryExitY);
                    bool startIsOuter = IsOuterEdge(portal.entryExitX, portal.entryExitY);
                    bool exitIsInner = IsInnerEdge(exitX, exitY);
                    bool exitIsOuter = IsOuterEdge(exitX, exitY);
                    if (startIsInner == startIsOuter)
                    {
                        throw new InvalidDataException($"Invalid portal {portal.entryExitX},{portal.entryExitY} start is inner & outer");
                    }
                    if (exitIsInner == exitIsOuter)
                    {
                        throw new InvalidDataException($"Invalid portal {portal.entryExitX},{portal.entryExitY} exit is inner & outer");
                    }
                    if (startIsInner && exitIsOuter)
                    {
                        levelDelta = 1;
                    }
                    if (startIsOuter && exitIsInner)
                    {
                        levelDelta = -1;
                    }

                    if (levelDelta == 0)
                    {
                        throw new InvalidDataException($"Invalid portal {portal.entryExitX},{portal.entryExitY} levelDetla == 0");
                    }
                    sPortals.Add((portal.id0, portal.id1, nodeIndex, targetIndex, levelDelta));
                }
                else
                {
                    throw new InvalidDataException($"Invalid portal {portal.entryExitX},{portal.entryExitY} connections count {countConnections}");
                }
            }

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
                    sLinks[nodeIndex] = new List<(int, int)>();
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
                        sLinks[nodeIndex].Add((targetNodeIndex, 0));
                    }
                    if ((x + 1 < sMapSize.w) && (sMap[x + 1, y] != 1))
                    {
                        var targetNodeIndex = x + 1 + y * sMapSize.w;
                        sLinks[nodeIndex].Add((targetNodeIndex, 0));
                    }
                    if ((y - 1 >= 0) && (sMap[x, y - 1] != 1))
                    {
                        var targetNodeIndex = x + (y - 1) * sMapSize.w;
                        sLinks[nodeIndex].Add((targetNodeIndex, 0));
                    }
                    if ((y + 1 < sMapSize.h) && (sMap[x, y + 1] != 1))
                    {
                        var targetNodeIndex = x + (y + 1) * sMapSize.w;
                        sLinks[nodeIndex].Add((targetNodeIndex, 0));
                    }
                }
            }

            // Add links between portals
            foreach (var portal in sPortals)
            {
                sLinks[portal.entryIndex].Add((portal.exitIndex, portal.levelDelta));
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
                foreach (var portal in sPortals)
                {
                    (int entryX, int entryY) = GetXYFromNodeIndex(portal.entryIndex);
                    (int exitX, int exitY) = GetXYFromNodeIndex(portal.exitIndex);
                    Console.WriteLine($"Portal {(char)(portal.id0 - PORTAL_START + 'A')}{(char)(portal.id1 - PORTAL_START + 'A')} {entryX}, {entryY} -> {exitX}, {exitY} levelDelta:{portal.levelDelta}");
                }
            }

            if (detailed)
            {
                for (int nodeIndex = 0; nodeIndex < sNodes.Length; ++nodeIndex)
                {
                    var node = sNodes[nodeIndex];
                    Console.WriteLine($"Node:{node.x},{node.y} {node.type}");
                    foreach (var linkTargetIndex in sLinks[nodeIndex])
                    {
                        (int linkX, int linkY) = GetXYFromNodeIndex(linkTargetIndex.nodeIndex);
                        Console.WriteLine($"Link:{linkTargetIndex} {linkX},{linkY} levelDelta:{linkTargetIndex.levelDelta}");
                    }
                }
            }
            Console.WriteLine($"Map Dimensions:{sMapSize.w} x {sMapSize.h}");
            Console.WriteLine($"InnerBBOX:{sInnerWallBBOX.minX},{sInnerWallBBOX.minY} -> {sInnerWallBBOX.maxX},{sInnerWallBBOX.maxY}");
            Console.WriteLine($"OuterBBOX:{sOuterWallBBOX.minX},{sOuterWallBBOX.minY} -> {sOuterWallBBOX.maxX},{sOuterWallBBOX.maxY}");
            Console.WriteLine($"PortalCount:{sPortals.Count}");
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

        static bool IsInnerEdge(int x, int y)
        {
            if ((x == sInnerWallBBOX.minX) || (x == sInnerWallBBOX.maxX))
            {
                return true;
            }
            else if ((y == sInnerWallBBOX.minY) || (y == sInnerWallBBOX.maxY))
            {
                return true;
            }
            return false;
        }

        static bool IsOuterEdge(int x, int y)
        {
            if ((x == sOuterWallBBOX.minX) || (x == sOuterWallBBOX.maxX))
            {
                return true;
            }
            else if ((y == sOuterWallBBOX.minY) || (y == sOuterWallBBOX.maxY))
            {
                return true;
            }
            return false;
        }

        public static int ShortestPath(bool part2)
        {
            return ShortestPath(sStartNode, sExitNode, part2);
        }

        static int ShortestPath(int startIndex, int endIndex, bool part2)
        {
            //(int fromX, int fromY) = GetXYFromNodeIndex(startIndex);
            //Console.WriteLine($"ShortestPath Start {startIndex} {fromX},{fromY}");
            Queue<(int nodeIndex, int level)> nodesToVisit = new Queue<(int, int)>();
            nodesToVisit.Enqueue((startIndex, 0));
            List<(int nodeIndex, int level)> visited = new List<(int, int)>(sNodes.Length * 100);
            Dictionary<(int nodexIndex, int level), (int nodeIndex, int level)> parents = new Dictionary<(int, int), (int, int)>(sNodes.Length * 100);
            int minNumSteps = int.MaxValue;
            while (nodesToVisit.Count > 0)
            {
                var node = nodesToVisit.Dequeue();
                int nodeIndex = node.nodeIndex;
                var currentLevel = part2 ? node.level : 0;
                (int x, int y) = GetXYFromNodeIndex(nodeIndex);
                //Console.WriteLine($"Node:{nodeIndex} {x},{y} Level {currentLevel}");
                /*
                if (nodeIndex == endIndex)
                {
                    Console.WriteLine($"Found the end {currentLevel}");
                }
                */
                if ((nodeIndex == endIndex) && (currentLevel == 0))
                {
                    //Console.WriteLine($"Solved");
                    int numSteps = 0;
                    (int, int) currentNode = (endIndex, 0);
                    bool foundParent = true;
                    while (foundParent)
                    {
                        //(x, y) = GetXYFromNodeIndex(currentNodeIndex);
                        //Console.WriteLine($"Node:{currentNodeIndex} {x},{y}");
                        numSteps++;
                        foundParent = parents.TryGetValue(currentNode, out var parentNode);
                        if (parentNode.nodeIndex == startIndex)
                        {
                            break;
                        }
                        currentNode = parentNode;
                    }
                    //Console.WriteLine($"numSteps:{numSteps}");
                    if (numSteps < minNumSteps)
                    {
                        minNumSteps = numSteps;
                    }
                }

                foreach (var link in sLinks[nodeIndex])
                {
                    (int nodexIndex, int level) newLink = link;
                    newLink.level = part2 ? currentLevel + link.levelDelta : 0;
                    if ((newLink.level >= 0) && (newLink.level <= 25))
                    {
                        if (!visited.Contains(newLink))
                        {
                            visited.Add(newLink);
                            nodesToVisit.Enqueue(newLink);
                            parents[newLink] = (nodeIndex, currentLevel);
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
