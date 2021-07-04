using System;
using PriceCalculatorKata.PriceModifier;

namespace PriceCalculatorKata
{
    public interface IPriceCalculator
    {
        double Calculate(Product product);
    }

    public enum DiscountCombinationMethod
    {
        Additive,
        Multiplicative
    }

    public enum CappingMethod
    {
        Absolute,
        Relative
    }

    public struct DiscountCap
    {
        public double Amount;
        public CappingMethod CappingMethod;
    }

    public class ProductPriceCalculator : IPriceCalculator
    {
        private readonly IPriceModifier[] _allTaxes;
        private readonly IPriceModifier[] _lowPrecedenceDiscounts;
        private readonly IPriceModifier[] _highPrecedenceDiscounts;
        private readonly IPriceModifier[] _expenses;
        private DiscountCombinationMethod _discountCombinationMethod;

        private DiscountCap _cap; 

        public ProductPriceCalculator(IPriceModifier[] allTaxes, IPriceModifier[] lowPrecedenceDiscounts,
            IPriceModifier[] highPrecedenceDiscounts, IPriceModifier[] expenses,
            DiscountCombinationMethod discountCombinationMethod = DiscountCombinationMethod.Additive,
            DiscountCap? discountCap = null)
        {
            _allTaxes = allTaxes;
            _lowPrecedenceDiscounts = lowPrecedenceDiscounts;
            _highPrecedenceDiscounts = highPrecedenceDiscounts;
            _expenses = expenses;
            _discountCombinationMethod = discountCombinationMethod;
            _cap = discountCap ?? new DiscountCap()
            {
                Amount = double.MaxValue,
                CappingMethod = CappingMethod.Absolute
            };
        }

        public double Calculate(Product product)
        {
            return RoundDigits(product.BasePrice + CalculateTax(product) + CalculateExpenses(product) -
                               CalculateDiscount(product));
        }

        public double CalculateTax(Product product)
        {
            double taxSummation = CalculateModifierSummation(_allTaxes, product);

            return RoundDigits(CalculatePriceAfterHighPrecedenceDiscount(product) * (taxSummation / 100.0));
        }

        public double CalculateExpenses(Product product)
        {
            double expenseSummation = CalculateModifierSummation(_expenses, product);

            return RoundDigits(product.BasePrice * (expenseSummation / 100.0));
        }

        public double CalculateDiscount(Product product)
        {
            double highPrecedenceDiscount =
                CalculateHighPrecedenceDiscount(product);
            double lowPrecedenceDiscount =
                RoundDigits(CalculateLowPrecedenceDiscount(product));
            double totalDiscounts = highPrecedenceDiscount + lowPrecedenceDiscount;
            CapDiscounts(ref totalDiscounts, product);
            return RoundDigits(totalDiscounts);
        }

        private double CalculateHighPrecedenceDiscount(Product product)
        {
            return CalculateDiscountAmount(_highPrecedenceDiscounts, product.BasePrice, product);
        }

        private double CalculateLowPrecedenceDiscount(Product product)
        {
            return CalculateDiscountAmount(_lowPrecedenceDiscounts, CalculatePriceAfterHighPrecedenceDiscount(product),
                product);
        }

        private double CalculatePriceAfterHighPrecedenceDiscount(Product product)
        {
            return (product.BasePrice - CalculateHighPrecedenceDiscount(product));
        }

        private double CalculateModifierSummation(IPriceModifier[] modifiers, Product product)
        {
            double sum = 0;
            foreach (var modifier in modifiers)
            {
                sum += modifier.getModificationPercentage(product);
            }

            return sum;
        }

        private double CalculateDiscountAmount(IPriceModifier[] discounts, double priceAccumulator, Product product)
        {
            double totalDiscounts = 0;
            foreach (var discount in discounts)
            {
                float currentDiscount =
                    RoundDigits(priceAccumulator * (discount.getModificationPercentage(product) / 100.0f));
                totalDiscounts += currentDiscount;
                if (_discountCombinationMethod == DiscountCombinationMethod.Multiplicative)
                {
                    priceAccumulator -= currentDiscount;
                }
            }

            return RoundDigits(totalDiscounts);
        }

        private void CapDiscounts(ref double totalDiscounts, Product product)
        {
            if (_cap.CappingMethod == CappingMethod.Absolute)
            {
                if (totalDiscounts > _cap.Amount)
                    totalDiscounts = _cap.Amount;
            }
            else if (_cap.CappingMethod == CappingMethod.Relative)
            {
                double cappingAmount = (_cap.Amount / 100.0) * product.BasePrice;
                if (totalDiscounts > cappingAmount)
                    totalDiscounts = cappingAmount;
            }
        }

        private static float RoundDigits(float unrounded)
        {
            return  Math.Round(unrounded, precision);
        }
    }
}