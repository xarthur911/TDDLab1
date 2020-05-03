#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Budget_Lab
{
    public class BudgetService
    {
        private readonly IBudgetRepo _budgetRepo;

        public BudgetService(IBudgetRepo budgetRepo)
        {
            _budgetRepo = budgetRepo;
        }

        public decimal Query(DateTime start, DateTime end)
        {
            if (end < start) return 0;

            if (start.ToString("yyyyMM") == end.ToString("yyyyMM"))
            {
                var intervalDays = (end - start).Days + 1;
                var budget = GetBudget(start);
                if (budget != null)
                {
                    return intervalDays * budget.DailyAmount();
                }

                return 0;
            }

            var tmpMid = (decimal) 0;
            var diffMonth = end.Year * 12 + end.Month - (start.Year * 12 + start.Month) + 1;
            for (var i = 0; i < diffMonth; i++)
            {
                var currentMonth = start.AddMonths(i);
                var currentBudget = GetBudget(currentMonth);

                var midAmount = 0m;
                if (currentBudget != null)
                {
                    var overlappingDays = OverlappingDays(new Period(start, end), currentBudget);
                    midAmount = overlappingDays * currentBudget.DailyAmount();
                }

                tmpMid += midAmount;
            }

            return tmpMid;
        }

        private static int OverlappingDays(Period period, Budget currentBudget)
        {
            DateTime overlappingEnd;
            DateTime overlappingStart;
            if (currentBudget.YearMonth == period.Start.ToString("yyyyMM"))
            {
                overlappingEnd = currentBudget.LastDay();
                overlappingStart = period.Start;
            }
            else if (currentBudget.YearMonth == period.End.ToString("yyyyMM"))
            {
                overlappingEnd = period.End;
                overlappingStart = currentBudget.FirstDay();
            }
            else
            {
                overlappingEnd = currentBudget.LastDay();
                overlappingStart = currentBudget.FirstDay();
            }

            var overlappingDays = (overlappingEnd - overlappingStart).Days + 1;
            return overlappingDays;
        }

        private Budget GetBudget(DateTime queryDate)
        {
            return _budgetRepo.GetAll().FirstOrDefault(i => i.YearMonth == queryDate.ToString("yyyyMM"));
        }
    }
}