using System;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using PriceCalculatorKata;
using PriceCalculatorKata.PriceModifier;
using PriceCalculatorKata.Report;


namespace PriceCalculatorsKataTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ProductPricePrecision()
        {
            UniversalTax.Tax = 20;
            UniversalDiscount.Discount = 15;
            UPCBasedDiscount.UPCDiscounts.TryAdd(12345, 7);
            IPriceModifier[] taxes = {new UniversalTax()};
            IPriceModifier[] lowPrecedenceDiscounts = {new UniversalDiscount()};
            IPriceModifier[] highPrecedenceDiscounts = {new UPCBasedDiscount()};
            IPriceModifier[] expenses =
            {
                new AbsoluteExpense(2.2f, "Transport"),
                new RelativeExpense(1, "Packaging")
            };
            IPriceCalculator productPriceCalculator =
                new ProductPriceCalculator(taxes, lowPrecedenceDiscounts, highPrecedenceDiscounts, expenses);
            IReporter productReporter = new ProductReporter((ProductPriceCalculator) productPriceCalculator);
            var roundedUpPriceProduct = new Product(0, 20.256f, productPriceCalculator, productReporter)
            {
                Name = "testing product",
            };
            var roundedDownPriceProduct = new Product(1, 20.254f, productPriceCalculator, productReporter)
            {
                Name = "testing product",
            };

            Assert.AreEqual(20.26f, roundedUpPriceProduct.BasePrice);
            Assert.AreEqual(20.25f, roundedDownPriceProduct.BasePrice);
        }

        [Test]
        public void ProductTaxAndDiscountCalculation()
        {
            UniversalTax.Tax = 20;
            UniversalDiscount.Discount = 15;
            IPriceModifier[] taxes = {new UniversalTax()};
            IPriceModifier[] lowPrecedenceDiscounts = {new UniversalDiscount()};
            IPriceModifier[] highPrecedenceDiscounts = { };
            IPriceModifier[] expenses =
            {
            };
            IPriceCalculator productPriceCalculator =
                new ProductPriceCalculator(taxes, lowPrecedenceDiscounts, highPrecedenceDiscounts, expenses);
            IReporter productReporter = new ProductReporter((ProductPriceCalculator) productPriceCalculator);
            Product productWithDefaultTax = new Product(1, 20.25f, productPriceCalculator, productReporter)
            {
                Name = "The Little Prince",
            };
            Assert.AreEqual(21.26f, productWithDefaultTax.FinalPrice);
        }

        [Test]
        public void ProductWithSpecialDiscountFinalPriceCalculations1()
        {
            UniversalTax.Tax = 20;
            UniversalDiscount.Discount = 15;
            UPCBasedDiscount.UPCDiscounts.TryAdd(12345, 7);
            IPriceModifier[] taxes = {new UniversalTax()};
            IPriceModifier[] lowPrecedenceDiscounts = {new UniversalDiscount(), new UPCBasedDiscount()};
            IPriceModifier[] highPrecedenceDiscounts = { };
            IPriceModifier[] expenses = { };
            IPriceCalculator productPriceCalculator =
                new ProductPriceCalculator(taxes, lowPrecedenceDiscounts, highPrecedenceDiscounts, expenses);
            IReporter productReporter = new ProductReporter((ProductPriceCalculator) productPriceCalculator);

            Product product = new Product(12345, 20.25f, productPriceCalculator, productReporter)
            {
                Name = "The Little Prince"
            };
            Assert.AreEqual(19.78f, Math.Round(product.FinalPrice), 2);
        }

        [Test]
        public void ProductWithNoSpecialDiscountFinalPriceCalculations2()
        {
            UniversalTax.Tax = 20;
            UniversalDiscount.Discount = 15;
            UPCBasedDiscount.UPCDiscounts.TryAdd(12345, 7);
            IPriceModifier[] taxes = {new UniversalTax()};
            IPriceModifier[] lowPrecedenceDiscounts = {new UniversalDiscount(), new UPCBasedDiscount()};
            IPriceModifier[] highPrecedenceDiscounts = { };
            IPriceModifier[] expenses = { };
            IPriceCalculator productPriceCalculator =
                new ProductPriceCalculator(taxes, lowPrecedenceDiscounts, highPrecedenceDiscounts, expenses);
            IReporter productReporter = new ProductReporter((ProductPriceCalculator) productPriceCalculator);
            UniversalTax.Tax = 21;

            Product product = new Product(789, 20.25f, productPriceCalculator, productReporter)
            {
                Name = "The Little Prince"
            };
            Assert.AreEqual(21.46f, Math.Round(product.FinalPrice), 2);
        }

        [Test]
        public void ProductPriceCalculationsWithExpenses()
        {
            UniversalTax.Tax = 21;
            UniversalDiscount.Discount = 15;
            UPCBasedDiscount.UPCDiscounts.TryAdd(12345, 7);
            IPriceModifier[] taxes = {new UniversalTax()};
            IPriceModifier[] lowPrecedenceDiscounts = {new UniversalDiscount(), new UPCBasedDiscount()};
            IPriceModifier[] highPrecedenceDiscounts = { };
            IPriceModifier[] expenses =
            {
                new AbsoluteExpense(2.2f, "Transport"),
                new RelativeExpense(1, "Packaging")
            };
            IPriceCalculator productPriceCalculator =
                new ProductPriceCalculator(taxes, lowPrecedenceDiscounts, highPrecedenceDiscounts, expenses);
            IReporter productReporter = new ProductReporter((ProductPriceCalculator) productPriceCalculator);
            Product p = new Product(12345, 20.25f, productPriceCalculator, productReporter)
            {
                Name = "The Little Prince"
            };
            Assert.AreEqual(22.45f, p.FinalPrice);
        }

        [Test]
        public void ProductPriceCalculationsWithOnlyAUniversalTax()
        {
            UniversalTax.Tax = 21;
            IPriceModifier[] taxes = {new UniversalTax()};
            IPriceModifier[] lowPrecedenceDiscounts = { };
            IPriceModifier[] highPrecedenceDiscounts = { };
            IPriceModifier[] expenses = { };
            IPriceCalculator productPriceCalculator =
                new ProductPriceCalculator(taxes, lowPrecedenceDiscounts, highPrecedenceDiscounts, expenses);
            IReporter productReporter = new ProductReporter((ProductPriceCalculator) productPriceCalculator);
   
            Product p = new Product(12345, 20.25f, productPriceCalculator, productReporter)
            {
                Name = "The Little Prince"
            };
            Assert.AreEqual(24.5f, p.FinalPrice);
        }

        [Test]
        public void ProductReporting()
        {
            UniversalTax.Tax = 21;
            UniversalDiscount.Discount = 15;
            UPCBasedDiscount.UPCDiscounts.TryAdd(12345, 7);
            IPriceModifier[] taxes = {new UniversalTax()};
            IPriceModifier[] lowPrecedenceDiscounts = {new UniversalDiscount(), new UPCBasedDiscount()};
            IPriceModifier[] highPrecedenceDiscounts = { };
            IPriceModifier[] expenses =
            {
                new AbsoluteExpense(2.2f, "Transport"),
                new RelativeExpense(1, "Packaging")
            };
            IPriceCalculator productPriceCalculator =
                new ProductPriceCalculator(taxes, lowPrecedenceDiscounts, highPrecedenceDiscounts, expenses);
            IReporter productReporter = new ProductReporter((ProductPriceCalculator) productPriceCalculator);

            Product product = new Product(12345, 20.25f, productPriceCalculator, productReporter)
            {
                Name = "The Little Prince",
            };
            product.Report(s =>
            {
                Regex rx = new Regex("(?<=(discount).*)([0-9]+.[0-9]+)", RegexOptions.IgnoreCase);
                MatchCollection matches = rx.Matches(s);
                var match = matches.Single();
                Assert.AreEqual("4.45", match.Value);
            });
        }
    }
}