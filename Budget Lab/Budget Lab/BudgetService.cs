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

            var firstBudget = GetBudget(start);
            var dailyAmountOfStart = 0m;
            var daysOfStartBudget = 0;
            if (firstBudget != null)
            {
                daysOfStartBudget = firstBudget.Days();
                dailyAmountOfStart = firstBudget.DailyAmount();
            }

            var endBudget = GetBudget(end);
            var dailyAmountOfEnd = 0m;
            if (endBudget != null)
            {
                dailyAmountOfEnd = endBudget.DailyAmount();
            }

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

            var s = (daysOfStartBudget - start.Day + 1) * dailyAmountOfStart;
            // var s = (daysOfStartBudget - start.Day + 1) * dailyAmountOfStart;
            var e = end.Day * dailyAmountOfEnd;
            var tmpMid = (decimal) 0;
            var diffMonth = end.Year * 12 + end.Month - (start.Year * 12 + start.Month) + 1;
            for (var i = 1; i < diffMonth - 1; i++)
            {
                var midAmount = _budgetRepo.GetAll()
                                           .FirstOrDefault(j => j.YearMonth == start.AddMonths(i).ToString("yyyyMM"))
                                           ?.Amount ?? 0;
                tmpMid += midAmount;
            }

            return s + tmpMid + e;
        }

        private Budget GetBudget(DateTime queryDate)
        {
            return _budgetRepo.GetAll().FirstOrDefault(i => i.YearMonth == queryDate.ToString("yyyyMM"));
        }
    }
}