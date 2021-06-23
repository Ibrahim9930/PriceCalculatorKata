using NUnit.Framework;
using PriceCalculatorKata;
namespace PriceCalculatorsKataTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void productPricePercision()
        {
            var roundedUpPriceProduct = new Product(0)
            {
                BasePrice = 20.256f,
                Name = "testing product",
                TaxPercentage = 20,
            };
            var roundedDownPriceProduct = new Product(1)
            {
                BasePrice = 20.254f,
                Name = "testing product",
                TaxPercentage = 20,
            };

            Assert.AreEqual(20.26f, roundedUpPriceProduct.BasePrice);
            Assert.AreEqual(20.25f, roundedDownPriceProduct.BasePrice);
        }

        [Test]
        public void productTaxAndDiscountCalculation()
        {
            var productWithDefaultTax = new Product(0)
            {
                Name = "testing product",
                BasePrice = 20.25f,
            };



            Product.discount = 15;

            Assert.AreEqual(21.26f, productWithDefaultTax.FinalPrice);


        }
        }
    
}