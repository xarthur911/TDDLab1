using System.Collections.Generic;

namespace Budget_Lab
{
    interface IBudgetRepo
    {
        List<Budget> GetAll();
    }

    internal class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }
    }
}
