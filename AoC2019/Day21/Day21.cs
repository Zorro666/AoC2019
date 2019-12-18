using System;
using System.IO;

namespace Day21
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
            Console.WriteLine("Day21 : Start");
            _ = new Program("Day21/input.txt", true);
            _ = new Program("Day21/input.txt", false);
            Console.WriteLine("Day21 : End");
        }
    }
}
