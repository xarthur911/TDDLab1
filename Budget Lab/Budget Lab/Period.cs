using System;

namespace Budget_Lab
{
    public class Period
    {
        public Period(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; private set; }
        public DateTime End   { get; private set; }

        public int OverlappingDays(Period anotherPeriod)
        {
            DateTime overlappingEnd = anotherPeriod.End < End
                ? anotherPeriod.End
                : End;

            DateTime overlappingStart = anotherPeriod.Start > Start
                ? anotherPeriod.Start
                : Start;

            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }
}