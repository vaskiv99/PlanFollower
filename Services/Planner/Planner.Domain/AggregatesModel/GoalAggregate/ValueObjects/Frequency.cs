using BuildingBlocks.Domain.ValueObject;
using Planner.Domain.AggregatesModel.GoalAggregate.Enums;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Planner.Domain.AggregatesModel.GoalAggregate.ValueObjects
{
    public class Frequency : ValueObject
    {
        private const string Format = "{0}_TIMES_PER_{1}_{2}";
        private const string RegexFormat = @"^(\d)_TIMES_PER_(\d)_([a-zA-Z]+$)";

        public TimePeriod TimePeriod { get; set; }

        public int CountOfPeriods { get; set; }

        public int CountOfTimes { get; set; }

        public Frequency(TimePeriod timePeriod, int countOfPeriods = 1, int countOfTimesPerPeriod = 1)
        {
            TimePeriod = timePeriod;
            CountOfPeriods = countOfPeriods;
            CountOfTimes = countOfTimesPerPeriod;
        }

        public Frequency() { }

        public Frequency(string frequencyString)
        {
            var regex = new Regex(RegexFormat, RegexOptions.IgnoreCase);

            if (!regex.IsMatch(frequencyString))
                throw new ArgumentException($"{nameof(frequencyString)} is invariant.");

            var matched = regex.Matches(frequencyString);
            CountOfTimes = Convert.ToInt32(matched[0].Groups[1].Value);
            CountOfPeriods = Convert.ToInt32(matched[0].Groups[2].Value);
            TimePeriod = System.Enum.Parse<TimePeriod>(matched[0].Groups[3].Value, true);
        }

        public override string ToString()
        {
            return string.Format(Format, CountOfTimes, CountOfPeriods, TimePeriod).ToUpperInvariant();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TimePeriod;
            yield return CountOfTimes;
            yield return CountOfPeriods;
        }
    }
}