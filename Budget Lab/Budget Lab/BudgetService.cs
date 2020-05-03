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
            var budgets = _budgetRepo.GetAll();

            var daysOfStartBudget = DateTime.DaysInMonth(start.Year, start.Month);
            var amountOfStartBudget = budgets
                              .FirstOrDefault(i => i.YearMonth == start.ToString("yyyyMM"))
                              ?.Amount ?? 0;
            decimal dailyAmountOfStart = (decimal)amountOfStartBudget / daysOfStartBudget;
            
            var daysOfEndBudget = DateTime.DaysInMonth(end.Year, end.Month);
            var amountOfEndBudget = budgets
                            .FirstOrDefault(i => i.YearMonth == end.ToString("yyyyMM"))
                            ?.Amount ?? 0;
            decimal dailyAmountOfEnd = (decimal)amountOfEndBudget / daysOfEndBudget;

            var intervalDays = end.Subtract(start).TotalDays + 1;
            if (intervalDays < 1)
                //// end < start
            {
                return 0;
            }

            //// 當天
            if (intervalDays == 1)
            {
                return dailyAmountOfStart;
            }

            var diffMonth = end.Year * 12 + end.Month - (start.Year * 12 + start.Month) + 1;
            if (diffMonth < 2)
            {
                //// 當月超過1日
                return (decimal) (intervalDays) * dailyAmountOfStart;
            }
            else
            {
                var s = (daysOfStartBudget - start.Day + 1) * dailyAmountOfStart;
                var e = end.Day * dailyAmountOfEnd;
                var tmpMid = (decimal) 0;
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
    }
}