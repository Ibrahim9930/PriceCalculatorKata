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
        private readonly ITax[] _allTaxes;
        private readonly IDiscount[] _lowPrecedenceDiscounts;
        private readonly IDiscount[] _highPrecedenceDiscounts;

        public ProductPriceCalculator(Product product)
        {
            _productBasePrice = product.BasePrice;
            var productUpc = product.UPC;
            _allTaxes = new ITax[]
            {
                new UniversalTax()
            };
            _lowPrecedenceDiscounts = new IDiscount[] {new UniversalDiscount()};
            _highPrecedenceDiscounts = new IDiscount[] {new UPCBasedDiscount(productUpc)};
        }

        public float Calculate()
        {
            return RoundDigits(_productBasePrice + CalculateTax() - CalculateDiscount());
        }

        public float CalculateTax()
        {
            float taxSummation = 0;
            foreach (var tax in _allTaxes)
            {
                taxSummation += tax.getTax();
            }
            return RoundDigits(CalculatePriceAfterHighPrecedenceDiscount() * (taxSummation / 100.0f));
        }

        public float CalculateDiscount()
        {
            float highPrecedenceDiscount =
                CalculateHighPrecedenceDiscount();
            float lowPrecedenceDiscount =
                RoundDigits(CalculateLowPrecedenceDiscount());
            return RoundDigits(highPrecedenceDiscount + lowPrecedenceDiscount);
        }

        private float CalculatePriceAfterHighPrecedenceDiscount()
        {
            return (_productBasePrice - CalculateHighPrecedenceDiscount());
        }

        private float CalculateHighPrecedenceDiscount()
        {
            float discountSummation = 0;
            foreach (var discount in _highPrecedenceDiscounts)
            {
                discountSummation += discount.getDiscount();
            }
            return RoundDigits(_productBasePrice * (discountSummation / 100.0f));
        }

        private float CalculateLowPrecedenceDiscount()
        {
            float discountSummation = 0;
            foreach (var discount in _lowPrecedenceDiscounts)
            {
                discountSummation += discount.getDiscount();
            }
            return CalculatePriceAfterHighPrecedenceDiscount() * (discountSummation / 100.0f);
        }

        private static float RoundDigits(float unrounded)
        {
            return (float) Math.Round(unrounded, 2);
        }
    }
}