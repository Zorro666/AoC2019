using System;
using System.Collections.Generic;
using System.IO;

/*

--- Day 6: Universal Orbit Map ---

You've landed at the Universal Orbit Map facility on Mercury. Because navigation in space often involves transferring between orbits, the orbit maps here are useful for finding efficient routes between, for example, you and Santa. You download a map of the local orbits (your puzzle input).

Except for the universal Center of Mass (COM), every object in space is in orbit around exactly one other object. An orbit looks roughly like this:

                  \
                   \
                    |
                    |
AAA--> o            o <--BBB
                    |
                    |
                   /
                  /
In this diagram, the object BBB is in orbit around AAA. The path that BBB takes around AAA (drawn with lines) is only partly shown. In the map data, this orbital relationship is written AAA)BBB, which means "BBB is in orbit around AAA".

Before you use your map data to plot a course, you need to make sure it wasn't corrupted during the download. To verify maps, the Universal Orbit Map facility uses orbit count checksums - the total number of direct orbits (like the one shown above) and indirect orbits.

Whenever A orbits B and B orbits C, then A indirectly orbits C. This chain can be any number of objects long: if A orbits B, B orbits C, and C orbits D, then A indirectly orbits D.

For example, suppose you have the following map:

COM)B
B)C
C)D
D)E
E)F
B)G
G)H
D)I
E)J
J)K
K)L
Visually, the above map of orbits looks like this:

        G - H       J - K - L
       /           /
COM - B - C - D - E - F
               \
                I
In this visual representation, when two objects are connected by a line, the one on the right directly orbits the one on the left.

Here, we can count the total number of orbits as follows:

D directly orbits C and indirectly orbits B and COM, a total of 3 orbits.
L directly orbits K and indirectly orbits J, E, D, C, B, and COM, a total of 7 orbits.
COM orbits nothing.
The total number of direct and indirect orbits in this example is 42.

What is the total number of direct and indirect orbits in your map data?

--- Part Two ---

Now, you just need to figure out how many orbital transfers you (YOU) need to take to get to Santa (SAN).

You start at the object YOU are orbiting; your destination is the object SAN is orbiting. An orbital transfer lets you move from any object to an object orbiting or orbited by that object.

For example, suppose you have the following map:

COM)B
B)C
C)D
D)E
E)F
B)G
G)H
D)I
E)J
J)K
K)L
K)YOU
I)SAN
Visually, the above map of orbits looks like this:

                          YOU
                         /
        G - H       J - K - L
       /           /
COM - B - C - D - E - F
               \
                I - SAN
In this example, YOU are in orbit around K, and SAN is in orbit around I. To move from K to I, a minimum of 4 orbital transfers are required:

K to J
J to E
E to D
D to I
Afterward, the map of orbits looks like this:

        G - H       J - K - L
       /           /
COM - B - C - D - E - F
               \
                I - SAN
                 \
                  YOU
What is the minimum number of orbital transfers required to move from the object YOU are orbiting to the object SAN is orbiting? (Between the objects they are orbiting - not between YOU and SAN.)

*/

namespace Day06
{
    class Program
    {
        static Dictionary<string, string> nodes;

        private Program(string inputFile, bool part1)
        {
            var elements = ReadProgram(inputFile);
            InitNodes();
            ParseLayout(elements);
            if (part1)
            {
                var result1 = TotalOrbits();
                Console.WriteLine($"Day06 : Result1 {result1}");
            }
            else
            {
                var result2 = NumTransfers("YOU", "SAN");
                Console.WriteLine($"Day06 : Result2 {result2}");
            }
        }

        private string[] ReadProgram(string inputFile)
        {
            var elements = File.ReadAllLines(inputFile);
            return elements;
        }

        public static void InitNodes()
        {
            nodes = new Dictionary<string, string>();
        }

        public static void ParseLayout(string[] elements)
        {
            InitNodes();
            foreach (var element in elements)
            {
                var tokens = element.Split(')');
                var parent = tokens[0];
                var node = tokens[1];
                AddNode(node, parent);
            }
            VerifyNodes();
        }

        public static void VerifyNodes()
        {
            foreach (var node in nodes)
            {
                var parent = node.Value;
                if (!String.IsNullOrEmpty(parent) && (parent != "COM") && !nodes.ContainsKey(parent))
                {
                    throw new ArgumentOutOfRangeException("parent", $"Parent does not exist {parent}");
                }
            }
        }

        public static void AddNode(string node, string parent)
        {
            if (nodes.ContainsKey(node))
            {
                throw new ArgumentOutOfRangeException("node", $"Node already exists {node}");
            }
            nodes.Add(node, parent);
        }

        public static string GetParent(string node)
        {
            if (!nodes.ContainsKey(node))
            {
                throw new ArgumentOutOfRangeException("node", $"Node does not exist {node}");
            }
            return nodes[node];
        }

        public static int TotalOrbits()
        {
            return DirectOrbits() + InDirectOrbits();
        }

        public static int DirectOrbits()
        {
            return nodes.Count;
        }

        public static int NumTransfers(string start, string end)
        {
            var minTransferCount = int.MaxValue;
            var startParentCount = 0;
            var startParents = GetParents(start);
            var endParents = GetParents(end);
            foreach (var startParent in startParents)
            {
                var transferCount = startParentCount;
                foreach (var endParent in endParents)
                {
                    if (endParent == startParent)
                    {
                        if (transferCount < minTransferCount)
                        {
                            minTransferCount = transferCount;
                        }
                        break;
                    }
                    ++transferCount;
                }
                ++startParentCount;
            }
            return minTransferCount;
        }

        public static string[] GetParents(string node)
        {
            List<string> parents = new List<string>();
            while (node != "COM")
            {
                var parent = GetParent(node);
                parents.Add(parent);
                node = parent;
            }
            return parents.ToArray();
        }

        public static int InDirectOrbits()
        {
            int totalDepth = 0;
            foreach (var node in nodes)
            {
                totalDepth += Depth(node.Value);
            }
            return totalDepth;
        }

        static int Depth(string node)
        {
            var depth = 0;
            while (node != "COM")
            {
                var parent = GetParent(node);
                ++depth;
                node = parent;
            }
            return depth;
        }

        public static void Run()
        {
            Console.WriteLine("Day06 : Start");
            _ = new Program("Day06/input.txt", true);
            _ = new Program("Day06/input.txt", false);
            Console.WriteLine("Day06 : End");
        }
    }
}
