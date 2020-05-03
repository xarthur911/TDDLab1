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

            var firstBudget = GetBudget(start);
            decimal overlappingAmountOfStartBudget = 0m;
            if (firstBudget != null)
            {
                var overlappingEnd = new DateTime(firstBudget.FirstDay().Year, firstBudget.FirstDay().Month, firstBudget.Days());
                var overlappingStart = start;
                var overlappingDays = (overlappingEnd - overlappingStart).Days + 1;
                overlappingAmountOfStartBudget = overlappingDays * firstBudget.DailyAmount();
            }

            var endBudget = GetBudget(end);
            decimal overlappingAmountOfEndBudget = 0m;
            if (endBudget != null)
            {
                var overlappingDays = end.Day;
                overlappingAmountOfEndBudget = overlappingDays * endBudget.DailyAmount();
            }

            var tmpMid = (decimal) 0;
            var diffMonth = end.Year * 12 + end.Month - (start.Year * 12 + start.Month) + 1;
            for (var i = 1; i < diffMonth - 1; i++)
            {
                var currentMonth = start.AddMonths(i);
                var middleBudget = GetBudget(currentMonth);

                int midAmount = 0;
                if (middleBudget != null)
                {
                    // midAmount = middleBudget.Amount;
                    midAmount = middleBudget.Amount;
                }

                tmpMid += midAmount;
            }

            return overlappingAmountOfStartBudget + tmpMid + overlappingAmountOfEndBudget;
        }

        private Budget GetBudget(DateTime queryDate)
        {
            return _budgetRepo.GetAll().FirstOrDefault(i => i.YearMonth == queryDate.ToString("yyyyMM"));
        }
    }
}