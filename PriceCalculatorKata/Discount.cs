﻿using System.Collections.Generic;

namespace PriceCalculatorKata.Dicsount
{
    public interface IDiscount
    {
        float getDiscount();
    }

    public class UniversalDiscount : IDiscount
    {
        public static float Discount = 0;

        public float getDiscount()
        {
            return Discount;
        }
    }

    public class UPCBasedDiscount : IDiscount
    {
        public static Dictionary<int, float> UPCDiscounts { get; private set; } = new Dictionary<int, float>();

        private int _productUPC;

        public UPCBasedDiscount(int productUPC)
        {
            _productUPC = productUPC;
        }

        public float getDiscount()
        {
            return UPCDiscounts.GetValueOrDefault(_productUPC);
        }
    }
    
}