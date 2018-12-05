using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _05
{
    class Program
    {
        private static bool IsCrushablePair(char a, char b)
        {
            return Math.Abs(a - b) == 32;
        }

        private static string CrushString(string given)
        {
            var letterStack = new Stack<char>();

            foreach (var letter in given)
            {
                if (letterStack.Any() && IsCrushablePair(letter, letterStack.Peek()))
                {
                    letterStack.Pop();
                } 
                else 
                {
                    letterStack.Push(letter);
                }
            }

            return string.Concat(letterStack);
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt").First();

            var part1Result = CrushString(input);

            Console.WriteLine($"{part1Result.Length} units remain after reaction 1");

            var shortestVariantLength = Enumerable
                .Range('a', 26)
                .Select(ch => (char)ch)
                .AsParallel() // Not necessarily faster
                .Select(x => CrushString(string.Concat(input.Where(y => char.ToUpperInvariant(y) != char.ToUpperInvariant(x)))))
                .Min(candidate => candidate.Length);

            Console.WriteLine($"{shortestVariantLength} units remain after reaction 2");
        }
    }
}
