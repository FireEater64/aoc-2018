using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _01
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputLines = File.ReadLines("input.txt").Select(int.Parse);

            Console.WriteLine($"First round result: {inputLines.Sum()}");

            var seen = new HashSet<int>();
            var result = 0;
            foreach (var item in Enumerable.Repeat(inputLines, 10000).SelectMany(x => x))
            {
                result += item;
                if (seen.Contains(result))
                {
                    Console.WriteLine($"Second round result: {result}");
                    break;
                }

                seen.Add(result);
            }
        }
    }
}
