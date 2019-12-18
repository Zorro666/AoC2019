using System;
using System.IO;

namespace Day16
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
            Console.WriteLine("Day16 : Start");
            _ = new Program("Day16/input.txt", true);
            _ = new Program("Day16/input.txt", false);
            Console.WriteLine("Day16 : End");
        }
    }
}
