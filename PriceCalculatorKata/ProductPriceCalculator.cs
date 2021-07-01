using System;
using PriceCalculatorKata.Dicsount;
using PriceCalculatorKata.Tax;

namespace PriceCalculatorKata
{
    public interface IPriceCalculator
    {
        float Calculate();
    }
    public class ProductPriceCalculator : IPriceCalculator
    {
        private float _productBasePrice;
        private readonly ITax _allTaxes;
        private readonly IDiscount _allDiscounts;

        public ProductPriceCalculator(Product product)
        {
            _productBasePrice = product.BasePrice;
            var productUpc = product.UPC;
            _allTaxes = new AllTaxes(new UniversalTax());
            _allDiscounts = new AllDiscounts(new UniversalDiscount(),
                new UPCBasedDiscount(productUpc));
        }
        
        public float Calculate()
        {
            return RoundDigits( _productBasePrice + CalculateTax() - CalculateDiscount());
        }
        
        private float CalculateTax()
        {
            return RoundDigits(_productBasePrice * (_allTaxes.getTax() / 100.0f));
        }

        private float CalculateDiscount()
        {
            return RoundDigits(_productBasePrice * (_allDiscounts.getDiscount() / 100.0f));
        }

        private static float RoundDigits(float unrounded)
        {
            return (float)Math.Round(unrounded,2);
        }

    }
}