using System;
using System.IO;
using System.Linq;

namespace _02
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadLines("input.txt");

            var groupings = input.Select(x => x.ToLookup(c => c)).ToList();

            var groupingsWithTwo = groupings.Count(x => x.Any(y => y.Count() == 2));
            var groupingsWithThree = groupings.Count(x => x.Any(y => y.Count() == 3));

            Console.WriteLine(groupingsWithTwo * groupingsWithThree);

            var result = input
            .SelectMany(x => input, (item1, item2) => new {First = item1, Second = item2})
            .First(x => x.First.Length - GetCommonString(x.First, x.Second).Length == 1);

            Console.WriteLine(GetCommonString(result.First, result.Second));
        }

        private static string GetCommonString(string first, string second) => 
            string.Concat(first.Select((c, i) => new {Character = c, Index = i})
            .Where(x => x.Character == second[x.Index]).Select(x => x.Character));
    }
}
