using System;
using System.Linq;
using System.IO;

namespace _03
{
    class Claim
    {
        public string Id {get;set;}
        public int X {get;set;}
        public int Y {get;set;}
        public int Width {get;set;}
        public int Height {get;set;}

        public Claim(string givenString)
        {
            // Parsing is a bit horrible
            var components = givenString.Split();
            this.Id = components[0];

            var xAndY = components[2].Split(new char[] {',', ':'}, StringSplitOptions.RemoveEmptyEntries);
            this.X = int.Parse(xAndY[0]);
            this.Y = int.Parse(xAndY[1]);

            var widthAndHeight = components[3].Split("x");
            Width = int.Parse(widthAndHeight[0]);
            Height = int.Parse(widthAndHeight[1]);
        }

        public bool HasNoOverlaps(int[,] givenFabric)
        {
            for (int x = this.X; x < this.X + this.Width; x++)
                for (int y = this.Y; y < this.Y + this.Height; y++)
                    if (givenFabric[x, y] > 1)
                        return false;

            return true;
        }
    }

    class Program
    {
        private const int FABRIC_SIZE = 1000;

        static void Main(string[] args)
        {
            // Part 1
            var claims = File.ReadAllLines("input.txt").Select(x => new Claim(x)).ToList();
            var fabric = new int[FABRIC_SIZE, FABRIC_SIZE];

            foreach (var claim in claims)
                for (int x = claim.X; x < claim.X + claim.Width; x++)
                    for (int y = claim.Y; y < claim.Y + claim.Height; y++)
                        fabric[x, y]++;

            var numberOfOverlapping = fabric.Cast<int>().Count(x => x > 1);
            Console.WriteLine($"There are {numberOfOverlapping} rectangles with overlaps");

            // Part 2
            // Could be more efficient (paint ID onto fabric, remove as overlaps are found)
            var validClaim = claims.Single(x => x.HasNoOverlaps(fabric));
            Console.WriteLine($"Only {validClaim.Id} has no overlapping segments");
        }
    }
}
