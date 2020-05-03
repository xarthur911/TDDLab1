#region

using System;

#endregion

namespace Budget_Lab
{
    public class Period
    {
        public Period(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        private DateTime Start { get; }
        private DateTime End   { get; }

        public int OverlappingDays(Period anotherPeriod)
        {
            var overlappingEnd = anotherPeriod.End < End
                ? anotherPeriod.End
                : End;

            var overlappingStart = anotherPeriod.Start > Start
                ? anotherPeriod.Start
                : Start;

            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }
}