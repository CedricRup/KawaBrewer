using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Engine
{
    [TestFixture]
    public class Tests
    {
        [Test]
// ReSharper disable InconsistentNaming


            //One tea is 0,4 euro, a coffee is 0,6 euro, a chocolate is 0,5 euro.
        public void Shoud_be_able_to_order_a_tea_with_a_sugar()
        {
            string result = Glop.Order(Drinks.Tea, 1, 0.4m, false);
            Assert.That(result,Is.EqualTo("T:1:0"));
        }

        [Test]
        public void Shoud_be_able_to_order_a_chocolate_without_sugar()
        {
            string result = Glop.Order(Drinks.Chocolate, 0, 0.5m, false);
            Assert.That(result, Is.EqualTo("H::"));
        }

        [Test]
        public void Should_be_able_to_order_a_coffee_with_two_sugars()
        {
            string result = Glop.Order(Drinks.Coffee, 2, 0.6m, false);
            Assert.That(result, Is.EqualTo("C:2:0"));
        }

        [Test]
        public void Should_send_a_message_when_not_enough_money()
        {
            string result = Glop.Order(Drinks.Coffee, 2, 0.1m, false);
            Assert.That(result, Is.StringStarting("M:"));
            Assert.That(result,Is.StringContaining((0.5).ToString()));
        }

        [Test]
        public void Should_send_a_message_when_not_enough_money_for_tea()
        {
            string result = Glop.Order(Drinks.Tea, 2, 0.2m, false);
            Assert.That(result, Is.StringStarting("M:"));
            Assert.That(result, Is.StringContaining((0.2).ToString()));
        }

        [Test]
        public void Should_order_a_tea_when_too_much_money_for_tea()
        {
            string result = Glop.Order(Drinks.Tea, 2, 0.6m, false);
            Assert.That(result, Is.EqualTo("T:2:0"));
        }

        [Test]
        public void Should_order_orange_juice()
        {
            //"O::" (Drink maker will make one orange juice)
            string result = Glop.Order(Drinks.Orange, 0, 0.6m, false);
            Assert.That(result, Is.EqualTo("O::"));
        }

        [Test]
        public void Should_order_extra_hot_Coffee()
        {
            string result = Glop.Order(Drinks.Coffee,0, 0.6m,true);
            Assert.That(result, Is.EqualTo("Ch::"));
        }

        [Test]
        public void Should_order_extra_hot_Chocolate_with_one_sugar()
        {
            string result = Glop.Order(Drinks.Chocolate, 1, 0.6m, true);
            Assert.That(result, Is.EqualTo("Hh:1:0"));
        }

        [Test]
        public void Should_order_extra_hot_tea_with_two_sugar()
        {
            string result = Glop.Order(Drinks.Tea, 2, 0.6m, true);
            Assert.That(result, Is.EqualTo("Th:2:0"));
        }

        [Test]
        public void Should_be_able_to_report_sales()
        {
            Glop.CleanReport();
            var report = Glop.GetReport();
            for (int i = 0; i < 4; i++)
            {
                Glop.Order(Drinks.Chocolate, 1, 0.5m, false);
            }
            for (int i = 0; i < 3; i++)
            {
                Glop.Order(Drinks.Coffee, 1, 0.6m, false);
            }

            Assert.That(report.SalesPerDrink[Drinks.Chocolate], Is.EqualTo(4));
            Assert.That(report.SalesPerDrink[Drinks.Coffee], Is.EqualTo(3));
            Assert.That(report.TotalMoneyEarned, Is.EqualTo(2.0d + 1.8d));
        }


        
        // ReSharper restore InconsistentNaming

    }

    public enum Drinks
    {
        Chocolate,
        Tea,
        Coffee,
        Orange
    }

    public class Glop
    {
        private static Dictionary<Drinks, decimal> prices = new Dictionary<Drinks, decimal>{
            {
                Drinks.Tea,0.40m
            },
            {
                Drinks.Chocolate, 0.50m
            },
            {
                Drinks.Coffee, 0.60m
            },
            {
                Drinks.Orange, 0.60m
            }
        };

        private static Report report = new Report();

        public static string Order(Drinks drink, int numberOfSugar, decimal inputedMoney, bool isExtraHot)
        {
            decimal amount;
            if ((amount = calculateMoneyMissing(drink, inputedMoney)) > 0)
            {
                return string.Format("M:{0} missing", amount);
            }
            ReportOrderedDrink(drink);

            var str = DrinkToString(drink);
            var sugar = numberOfSugar > 0 ? numberOfSugar.ToString() : string.Empty;
            var touillette = numberOfSugar > 0 ? 0.ToString() : string.Empty;
            var extraHot = isExtraHot ? "h" : string.Empty;
            return string.Format("{0}{3}:{1}:{2}", str, sugar,touillette,extraHot);
            
        }

        public static void CleanReport()
        {
            Glop.report = new Report();
        }

        private static void ReportOrderedDrink(Drinks drink)
        {
            Glop.report.SalesPerDrink[drink]++;
            Glop.report.TotalMoneyEarned += Glop.prices[drink];
        }

        private static decimal calculateMoneyMissing(Drinks drink, decimal inputedMoney)
        {
            var result = Glop.prices[drink] - inputedMoney;

            return result;
        }

        private static string DrinkToString(Drinks drink)
        {
            switch (drink)
            {
                case Drinks.Tea:
                    return "T";
                case Drinks.Chocolate:
                    return "H";
                case Drinks.Coffee:
                    return "C";
                case Drinks.Orange:
                    return "O";
                default:
                    throw new UnknownDrinkException();
            }
        }

        public static Report GetReport()
        {
            return Glop.report;
        }
    }

    public class Report
    {
        public Dictionary<Drinks, int> SalesPerDrink { get; private set; }

        public decimal TotalMoneyEarned { get; set; }

        public Report()
        {
            SalesPerDrink = new Dictionary<Drinks, int>{
                {
                    Drinks.Tea, 0
                },
                {
                    Drinks.Chocolate, 0
                },
                {
                    Drinks.Coffee, 0
                },
                {
                    Drinks.Orange, 0
                }
            };
        }
    }

    internal class UnknownDrinkException : Exception
    {
    }
}
