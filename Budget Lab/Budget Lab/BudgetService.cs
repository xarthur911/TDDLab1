using System;
using System.CodeDom;
using System.Globalization;
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
            var budgets = this._budgetRepo.GetAll();
            var diffDays = (end - start).TotalDays + 1;
            var diffMonth = end.Year * 12 + end.Month - (start.Year * 12 + start.Month);
            var startMonthDays = DateTime.DaysInMonth(start.Year, start.Month);
            var endMonthDays = DateTime.DaysInMonth(end.Year, end.Month);
            var startAmount = budgets
                .FirstOrDefault(i => i.YearMonth == start.ToString("yyyyMM"))
                ?.Amount ?? 0;
            var endAmount = budgets
                .FirstOrDefault(i => i.YearMonth == end.ToString("yyyyMM"))
                ?.Amount ?? 0;

            decimal startOneDay = startAmount / startMonthDays;
            decimal endOneDay = endAmount / endMonthDays;

            if (end < start)
            {
                return 0;
            }

            //// 一天
            if (diffDays == 1)
            {
                return startOneDay;
            }

            if (diffMonth<1)
            {
                //// 當月超過1日
                return (decimal)(diffDays) * startOneDay;
            }
            else
            {
                var startMonthAmount = (startMonthDays - start.Day + 1) * startOneDay;
                var endMonthAmount = end.Day * endOneDay;
                var tmpMid = 0m;
                for (var i = 1; i < diffMonth; i++)
                {
                    var midAmount = budgets
                        .FirstOrDefault(j => j.YearMonth == start.AddMonths(i).ToString("yyyyMM"))
                        ?.Amount ?? 0;
                    tmpMid += midAmount;
                }

                return startMonthAmount + tmpMid + endMonthAmount;
            }
        }

    }
}