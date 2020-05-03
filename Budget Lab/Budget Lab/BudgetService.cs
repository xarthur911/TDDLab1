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

            var period = new Period(start, end);
            var result = (decimal) 0;
            foreach (var currentBudget in _budgetRepo.GetAll())
            {
                result += currentBudget.OverlappingAmount(period);
            }

            return result;
        }

        private Budget GetBudget(DateTime queryDate)
        {
            return _budgetRepo.GetAll().FirstOrDefault(i => i.YearMonth == queryDate.ToString("yyyyMM"));
        }
    }
}