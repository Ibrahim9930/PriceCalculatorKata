﻿using System;
using PriceCalculatorKata.PriceModifier;

namespace PriceCalculatorKata
{
    public interface IPriceCalculator
    {
        float Calculate(Product product);
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
        public float Amount;
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
                Amount = float.MaxValue,
                CappingMethod = CappingMethod.Absolute
            };
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
            return CalculateDiscountAmount(_highPrecedenceDiscounts, product.BasePrice, product);
        }

        private float CalculateLowPrecedenceDiscount(Product product)
        {
            return CalculateDiscountAmount(_lowPrecedenceDiscounts, CalculatePriceAfterHighPrecedenceDiscount(product),
                product);
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