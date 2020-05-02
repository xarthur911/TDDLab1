using System;
using System.Collections.Generic;
using NSubstitute;
using NSubstitute.Core;
using Xunit;


namespace Budget_Lab
{
    public class BudgetServiceTest
    {
        private IBudgetRepo _budgetRepo;

        public BudgetServiceTest()
        {
            this._budgetRepo = Substitute.For<IBudgetRepo>();
        }

        [Fact]
        public void 一天()
        {
            var budgetService = new BudgetService(this._budgetRepo);
            var start = new DateTime(2020, 4, 01);
            var end = new DateTime(2020, 4, 01);
            this._budgetRepo.GetAll().Returns(new List<Budget>()
            {
                new Budget() {YearMonth = "202004",Amount = 30000}
            });
            var r =  budgetService.Query(start,end);
            Assert.Equal(1000,r);
        }

        [Fact]
        public void end小於start()
        {
            var budgetService = new BudgetService(this._budgetRepo);
            var start = new DateTime(2020, 4, 01);
            var end = new DateTime(2020, 3, 01);
            this._budgetRepo.GetAll().Returns(new List<Budget>()
            {
                new Budget() {YearMonth = "202004",Amount = 30000}
            });
            var r = budgetService.Query(start, end);
            Assert.Equal(0, r);
        }

        [Fact]
        public void 當月N天()
        {
            var budgetService = new BudgetService(this._budgetRepo);
            var start = new DateTime(2020, 4, 01);
            var end = new DateTime(2020, 4, 10);
            this._budgetRepo.GetAll().Returns(new List<Budget>()
            {
                new Budget() {YearMonth = "202004",Amount = 30000}
            });
            var r = budgetService.Query(start, end);
            Assert.Equal(10000, r);
        }

        [Fact]
        public void 跨兩月()
        {
            var budgetService = new BudgetService(this._budgetRepo);
            var start = new DateTime(2020, 4, 28);
            var end = new DateTime(2020, 5, 2);
            this._budgetRepo.GetAll().Returns(new List<Budget>()
            {
                new Budget() {YearMonth = "202004",Amount = 30000},
                new Budget() {YearMonth = "202005",Amount = 3100}
            });
            var r = budgetService.Query(start, end);
            Assert.Equal(3200, r);
        }

        [Fact]
        public void 跨三月()
        {
            var budgetService = new BudgetService(this._budgetRepo);
            var start = new DateTime(2020, 4, 28);
            var end = new DateTime(2020, 6, 2);
            this._budgetRepo.GetAll().Returns(new List<Budget>()
            {
                new Budget() {YearMonth = "202004",Amount = 30000},
                new Budget() {YearMonth = "202005",Amount = 3100},
                new Budget() {YearMonth = "202006",Amount = 300}
            });
            var r = budgetService.Query(start, end);
            Assert.Equal(6120, r);
        }
    }
}
