using System;
using System.Collections.Generic;
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
            var totalDays = TotalDays(start, end);
            var totalMonth = TotalMonth(start, end);
            var startMonthDays = MonthDays(start);
            var endMonthDays = MonthDays(end);
            var startOneDayAmount = OneDayAmount(start, budgets, startMonthDays);
            var endOneDayAmount = OneDayAmount(end, budgets, endMonthDays);

            if (end < start)
            {
                return 0;
            }

            //// 一天
            if (totalDays == 1)
            {
                return startOneDayAmount;
            }

            if (totalMonth < 1)
            {
                //// 當月超過1日
                return (decimal)(totalDays) * startOneDayAmount;
            }
            else
            {
                var startMonthAmount = (startMonthDays - start.Day + 1) * startOneDayAmount;
                var endMonthAmount = end.Day * endOneDayAmount;
                var midMonthAmount = MidMonthAmount(start, totalMonth, budgets);

                return startMonthAmount + midMonthAmount + endMonthAmount;
            }
        }

        private static decimal MidMonthAmount(DateTime start, int totalMonth, List<Budget> budgets)
        {
            var midMonthAmount = 0m;
            for (var i = 1; i < totalMonth; i++)
            {
                var midAmount = budgets
                    .FirstOrDefault(j => j.YearMonth == start.AddMonths(i).ToString("yyyyMM"))
                    ?.Amount ?? 0;
                midMonthAmount += midAmount;
            }

            return midMonthAmount;
        }

        private static int MonthDays(DateTime start)
        {
            return DateTime.DaysInMonth(start.Year, start.Month);
        }

        private static double TotalDays(DateTime start, DateTime end)
        {
            return (end - start).TotalDays + 1;
        }

        private static decimal OneDayAmount(DateTime start, List<Budget> budgets, int startMonthDays)
        {
            return MonthAmount(start, budgets) / startMonthDays;
        }

        private static int TotalMonth(DateTime start, DateTime end)
        {
            return end.Year * 12 + end.Month - (start.Year * 12 + start.Month);
        }

        private static int MonthAmount(DateTime start, List<Budget> budgets)
        {
            return budgets
                .FirstOrDefault(i => i.YearMonth == start.ToString("yyyyMM"))
                ?.Amount ?? 0;
        }
    }
}