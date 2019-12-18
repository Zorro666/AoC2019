using System;
using System.IO;

namespace Day25
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
            Console.WriteLine("Day25 : Start");
            _ = new Program("Day25/input.txt", true);
            _ = new Program("Day25/input.txt", false);
            Console.WriteLine("Day25 : End");
        }
    }
}
