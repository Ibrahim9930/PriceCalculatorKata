using System.Collections.Generic;

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

    public class DiscountsSummation : IDiscount
    {
        private IDiscount[] _discounts;

        public DiscountsSummation(params IDiscount[] discounts)
        {
            _discounts = discounts;
        }

        public float getDiscount()
        {
            float discountSummation = 0;

            foreach (var discount in _discounts)
            {
                discountSummation += discount.getDiscount();
            }

            return discountSummation;
        }
    }
}