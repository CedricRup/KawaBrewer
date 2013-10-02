using System;
using System.Linq;
using NUnit.Framework;

namespace Engine
{
    [TestFixture]
    public class Tests
    {
        [Test]
// ReSharper disable InconsistentNaming
        public void Shoud_be_able_to_order_a_tea_with_a_sugar()

        {
            string result = Glop.Order(Drinks.Tea, 1);
            Assert.That(result,Is.EqualTo("T:1:0"));
        }

        [Test]
        public void Shoud_be_able_to_order_a_chocolate_without_sugar()
        {
            string result = Glop.Order(Drinks.Chocolate,0);
            Assert.That(result, Is.EqualTo("H::"));
        }

        [Test]
        public void Should_be_able_to_order_a_coffee_with_two_sugars()
        {
            string result = Glop.Order(Drinks.Coffee, 2);
            Assert.That(result, Is.EqualTo("C:2:0"));
        }
        // ReSharper restore InconsistentNaming

    }

    public enum Drinks
    {
        Chocolate,
        Tea,
        Coffee
    }

    public class Glop
    {
        public static string Order(Drinks drink, int numberOfSugar)
        {
            var str = DrinkToString(drink);
            var sugar = numberOfSugar > 0 ? numberOfSugar.ToString() : string.Empty;
            var touillette = numberOfSugar > 0 ? 0.ToString() : string.Empty;
            return string.Format("{0}:{1}:{2}", str, sugar,touillette);
            
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
                default:
                    throw new UnknownDrinkException();
            }
        }
    }

    internal class UnknownDrinkException : Exception
    {
    }
}
