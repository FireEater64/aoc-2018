using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace _04
{
    class Program
    {
        private class InputLine
        {
            public DateTime TimeStamp {get;set;}
            public string Details {get;set;}

            public InputLine(string given)
            {
                // Some horrible constants in here
                this.TimeStamp = DateTime.Parse(given.Substring(1, 16));

                // Round inputs outside the interest hour to the nearest day
                if (this.TimeStamp.Hour == 23)
                    this.TimeStamp = this.TimeStamp.AddDays(1).Date;

                this.Details = given.Substring(19);
            }
        }

        private class NightDetails
        {
            public string GuardId {get;set;}
            public bool[] SleepOccurances {get;set;}
            public DateTime Date {get;set;}

            public NightDetails(IGrouping<DateTime, InputLine> givenLines)
            {
                var orderedInput = givenLines.OrderBy(x => x.TimeStamp).ToList();

                this.Date = givenLines.Key;
                var guardDetails = orderedInput.Single(x => x.Details.Contains("Guard")).Details;

                this.GuardId = guardDetails.Substring(6, 5).Trim();
                
                this.SleepOccurances = new bool[60];

                var sleepLines = orderedInput.Where(x => !x.Details.Contains("Guard")).ToList();
                sleepLines.Add(new InputLine($"[{this.Date} 01:00] Night ends"));

                // Lazy pairwise
                foreach (var line in sleepLines.Zip(sleepLines.Skip(1), (a, b) => Tuple.Create(a, b)))
                {
                    for (int i = line.Item1.TimeStamp.Minute; i < line.Item2.TimeStamp.Minute; i++)
                    {
                        this.SleepOccurances[i] = line.Item1.Details.Contains("asleep");
                    }
                }
            }

            public int MinutesAsleep { get => this.SleepOccurances.Count(x => x); }
        }

        private static int[] SumNights(IEnumerable<bool[]> givenNights)
        {
            int[] minutes = new int[60];
            foreach (var night in givenNights)
            {
                for (int i = 0; i < 60; i++)
                {
                    minutes[i] += night[i] ? 1 : 0;
                }
            }

            return minutes;
        }

        static void Main(string[] args)
        {
            var guardGrouping = File.ReadAllLines("input.txt")
                .Select(x => new InputLine(x))
                .GroupBy(z => z.TimeStamp.Date)
                .Select(i => new NightDetails(i))
                .GroupBy(night => night.GuardId);

            // Part 1
            var sleepyGuard1 = guardGrouping
                .OrderByDescending(guardNights => guardNights.Sum(night => night.MinutesAsleep))
                .First();

            var guardId = sleepyGuard1.Key;
            Console.WriteLine($"Guard {guardId} was asleep the longest");

            // Get the most commonly asleep minute
            var minutes = SumNights(sleepyGuard1.Select(x => x.SleepOccurances));

            var mostCommonlyAsleepMinute = minutes.ToList().IndexOf(minutes.Max());
            Console.WriteLine($"Most commonly asleep minute for guard {guardId} is {minutes.ToList().IndexOf(minutes.Max())}");
            Console.WriteLine($"Therefore, the result for part 1 is {mostCommonlyAsleepMinute * int.Parse(guardId.Substring(1))}");
            Console.WriteLine();

            // Part 2
            var guardTotalSleepMap = guardGrouping.ToDictionary(x => x.Key, x => SumNights(x.Select(y => y.SleepOccurances)));
            var maxMinutes = guardTotalSleepMap.OrderByDescending(x => x.Value.Max()).First();
            var guardId2 = maxMinutes.Key;
            var longestAsleep = maxMinutes.Value.Max();
            var longestAsleepMinute = maxMinutes.Value.ToList().IndexOf(longestAsleep);

            Console.WriteLine($"Guard {maxMinutes.Key} was asleep {longestAsleep} times in minute {maxMinutes.Value.ToList().IndexOf(longestAsleep)}");
            Console.WriteLine($"Therefore, the result for part 2 is {longestAsleepMinute * int.Parse(guardId2.Substring(1))}");
        }
    }
}
