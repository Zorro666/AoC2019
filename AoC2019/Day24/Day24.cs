using System;
using System.IO;

namespace Day24
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
            Console.WriteLine("Day24 : Start");
            _ = new Program("Day24/input.txt", true);
            _ = new Program("Day24/input.txt", false);
            Console.WriteLine("Day24 : End");
        }
    }
}
