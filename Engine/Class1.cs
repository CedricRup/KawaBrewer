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
            string result = Glop.Order(Drinks.Tea, 1, 0.4, false);
            Assert.That(result,Is.EqualTo("T:1:0"));
        }

        [Test]
        public void Shoud_be_able_to_order_a_chocolate_without_sugar()
        {
            string result = Glop.Order(Drinks.Chocolate, 0, 0.5, false);
            Assert.That(result, Is.EqualTo("H::"));
        }

        [Test]
        public void Should_be_able_to_order_a_coffee_with_two_sugars()
        {
            string result = Glop.Order(Drinks.Coffee, 2, 0.6, false);
            Assert.That(result, Is.EqualTo("C:2:0"));
        }

        [Test]
        public void Should_send_a_message_when_not_enough_money()
        {
            string result = Glop.Order(Drinks.Coffee, 2, 0.1, false);
            Assert.That(result, Is.StringStarting("M:"));
            Assert.That(result,Is.StringContaining((0.5).ToString()));
        }

        [Test]
        public void Should_send_a_message_when_not_enough_money_for_tea()
        {
            string result = Glop.Order(Drinks.Tea, 2, 0.2, false);
            Assert.That(result, Is.StringStarting("M:"));
            Assert.That(result, Is.StringContaining((0.2).ToString()));
        }

        [Test]
        public void Should_order_a_tea_when_too_much_money_for_tea()
        {
            string result = Glop.Order(Drinks.Tea, 2, 0.6, false);
            Assert.That(result, Is.EqualTo("T:2:0"));
        }

        [Test]
        public void Should_order_orange_juice()
        {
            //"O::" (Drink maker will make one orange juice)
            string result = Glop.Order(Drinks.Orange, 0, 0.6, false);
            Assert.That(result, Is.EqualTo("O::"));
        }

        [Test]
        public void Should_order_extra_hot_Coffee()
        {
            string result = Glop.Order(Drinks.Coffee,0, 0.6,true);
            Assert.That(result, Is.EqualTo("Ch::"));
        }

        [Test]
        public void Should_order_extra_hot_Chocolate_with_one_sugar()
        {
            string result = Glop.Order(Drinks.Chocolate, 1, 0.6, true);
            Assert.That(result, Is.EqualTo("Hh:1:0"));
        }

        [Test]
        public void Should_order_extra_hot_tea_with_two_sugar()
        {
            string result = Glop.Order(Drinks.Tea, 2, 0.6, true);
            Assert.That(result, Is.EqualTo("Th:2:0"));
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
        public static string Order(Drinks drink, int numberOfSugar, double inputedMoney, bool isExtraHot)
        {
            double amount;
            if ((amount = calculateMoneyMissing(drink, inputedMoney)) > 0)
            {
                return string.Format("M:{0} missing", amount);
            }
            var str = DrinkToString(drink);
            var sugar = numberOfSugar > 0 ? numberOfSugar.ToString() : string.Empty;
            var touillette = numberOfSugar > 0 ? 0.ToString() : string.Empty;
            var extraHot = isExtraHot ? "h" : string.Empty;
            return string.Format("{0}{3}:{1}:{2}", str, sugar,touillette,extraHot);
            
        }

        private static double calculateMoneyMissing(Drinks drink, double inputedMoney)
        {
            var prices = new Dictionary<Drinks,double>{
                {
                    Drinks.Tea,0.40
                },
                {
                    Drinks.Chocolate, 0.50
                },
                {
                    Drinks.Coffee, 0.60
                },
                {
                    Drinks.Orange, 0.60
                }
            };
            var result = prices[drink] - inputedMoney;

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
    }

    internal class UnknownDrinkException : Exception
    {
    }
}
