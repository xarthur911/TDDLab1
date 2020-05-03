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

        public int OverlappingDays(Budget currentBudget)
        {
            DateTime overlappingEnd = currentBudget.LastDay() < End
                ? currentBudget.LastDay()
                : End;

            DateTime overlappingStart = currentBudget.FirstDay() > Start
                ? currentBudget.FirstDay()
                : Start;

            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }
}