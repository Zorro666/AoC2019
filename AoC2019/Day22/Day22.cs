using System;
using System.Collections.Generic;
using System.IO;

/*

--- Day 22: Slam Shuffle ---

There isn't much to do while you wait for the droids to repair your ship. At least you're drifting in the right direction.You decide to practice a new card shuffle you've been working on.

Digging through the ship's storage, you find a deck of space cards! Just like any deck of space cards, there are 10007 cards in the deck numbered 0 through 10006. The deck must be new - they're still in factory order, with 0 on the top, then 1, then 2, and so on, all the way through to 10006 on the bottom.

You've been practicing three different techniques that you use while shuffling. Suppose you have a deck of only 10 cards (numbered 0 through 9):

To deal into new stack, create a new stack of cards by dealing the top card of the deck onto the top of the new stack repeatedly until you run out of cards:

Top Bottom
0 1 2 3 4 5 6 7 8 9   Your deck
                      New stack

  1 2 3 4 5 6 7 8 9   Your deck
                  0   New stack

    2 3 4 5 6 7 8 9   Your deck
                1 0   New stack

      3 4 5 6 7 8 9   Your deck
              2 1 0   New stack

Several steps later...

                  9   Your deck
  8 7 6 5 4 3 2 1 0   New stack

                      Your deck
9 8 7 6 5 4 3 2 1 0   New stack
Finally, pick up the new stack you've just created and use it as the deck for the next technique.

To cut N cards, take the top N cards off the top of the deck and move them as a single unit to the bottom of the deck, retaining their order.For example, to cut 3:

Top Bottom
0 1 2 3 4 5 6 7 8 9   Your deck

      3 4 5 6 7 8 9   Your deck
0 1 2                 Cut cards

3 4 5 6 7 8 9         Your deck
              0 1 2   Cut cards

3 4 5 6 7 8 9 0 1 2   Your deck
You've also been getting pretty good at a version of this technique where N is negative! In that case, cut (the absolute value of) N cards from the bottom of the deck onto the top. For example, to cut -4:

Top Bottom
0 1 2 3 4 5 6 7 8 9   Your deck

0 1 2 3 4 5           Your deck
            6 7 8 9   Cut cards

        0 1 2 3 4 5   Your deck
6 7 8 9               Cut cards

6 7 8 9 0 1 2 3 4 5   Your deck
To deal with increment N, start by clearing enough space on your table to lay out all of the cards individually in a long line.Deal the top card into the leftmost position.Then, move N positions to the right and deal the next card there. If you would move into a position past the end of the space on your table, wrap around and keep counting from the leftmost card again. Continue this process until you run out of cards.

For example, to deal with increment 3:


0 1 2 3 4 5 6 7 8 9   Your deck
. . . . . . . . . .   Space on table
^                     Current position

Deal the top card to the current position:

  1 2 3 4 5 6 7 8 9   Your deck
0 . . . . . . . . .   Space on table
^                     Current position

Move the current position right 3:

  1 2 3 4 5 6 7 8 9   Your deck
0 . . . . . . . . .   Space on table
      ^               Current position

Deal the top card:

    2 3 4 5 6 7 8 9   Your deck
0 . . 1 . . . . . .   Space on table
      ^               Current position

Move right 3 and deal:

      3 4 5 6 7 8 9   Your deck
0 . . 1 . . 2 . . .   Space on table
            ^         Current position

Move right 3 and deal:

        4 5 6 7 8 9   Your deck
0 . . 1 . . 2 . . 3   Space on table
                  ^   Current position

Move right 3, wrapping around, and deal:

          5 6 7 8 9   Your deck
0 . 4 1 . . 2 . . 3   Space on table
    ^                 Current position

And so on:

0 7 4 1 8 5 2 9 6 3   Space on table
Positions on the table which already contain cards are still counted; they're not skipped. Of course, this technique is carefully designed so it will never put two cards in the same position or leave a position empty.

Finally, collect the cards on the table so that the leftmost card ends up at the top of your deck, the card to its right ends up just below the top card, and so on, until the rightmost card ends up at the bottom of the deck.

The complete shuffle process(your puzzle input) consists of applying many of these techniques.Here are some examples that combine techniques; they all start with a factory order deck of 10 cards:

deal with increment 7
deal into new stack
deal into new stack
Result: 0 3 6 9 2 5 8 1 4 7
cut 6
deal with increment 7
deal into new stack
Result: 3 0 7 4 1 8 5 2 9 6
deal with increment 7
deal with increment 9
cut -2
Result: 6 3 0 7 4 1 8 5 2 9
deal into new stack
cut -2
deal with increment 7
cut 8
cut -4
deal with increment 7
cut 3
deal with increment 9
deal with increment 3
cut -1
Result: 9 2 5 8 1 4 7 0 3 6
Positions within the deck count from 0 at the top, then 1 for the card immediately below the top card, and so on to the bottom. (That is, cards start in the position matching their number.)

After shuffling your factory order deck of 10007 cards, what is the position of card 2019?

*/
namespace Day22
{
    class Program
    {
        static int sDeckSize;
        static int[] sDeck;
        static int[] sWorkingDeck;

        private Program(string inputFile, bool part1)
        {
            var instructions = ReadProgram(inputFile);
            CreateDeck(10007);

            if (part1)
            {
                RunInstructions(instructions);
                var result = -123;
                for (int i = 0; i < sDeckSize; ++i)
                {
                    if (sDeck[i] == 2019)
                    {
                        result = i;
                    }
                }
                var expected = 6638;
                Console.WriteLine($"Day22: Result1 {result}");
                if (result != expected)
                {
                    throw new InvalidDataException($"Part1 is broken {result} != {expected}");
                }
            }
        }

        private string[] ReadProgram(string inputFile)
        {
            var instructions = File.ReadAllLines(inputFile);
            return instructions;
        }

        public static void CreateDeck(int count)
        {
            sDeckSize = count;
            sDeck = new int[sDeckSize];
            sWorkingDeck = new int[sDeckSize];
            for (var i = 0; i < sDeckSize; ++i)
            {
                sDeck[i] = i;
            }
        }

        public static void RunInstructions(string[] instructions)
        {
            if (instructions == null)
            {
                return;
            }
            foreach (var instruction in instructions)
            {
                CopyDeckIntoWorkingDeck();
                ApplyInstruction(instruction);
            }
        }

        private static void ApplyInstruction(string instruction)
        {
            // "deal with increment XXX",
            if (instruction.StartsWith("deal with increment "))
            {
                string amount = instruction.Split("deal with increment ")[1];
                int incrementAmount = int.Parse(amount);
                DealWithIncrement(incrementAmount);
            }
            // "deal into new stack"
            else if (instruction.StartsWith("deal into new stack"))
            {
                DealIntoNewStack();
            }
            // "cut XXX"
            else if (instruction.StartsWith("cut "))
            {
                string amount = instruction.Split("cut ")[1];
                int cutAmount = int.Parse(amount);
                while (cutAmount < 0)
                {
                    cutAmount += sDeckSize;
                }
                Cut(cutAmount);
            }
            else
            {
                throw new ArgumentException($"Unknown instruction {instruction}", nameof(instruction));
            }
        }

        private static void DealWithIncrement(int incrementAmount)
        {
            int newPosition = 0;
            for (var i = 0; i < sDeckSize; ++i)
            {
                sDeck[newPosition] = sWorkingDeck[i];
                newPosition += incrementAmount;
                newPosition %= sDeckSize;
            }
        }

        private static void DealIntoNewStack()
        {
            for (var i = 0; i < sDeckSize; ++i)
            {
                int newPosition = sDeckSize - 1 - i;
                sDeck[newPosition] = sWorkingDeck[i];
            }
        }

        private static void Cut(int cutAmount)
        {
            for (var i = 0; i < sDeckSize; ++i)
            {
                int newPosition = (i - cutAmount);
                while (newPosition < 0)
                {
                    newPosition += sDeckSize;
                }
                newPosition %= sDeckSize;
                sDeck[newPosition] = sWorkingDeck[i];
            }
        }

        private static void CopyDeckIntoWorkingDeck()
        {
            for (var i = 0; i < sDeckSize; ++i)
            {
                sWorkingDeck[i] = sDeck[i];
            }
        }

        public static string DeckAsString()
        {
            if (sDeckSize < 1)
            {
                throw new InvalidDataException($"Deck Size must be >= 1 {sDeckSize}");
            }
            string result = sDeck[0].ToString();
            for (var i = 1; i < sDeckSize; ++i)
            {
                result += $" {sDeck[i]}";
            }
            return result;
        }

        public static void Run()
        {
            Console.WriteLine("Day22 : Start");
            _ = new Program("Day22/input.txt", true);
            _ = new Program("Day22/input.txt", false);
            Console.WriteLine("Day22 : End");
        }
    }
}
