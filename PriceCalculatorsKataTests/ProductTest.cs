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
        public void ProductWithSpecialDiscountPriceCalculations1()
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
        public void ProductWithNoSpecialDiscountPriceCalculations2()
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
            Assert.AreEqual(22.44f, p.FinalPrice);
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
                Assert.AreEqual("4.46", match.Value);
            });
        }

        [Test]
        public void ProductPriceCalculationsWithMultiplicativeDiscounts()
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
                new ProductPriceCalculator(taxes, lowPrecedenceDiscounts, highPrecedenceDiscounts, expenses,
                    DiscountCombinationMethod.Multiplicative);
            IReporter productReporter = new ProductReporter((ProductPriceCalculator) productPriceCalculator);
            Product p = new Product(12345, 20.25f, productPriceCalculator, productReporter)
            {
                Name = "The Little Prince"
            };
            Assert.AreEqual(22.66f, p.FinalPrice);
        }

        [Test]
        public void ProductPriceCalculationsWithAbsolutelyCappedDiscounts()
        {
            UniversalTax.Tax = 21;
            UniversalDiscount.Discount = 15;
            UPCBasedDiscount.UPCDiscounts.TryAdd(12345, 7);
            IPriceModifier[] taxes = {new UniversalTax()};
            IPriceModifier[] lowPrecedenceDiscounts = {new UniversalDiscount(), new UPCBasedDiscount()};
            IPriceModifier[] highPrecedenceDiscounts = { };
            IPriceModifier[] expenses = { };
            IPriceCalculator productPriceCalculator =
                new ProductPriceCalculator(taxes, lowPrecedenceDiscounts, highPrecedenceDiscounts, expenses,
                    DiscountCombinationMethod.Additive, new DiscountCap()
                    {
                        Amount = 4,
                        CappingMethod = CappingMethod.Absolute
                    });
            IReporter productReporter = new ProductReporter((ProductPriceCalculator) productPriceCalculator);

            Product p = new Product(12345, 20.25f, productPriceCalculator, productReporter)
            {
                Name = "The Little Prince"
            };
            Assert.AreEqual(20.50f, p.FinalPrice);
        }

        [Test]
        public void ProductPriceCalculationsWithRelativelyCappedDiscounts()
        {
            UniversalTax.Tax = 21;
            UniversalDiscount.Discount = 15;
            UPCBasedDiscount.UPCDiscounts.TryAdd(12345, 7);
            IPriceModifier[] taxes = {new UniversalTax()};
            IPriceModifier[] lowPrecedenceDiscounts = {new UniversalDiscount(), new UPCBasedDiscount()};
            IPriceModifier[] highPrecedenceDiscounts = { };
            IPriceModifier[] expenses = { };
            IPriceCalculator productPriceCalculator =
                new ProductPriceCalculator(taxes, lowPrecedenceDiscounts, highPrecedenceDiscounts, expenses,
                    DiscountCombinationMethod.Additive, new DiscountCap()
                    {
                        Amount = 20,
                        CappingMethod = CappingMethod.Relative
                    });
            IReporter productReporter = new ProductReporter((ProductPriceCalculator) productPriceCalculator);

            Product p = new Product(12345, 20.25f, productPriceCalculator, productReporter)
            {
                Name = "The Little Prince"
            };
            Assert.AreEqual(20.45f, p.FinalPrice);
        }

        [Test]
        public void ProductPriceCalculationsWithoutRelativelyCappedDiscounts()
        {
            UniversalTax.Tax = 21;
            UniversalDiscount.Discount = 15;
            UPCBasedDiscount.UPCDiscounts.TryAdd(12345, 7);
            IPriceModifier[] taxes = {new UniversalTax()};
            IPriceModifier[] lowPrecedenceDiscounts = {new UniversalDiscount(), new UPCBasedDiscount()};
            IPriceModifier[] highPrecedenceDiscounts = { };
            IPriceModifier[] expenses = { };
            IPriceCalculator productPriceCalculator =
                new ProductPriceCalculator(taxes, lowPrecedenceDiscounts, highPrecedenceDiscounts, expenses,
                    DiscountCombinationMethod.Additive, new DiscountCap()
                    {
                        Amount = 30,
                        CappingMethod = CappingMethod.Relative
                    });
            IReporter productReporter = new ProductReporter((ProductPriceCalculator) productPriceCalculator);

            Product p = new Product(12345, 20.25f, productPriceCalculator, productReporter)
            {
                Name = "The Little Prince"
            };
            Assert.AreEqual(20.04f, p.FinalPrice);
        }

        [Test]
        public void ProductReportingWithCurrencyConversion()
        {
            UniversalTax.Tax = 20;
            IPriceModifier[] taxes = {new UniversalTax()};
            IPriceModifier[] lowPrecedenceDiscounts = { };
            IPriceModifier[] highPrecedenceDiscounts = { };
            IPriceModifier[] expenses = { };
            IPriceCalculator productPriceCalculator =
                new ProductPriceCalculator(taxes, lowPrecedenceDiscounts, highPrecedenceDiscounts, expenses);
            IReporter productReporter = new ProductReporter((ProductPriceCalculator) productPriceCalculator,
                new Currency() {CurrencyCode = "GBP"});

            Product product = new Product(12345, 20.25f, productPriceCalculator, productReporter)
            {
                Name = "The Little Prince",
            };
            product.Report(async s =>
            {
                float convertedBasePrice = await 
                    product.Currency.ConvertTo((productReporter as ProductReporter).Currency, product.BasePrice);
                Regex rx = new Regex("(?<=(base price).*)([0-9]+.[0-9]+)", RegexOptions.IgnoreCase);
                MatchCollection matches = rx.Matches(s);
                var match = matches.Single();
                Assert.AreEqual("14.65", match.Value);
            });
        }
    }
}