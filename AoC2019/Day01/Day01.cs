using System;
using System.IO;

/*

Fuel required to launch a given module is based on its mass. Specifically, to find the fuel required for a module, take its mass, divide by three, round down, and subtract 2.

For example:

For a mass of 12, divide by 3 and round down to get 4, then subtract 2 to get 2.
For a mass of 14, dividing by 3 and rounding down still yields 4, so the fuel required is also 2.
For a mass of 1969, the fuel required is 654.
For a mass of 100756, the fuel required is 33583.

*/

namespace Day01
{
    class Program
    {
        long[] inputs;

        private Program(string inputFile)
        {
            ReadDataFile(inputFile);
            long total1 = 0;
            long total2 = 0;
            foreach (var input in inputs)
            {
                total1 += ComputeFuel(input);
                total2 += ComputeFuelRecursive(input);
            }
            Console.WriteLine($"Day01 : Result1 {total1}");
            Console.WriteLine($"Day01 : Result2 {total2}");
        }

        private void ReadDataFile(string inputFile)
        {
            var lines = File.ReadAllLines(inputFile);
            inputs = new long[lines.Length];
            var index = 0;
            foreach (var line in lines)
            {
                inputs[index] = Int32.Parse(line);
                index++;
            }
        }

        public static long ComputeFuel(long mass)
        {
            return (mass / 3) - 2;
        }

        public static long ComputeFuelRecursive(long mass)
        {
            long total = 0;
            while (mass > 0)
            {
                var newmass = (mass / 3) - 2;
                if (newmass > 0)
                {
                    total += newmass;
                }
                mass = newmass;
            }
            return total;
        }

        public static void Run()
        {
            Console.WriteLine("Day01 : Start");
            _ = new Program("Day01/input.txt");
            Console.WriteLine("Day01 : End");
        }
    }
}
