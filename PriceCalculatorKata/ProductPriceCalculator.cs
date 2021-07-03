﻿using System;
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
        private readonly IDiscount _lowPrecedenceDiscounts;
        private readonly IDiscount _highPrecedenceDiscounts;

        public ProductPriceCalculator(Product product)
        {
            _productBasePrice = product.BasePrice;
            var productUpc = product.UPC;
            _allTaxes = new TaxesSummation(new UniversalTax());
            _lowPrecedenceDiscounts = new DiscountsSummation(new UniversalDiscount());
            _highPrecedenceDiscounts = new DiscountsSummation(new UPCBasedDiscount(productUpc));
        }
        
        public float Calculate()
        {
            return RoundDigits( _productBasePrice + CalculateTax() - CalculateDiscount());
        }
        
        public float CalculateTax()
        {
            return RoundDigits(_productBasePrice * (_allTaxes.getTax() / 100.0f));
        }

        public float CalculateDiscount()
        {
            return RoundDigits(_productBasePrice * (_allDiscounts.getDiscount() / 100.0f));
        }

        private static float RoundDigits(float unrounded)
        {
            return (float)Math.Round(unrounded,2);
        }

    }
}