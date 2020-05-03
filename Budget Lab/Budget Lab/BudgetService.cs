using System;
using System.CodeDom;
using System.Linq;

namespace Budget_Lab
{
    public class BudgetService
    {
        private readonly IBudgetRepo _budgetRepo;

        public BudgetService(IBudgetRepo budgetRepo)
        {
            this._budgetRepo = budgetRepo;
        }

        public decimal Query(DateTime start, DateTime end)
        {
            if (end < start)
            {
                return 0;
            }

            var budgets = _budgetRepo.GetAll();

            var firstBudget = budgets
                .FirstOrDefault(i => i.YearMonth == start.ToString("yyyyMM"));

            int amountOfStartBudget = 0;
            decimal dailyAmountOfStart = 0m;
            int daysOfStartBudget = 0;
            if (firstBudget != null)
            {
                daysOfStartBudget = DaysOfBudget(firstBudget);
                amountOfStartBudget = firstBudget.Amount;
                dailyAmountOfStart = (decimal) amountOfStartBudget / daysOfStartBudget;
            }

            var daysOfEndBudget = DateTime.DaysInMonth(end.Year, end.Month);
            var amountOfEndBudget = budgets
                                    .FirstOrDefault(i => i.YearMonth == end.ToString("yyyyMM"))
                                    ?.Amount ?? 0;
            decimal dailyAmountOfEnd = (decimal) amountOfEndBudget / daysOfEndBudget;

            if (start.ToString("yyyyMM") == end.ToString("yyyyMM"))
            {
                var intervalDays = (end - start).Days + 1;
                return intervalDays * dailyAmountOfStart;
            }
            else
            {
                var s = (daysOfStartBudget - start.Day + 1) * dailyAmountOfStart;
                var e = end.Day * dailyAmountOfEnd;
                var tmpMid = (decimal) 0;
                var diffMonth = end.Year * 12 + end.Month - (start.Year * 12 + start.Month) + 1;
                for (var i = 1; i < diffMonth - 1; i++)
                {
                    var midAmount = budgets
                                    .FirstOrDefault(j => j.YearMonth == start.AddMonths(i).ToString("yyyyMM"))
                                    ?.Amount ?? 0;
                    tmpMid += midAmount;
                }

                return s + tmpMid + e;
            }

            return 0;
        }

        private static int DaysOfBudget(Budget firstBudget)
        {
            var firstDayOfStartBudget = firstBudget.FirstDay();

            var days = DateTime.DaysInMonth(firstDayOfStartBudget.Year, firstDayOfStartBudget.Month);
            return days;
        }
    }
}