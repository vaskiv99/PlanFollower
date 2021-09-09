using BuildingBlocks.Domain.ValueObject;
using System;
using System.Collections.Generic;

namespace Planner.Domain.ValueObjects
{
    /// <summary>
    /// Represent date duration
    /// </summary>
    public class Duration : ValueObject
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public Duration(DateTime start, DateTime end)
        {
            if (start >= end)
            {
                throw new ArgumentException($"{nameof(start)} must be less than {nameof(end)}");
            }

            Start = start.Date;
            End = end.Date;
        }

        public Duration() { }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Start;
            yield return End;
        }

        public bool Contains(DateTime date) => Start <= date && End >= date;

        public bool IsSubDuration(Duration subDuration) => Start <= subDuration.Start && End >= subDuration.End;
    }
}