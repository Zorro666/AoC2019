using System;
using System.IO;

namespace Day20
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
            Console.WriteLine("Day20 : Start");
            _ = new Program("Day20/input.txt", true);
            _ = new Program("Day20/input.txt", false);
            Console.WriteLine("Day20 : End");
        }
    }
}
