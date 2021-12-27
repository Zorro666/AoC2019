using System;
using System.Diagnostics;

namespace AoC2019
{
    class Program
    {
        static bool runMulti = false;
        static void RunDay(int requestedDay, int thisDay, Action function)
        {
            if ((requestedDay == -1) || (requestedDay == thisDay) || (runMulti && thisDay >= requestedDay))
            {
                var watch = Stopwatch.StartNew();
                function();
                watch.Stop();
                var milliSeconds = watch.ElapsedMilliseconds;
                float seconds = milliSeconds / 1000.0f;
                float minutes = seconds / 60.0f;
                Console.WriteLine($"Elapsed {milliSeconds}ms {seconds}s {minutes}mins");
            }
        }
        static void Main(string[] args)
        {
            var day = -1;
            runMulti = false;
            if (args.Length == 1)
            {
                runMulti = args[0].EndsWith("+");
                day = int.Parse(args[0].TrimEnd('+'));
            }
            RunDay(day, 1, Day01.Program.Run);
            RunDay(day, 2, Day02.Program.Run);
            RunDay(day, 3, Day03.Program.Run);
            RunDay(day, 4, Day04.Program.Run);
            RunDay(day, 5, Day05.Program.Run);
            RunDay(day, 6, Day06.Program.Run);
            RunDay(day, 7, Day07.Program.Run);
            RunDay(day, 8, Day08.Program.Run);
            RunDay(day, 9, Day09.Program.Run);
            RunDay(day, 10, Day10.Program.Run);
            RunDay(day, 11, Day11.Program.Run);
            RunDay(day, 12, Day12.Program.Run);
            RunDay(day, 13, Day13.Program.Run);
            RunDay(day, 14, Day14.Program.Run);
            RunDay(day, 15, Day15.Program.Run);
            RunDay(day, 16, Day16.Program.Run);
            RunDay(day, 17, Day17.Program.Run);
            RunDay(day, 18, Day18.Program.Run);
            RunDay(day, 19, Day19.Program.Run);
            RunDay(day, 20, Day20.Program.Run);
            RunDay(day, 21, Day21.Program.Run);
            RunDay(day, 22, Day22.Program.Run);
            RunDay(day, 23, Day23.Program.Run);
            RunDay(day, 24, Day24.Program.Run);
            RunDay(day, 25, Day25.Program.Run);
        }
    }
}
