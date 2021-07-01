using System;
using PriceCalculatorKata.Dicsount;
using PriceCalculatorKata.Tax;

namespace PriceCalculatorKata.Report
{
    public interface IReporter
    {
        void Report(Action<string> display);
    }

    public class ProductReporter : IReporter
    {
        private Product _product;
        private IDiscount _allDiscounts;
        private ProductPriceCalculator _productPriceCalculator;

        public ProductReporter(Product product)
        {
            _product = product;
            _allDiscounts = new AllDiscounts(new UniversalDiscount(),
                new UPCBasedDiscount(_product.UPC));
            _productPriceCalculator = new ProductPriceCalculator(product);
        }


        public void Report(Action<string> displayMethod)
        {
            displayMethod($"{_product.Name}'s price before tax : {_product.BasePrice:0.00}$" +
                          $" and after a {UniversalTax.Tax}% tax : {_product.FinalPrice:0.00}$ with a discount of " +
                          $"{GetDiscountTextRepresentation()}");
        }

        private string GetDiscountTextRepresentation()
        {
            return _allDiscounts.getDiscount() == 0
                ? "no discount applied"
                : $"with {_productPriceCalculator.CalculateDiscount():0.00}$ discount applied";
        }
    }
}