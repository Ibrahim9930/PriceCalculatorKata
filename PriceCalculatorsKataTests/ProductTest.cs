using System;
using NUnit.Framework;
using PriceCalculatorKata;
using PriceCalculatorKata.Dicsount;
using PriceCalculatorKata.Tax;

namespace PriceCalculatorsKataTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            UniversalTax.Tax = 20f;
            UniversalDiscount.Discount = 15f;
        }

        [Test]
        public void productPricePercision()
        {
            var roundedUpPriceProduct = new Product(0,20.256f)
            {
                Name = "testing product",
            };
            var roundedDownPriceProduct = new Product(1,20.254f)
            {
                Name = "testing product",
            };

            Assert.AreEqual(20.26f, roundedUpPriceProduct.BasePrice);
            Assert.AreEqual(20.25f, roundedDownPriceProduct.BasePrice);
        }

        [Test]
        public void productTaxAndDiscountCalculation()
        {
            Product productWithDefaultTax = new Product(1,20.25f)
            {
                Name = "s",
            };
            Assert.AreEqual(21.26f, productWithDefaultTax.FinalPrice);
        }

        [Test]
        public void ProductWithSpecialDiscountFinalPriceCalculations1()
        {
            UPCBasedDiscount.UPCDiscounts.Add(12345,7);
            Product product = new Product(12345, 20.25f)
            {
                Name = "The Little Prince"
            };
            Assert.AreEqual(19.84f,Math.Round(product.FinalPrice),2);
        }
        
        [Test]
        public void ProductWithSpecialDiscountFinalPriceCalculations2()
        {
            UniversalTax.Tax = 21;
            
            UPCBasedDiscount.UPCDiscounts.Add(789,7);
            Product product = new Product(12345, 20.25f)
            {
                Name = "The Little Prince"
            };
            Assert.AreEqual(21.46f,Math.Round(product.FinalPrice),2);
        }
    }
}