using System;
using System.IO;

namespace Day23
{
    class Program
    {
        private Program(string inputFile, bool part1)
        {
            var elements = ReadProgram(inputFile);
        }

        private string[] ReadProgram(string inputFile)
        {
            var elements = File.ReadAllLines(inputFile);
            return elements;
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
