using System;
using System.Linq;
using System.Runtime.InteropServices;

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
            var allAmount = this._budgetRepo.GetAll();
            var diffDays = end.Subtract(start).TotalDays;

            //// end < start
            if (diffDays < 0)
            {
                return 0;
            }

            //// 當天
            if (diffDays == 0)
            {
                var monthDays = DateTime.DaysInMonth(start.Year, start.Month);
                var amount = allAmount
                    .FirstOrDefault(i => i.YearMonth == start.ToString("yyyyMM"))
                    ?.Amount??0;
                return amount / monthDays;
            }

            //// 當月超過1日
            if()

            return 0;
        }

    }
}