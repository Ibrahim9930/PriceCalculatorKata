using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PriceCalculatorKata.PriceModifier;


namespace PriceCalculatorKata.Report
{
    public interface IReporter
    {
        Task Report(Product product, Action<string> display);
    }

    public class ProductReporter : IReporter
    {
        private ProductPriceCalculator _productPriceCalculator;
        public Currency Currency;

        public ProductReporter(ProductPriceCalculator productPriceCalculator, Currency? currency = null)
        {
            _productPriceCalculator = productPriceCalculator;
            Currency = currency ?? new Currency() {CurrencyCode = "USD"};
        }


        public async Task Report(Product product, Action<string> displayMethod)
        {
            Dictionary<string, float> priceModifiersAmounts = new Dictionary<string, float>()
            {
                {"Base Price",product.BasePrice},
                {"Tax",_productPriceCalculator.CalculateTax(product)},
                {"Discount",_productPriceCalculator.CalculateDiscount(product)},
                {"Expenses ",_productPriceCalculator.CalculateExpenses(product)},
                {"Final Price",product.FinalPrice},
                
            };
            await ConvertCurrencyIfNeeded(priceModifiersAmounts,product);
            string modifiersReportingString = CreateModifiersReportingString(priceModifiersAmounts);
            displayMethod($"{product.Name}\n{modifiersReportingString}");
        }

        private string CreateModifiersReportingString(Dictionary<string, float> priceModifiersAmounts)
        {
            string output = "";
            foreach (var modifiersAmount in priceModifiersAmounts)
            {
                output += $"{modifiersAmount.Key} : {modifiersAmount.Value:0.00} {Currency.CurrencyCode}\n";
            }

            return output;
        }

        private async Task ConvertCurrencyIfNeeded(Dictionary<string, float> priceModifiersAmounts,Product product)
        {
            if (!Currency.Equals(product.Currency))
            {
                var keys = priceModifiersAmounts.Keys.ToArray();
                foreach (var key in keys)
                {
                    priceModifiersAmounts[key] =
                        await product.Currency.ConvertTo(Currency, priceModifiersAmounts[key]);
                }
            }
        }
        
    }
}