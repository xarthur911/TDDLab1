#region

using System;
using System.Collections.Generic;

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

            var period = new Period(start, end);
            var result = (decimal) 0;
            foreach (var currentBudget in _budgetRepo.GetAll())
            {
                result += currentBudget.OverlappingAmount(period);
            }

            return result;
        }
    }
}