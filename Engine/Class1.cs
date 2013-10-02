﻿using NUnit.Framework;

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

        // ReSharper restore InconsistentNaming

    }

    public enum Drinks
    {
        Chocolate,
        Tea
    }

    public class Glop
    {
        public static string Order(Drinks drink, int numberOfSugar)
        {
            string str = "";
            switch (drink)
            {
                    case Drinks.Tea:
                    str += "T";
                    break;
                    case Drinks.Chocolate:
                    str += "H";
                    break;
            }
            str += ":";
            if (numberOfSugar > 0)
                str += numberOfSugar;
            str += ":";
            if (numberOfSugar > 0)
                str += "0";

            return str;
        }
    }
}