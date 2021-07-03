using System;
using PriceCalculatorKata.PriceModifier;

namespace PriceCalculatorKata
{
    public interface IPriceCalculator
    {
        float Calculate(Product product);
    }

    public class ProductPriceCalculator : IPriceCalculator
    {
        private readonly IPriceModifier[] _allTaxes;
        private readonly IPriceModifier[] _lowPrecedenceDiscounts;
        private readonly IPriceModifier[] _highPrecedenceDiscounts;
        private readonly IPriceModifier[] _expenses;

        public ProductPriceCalculator(IPriceModifier[] allTaxes, IPriceModifier[] lowPrecedenceDiscounts,
            IPriceModifier[] highPrecedenceDiscounts, IPriceModifier[] expenses)
        {
            _allTaxes = allTaxes;
            _lowPrecedenceDiscounts = lowPrecedenceDiscounts;
            _highPrecedenceDiscounts = highPrecedenceDiscounts;
            _expenses = expenses;
        }

        public float Calculate(Product product)
        {
            return RoundDigits(product.BasePrice + CalculateTax(product) + CalculateExpenses(product) -
                               CalculateDiscount(product));
        }

        public float CalculateTax(Product product)
        {
            float taxSummation = CalculateModifierSummation(_allTaxes, product);

            return RoundDigits(CalculatePriceAfterHighPrecedenceDiscount(product) * (taxSummation / 100.0f));
        }

        public float CalculateExpenses(Product product)
        {
            float expenseSummation = CalculateModifierSummation(_expenses, product);

            return RoundDigits(product.BasePrice * (expenseSummation / 100.0f));
        }

        public float CalculateDiscount(Product product)
        {
            float highPrecedenceDiscount =
                CalculateHighPrecedenceDiscount(product);
            float lowPrecedenceDiscount =
                RoundDigits(CalculateLowPrecedenceDiscount(product));
            return RoundDigits(highPrecedenceDiscount + lowPrecedenceDiscount);
        }

   

        private float CalculateHighPrecedenceDiscount(Product product)
        {
            float discountSummation = 0;
            foreach (var discount in _highPrecedenceDiscounts)
            {
                discountSummation += discount.getModificationPercentage(product);
            }

            return RoundDigits(product.BasePrice * (discountSummation / 100.0f));
        }

  
        private float CalculateLowPrecedenceDiscount(Product product)
        {
            float discountSummation = 0;
            foreach (var discount in _lowPrecedenceDiscounts)
            {
                discountSummation += discount.getModificationPercentage(product);
            }

            return CalculatePriceAfterHighPrecedenceDiscount(product) * (discountSummation / 100.0f);
        }

        private float CalculatePriceAfterHighPrecedenceDiscount(Product product)
        {
            return (product.BasePrice - CalculateHighPrecedenceDiscount(product));
        }

        private float CalculateModifierSummation(IPriceModifier[] modifiers, Product product)
        {
            float sum = 0;
            foreach (var modifier in modifiers)
            {
                sum += modifier.getModificationPercentage(product);
            }

            return sum;
        }

 

        private static float RoundDigits(float unrounded)
        {
            return (float) Math.Round(unrounded, 2);
        }
    }
}