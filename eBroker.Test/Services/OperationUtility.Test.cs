using eBroker.Core;
using eBroker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace eBroker.Test
{
    /// <summary>
    /// Operation Utility Test
    /// </summary>
    public class OperationUtilityTest
    {
        /// <summary>
        /// Operation Utility Proxy (System Under Test)
        /// </summary>
        IOperationsUtilityProxy operationUtility;

        /// <summary>
        /// Test Setup
        /// </summary>
        public OperationUtilityTest()
        {
            this.operationUtility = new OperationsUtilityProxy();
        }

        /// <summary>
        /// Valid data for Is Operating Hours
        /// </summary>
        /// <param name="time"></param>
        [Theory]
        [MemberData(nameof(ValidOperatingHours))]
        public void IsOperatingHours_ValidData(DateTime time)
        {
            // ACT
            var result = this.operationUtility.IsOperatingHours(time);

            // ASSERT
            Assert.True(result);
        }

        /// <summary>
        /// Invalid data for Is Operating Hours
        /// </summary>
        /// <param name="time"></param>
        [Theory]
        [MemberData(nameof(InvalidOperatingHours))]
        public void IsOperatingHours_InvalidData(DateTime time)
        {
            // ACT, ASSERT
            Assert.Throws<ArgumentOutOfRangeException>(() => this.operationUtility.IsOperatingHours(time));
        }

        /// <summary>
        /// Increase Funds Success Handler
        /// </summary>
        /// <param name="fund">Existing Fund</param>
        /// <param name="amount">Amount to increase</param>
        /// <param name="expected">Expected new fund</param>
        [Theory]
        [InlineData(20000, 10000, 30000)]
        [InlineData(20000, 100000, 120000)]
        [InlineData(20000, 100001, 119950.9995)]
        [InlineData(20000, 1000000, 1019500)]
        [InlineData(20000, 5, 20005)]
        public void IncreaseFunds_ValidateData(double fund, double amount, double expected)
        {
            // ARRANGE
            var trader = new Trader { ID = 1, Name = "Jitesh", Funds = fund };

            // ACT
            var status = this.operationUtility.IncreaseFunds(trader, amount);

            // ASSERT
            Assert.True(status);
            Assert.Equal(expected, trader.Funds);
        }

        /// <summary>
        /// No Funds for add equities
        /// </summary>
        /// <param name="fund">Available fund</param>
        /// <param name="amount">Amount of equity</param>
        /// <param name="quantity">Quantity of equity</param>
        [Theory]
        [InlineData(20000, 10000, 5)]
        [InlineData(5, 1, 6)]
        public void AddEquities_NoFunds(double fund, double amount, int quantity)
        {
            // ARRANGE
            var trader = new Trader { ID = 1, Name = "Jitesh", Funds = fund, TraderEquities = new List<TraderEquity>() };
            var equity = new Equity { ID = 1, Name = "Sensex", Amount = amount };

            // ACT
            var status = this.operationUtility.AddEquities(trader, equity, quantity);

            // ASSERT
            Assert.False(status);
            Assert.Equal(fund, trader.Funds);
        }

        /// <summary>
        /// Add Equities for non existing equity
        /// </summary>
        /// <param name="fund">Existing fund</param>
        /// <param name="amount">Amount of equity</param>
        /// <param name="quantity">Quantity to add</param>
        /// <param name="newFund">new funds</param>
        [Theory]
        [InlineData(20000, 10000, 2, 0)]
        [InlineData(50000, 10000, 2, 30000)]
        [InlineData(5, 1, 5, 0)]
        public void AddEquities_NonExistingEquity(double fund, double amount, int quantity, double newFund)
        {
            // ARRANGE
            var trader = new Trader { ID = 1, Name = "Jitesh", Funds = fund, TraderEquities = new List<TraderEquity>() };
            var equity = new Equity { ID = 1, Name = "Sensex", Amount = amount };

            // ACT
            var status = this.operationUtility.AddEquities(trader, equity, quantity);

            // ASSERT
            Assert.True(status);
            Assert.Equal(newFund, trader.Funds);
            Assert.Equal("Sensex", trader.TraderEquities.First().Equity.Name);
        }

        /// <summary>
        /// Add Equities to existing list
        /// </summary>
        /// <param name="fund">Existing funds</param>
        /// <param name="amount">Amount of equity</param>
        /// <param name="quantity">Quantity of equity</param>
        /// <param name="newFund">New fund</param>
        /// <param name="oldquantity">Old quantity of equity</param>
        [Theory]
        [InlineData(20000, 10000, 2, 0, 3)]
        [InlineData(50000, 10000, 2, 30000, 2)]
        [InlineData(5, 1, 5, 0, 4)]
        public void AddEquities_ExistingEquity(double fund, double amount, int quantity, double newFund, int oldquantity)
        {
            // ARRANGE
            var trader = new Trader
            {
                ID = 1,
                Name = "Jitesh",
                Funds = fund,
                TraderEquities = new List<TraderEquity>()
            {
                new TraderEquity
                {
                    ID = 1,
                    EquityId = 1,
                    Quantity = oldquantity,
                    TraderId = 1,
                    Equity = new Equity
                    {
                        ID = 1,
                        Name =
                        "Sensex",
                        Amount = amount
                    }
                }
            }
            };

            var equity = new Equity { ID = 1, Name = "Sensex", Amount = amount };

            // ACT
            var status = this.operationUtility.AddEquities(trader, equity, quantity);

            // ASSERT
            Assert.True(status);
            Assert.Equal(newFund, trader.Funds);
            Assert.Equal("Sensex", trader.TraderEquities.First().Equity.Name);
            Assert.Equal(oldquantity + quantity, trader.TraderEquities.First().Quantity);
        }

        /// <summary>
        /// Reduce Equities when no quantity
        /// </summary>
        /// <param name="fund">Existing fund</param>
        /// <param name="amount">Amount of equity</param>
        /// <param name="quantity">Quantity of equity</param>
        /// <param name="oldQuantity">old quantity of equity</param>
        [Theory]
        [InlineData(20000, 10000, 5, 3)]
        [InlineData(5, 1, 6, 3)]
        public void ReduceEquities_NoQuantity(double fund, double amount, int quantity, int oldQuantity)
        {
            // ARRANGE
            var trader = new Trader
            {
                ID = 1,
                Name = "Jitesh",
                Funds = fund,
                TraderEquities = new List<TraderEquity>()
                {
                    new TraderEquity
                    {
                        ID = 1,
                        EquityId = 1,
                        Quantity = oldQuantity,
                        TraderId = 1,
                        Equity = new Equity
                        {
                            ID = 1,
                            Name = "Sensex",
                            Amount = amount
                        }
                    }
                }
            };

            // ACT
            var status = this.operationUtility.ReduceEquities(trader, 1, quantity);

            // ASSERT
            Assert.False(status);
            Assert.Equal(fund, trader.Funds);
        }

        /// <summary>
        /// Success Handler for Reduce Equities
        /// </summary>
        /// <param name="fund">Existing fund</param>
        /// <param name="amount">Amount of equity</param>
        /// <param name="quantity">Quantity of equity</param>
        /// <param name="oldQuantity">Old Quantity</param>
        /// <param name="newFund">New funds</param>
        [Theory]
        [InlineData(20000, 10000, 5, 5, 69975)]
        [InlineData(5, 1, 6, 8, 0)]
        public void ReduceEquities_Success(double fund, double amount, int quantity, int oldQuantity, double newFund)
        {
            // ARRANGE
            var trader = new Trader
            {
                ID = 1,
                Name = "Jitesh",
                Funds = fund,
                TraderEquities = new List<TraderEquity>()
                {
                    new TraderEquity
                    {
                        ID = 1,
                        EquityId = 1,
                        Quantity = oldQuantity,
                        TraderId = 1,
                        Equity = new Equity
                        {
                            ID = 1,
                            Name = "Sensex",
                            Amount = amount
                        }
                    }
                }
            };

            // ACT
            var status = this.operationUtility.ReduceEquities(trader, 1, quantity);

            // ASSERT
            Assert.True(status);
            Assert.Equal(newFund, trader.Funds);
            Assert.Equal(oldQuantity - quantity, trader.TraderEquities.First().Quantity);
        }

        /// <summary>
        /// Valid operating hours data
        /// </summary>
        public static IEnumerable<object[]> ValidOperatingHours =>
            new List<object[]>
            {
            new object[] { new DateTime(2021, 12, 18, 9, 0, 0) },
            new object[] { new DateTime(2021, 12, 18, 14, 59, 0) },
            new object[] { new DateTime(2021, 12, 18, 9, 0, 1) },
            new object[] { new DateTime(2021, 12, 18, 11, 0, 0) },
            };

        /// <summary>
        /// Invalid operating hours data
        /// </summary>
        public static IEnumerable<object[]> InvalidOperatingHours =>
            new List<object[]>
            {
            new object[] { new DateTime(2021, 12, 18, 8, 59, 59) },
            new object[] { new DateTime(2021, 12, 18, 15, 0, 0) },
            new object[] { new DateTime(2021, 12, 18, 8, 0, 0) },
            new object[] { new DateTime(2021, 12, 18, 16, 0, 0) },
            };
    }
}
