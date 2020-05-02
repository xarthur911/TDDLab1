using System;
using System.Runtime.InteropServices;

namespace Budget_Lab
{
    public class BudgetService
    {
        public decimal Query(DateTime start, DateTime end)
        {
            var dateDiffDays = start.Date.CompareTo(end.Date);

            //// 當天
            if (dateDiffDays == 0)
            {
                int monthDays =
            }

            return 0;
        }
    }
}