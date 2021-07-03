using System;
using PriceCalculatorKata.PriceModifier;


namespace PriceCalculatorKata.Report
{
    public interface IReporter
    {
        void Report(Product product,Action<string> display);
    }

    public class ProductReporter : IReporter
    {
        private ProductPriceCalculator _productPriceCalculator;

        public ProductReporter(ProductPriceCalculator productPriceCalculator)
        {
            _productPriceCalculator = productPriceCalculator;
        }


        public void Report(Product product, Action<string> displayMethod)
        {
            displayMethod($"{product.Name}'s price before tax : ${product.BasePrice:0.00}" +
                          $" and after a {UniversalTax.Tax}% tax : ${product.FinalPrice:0.00} with a discount of " +
                          $"${GetDiscountTextRepresentation(product)}");
        }

        private string GetDiscountTextRepresentation(Product product)
        {
            return _productPriceCalculator.CalculateDiscount(product) == 0
                ? "no discount applied"
                : $"with {_productPriceCalculator.CalculateDiscount(product):0.00}$ discount applied";
        }
    }
}