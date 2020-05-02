#region

using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using Xunit;

#endregion

namespace Budget_Lab
{
    public class BudgetServiceTest
    {
        private readonly IBudgetRepo _budgetRepo;
        private readonly BudgetService _budgetService;

        public BudgetServiceTest()
        {
            _budgetRepo = Substitute.For<IBudgetRepo>();
            _budgetService = new BudgetService(_budgetRepo);
        }

        [Fact]
        public void end小於start()
        {
            GivenBudgets(new Budget {YearMonth = "202004", Amount = 30000});
            QueryAmountShouldBe(0,
             new DateTime(2020, 4, 01),
              new DateTime(2020, 3, 01));
        }

        private void QueryAmountShouldBe(int expected, DateTime start, DateTime end)
        {
            Assert.Equal(expected, _budgetService.Query(start, end));
        }

        private void GivenBudgets(params Budget[] budgets)
        {
            _budgetRepo.GetAll().Returns(budgets.ToList());
        }

        [Fact]
        public void 當天()
        {
            var start = new DateTime(2020, 4, 01);
            var end = new DateTime(2020, 4, 01);
            _budgetRepo.GetAll().Returns(new List<Budget>
                                         {
                                             new Budget {YearMonth = "202004", Amount = 30000}
                                         });
            var r = _budgetService.Query(start, end);
            Assert.Equal(1000, r);
        }

        [Fact]
        public void 當月N天()
        {
            var start = new DateTime(2020, 4, 01);
            var end = new DateTime(2020, 4, 10);
            _budgetRepo.GetAll().Returns(new List<Budget>
                                         {
                                             new Budget {YearMonth = "202004", Amount = 30000}
                                         });
            var r = _budgetService.Query(start, end);
            Assert.Equal(10000, r);
        }

        [Fact]
        public void 跨三月()
        {
            var start = new DateTime(2020, 4, 28);
            var end = new DateTime(2020, 6, 2);
            _budgetRepo.GetAll().Returns(new List<Budget>
                                         {
                                             new Budget {YearMonth = "202004", Amount = 30000},
                                             new Budget {YearMonth = "202005", Amount = 3100},
                                             new Budget {YearMonth = "202006", Amount = 300}
                                         });
            var r = _budgetService.Query(start, end);
            Assert.Equal(6120, r);
        }

        [Fact]
        public void 跨兩月()
        {
            var start = new DateTime(2020, 4, 28);
            var end = new DateTime(2020, 5, 2);
            _budgetRepo.GetAll().Returns(new List<Budget>
                                         {
                                             new Budget {YearMonth = "202004", Amount = 30000},
                                             new Budget {YearMonth = "202005", Amount = 3100}
                                         });
            var r = _budgetService.Query(start, end);
            Assert.Equal(3200, r);
        }
    }
}