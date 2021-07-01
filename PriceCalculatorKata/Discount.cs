using System.Collections.Generic;

namespace PriceCalculatorKata.Dicsount
{
    public interface IDiscount
    {
        float getDiscount();
    }
    public class UniversalDiscount : IDiscount
    {
        private readonly float _discount;

        public UniversalDiscount(float discount)
        {
            _discount = discount;
        }
        public float getDiscount()
        {
            return _discount;
        }
    }

    public class UPCBasedDiscount : IDiscount
    {
        public static Dictionary<int, float> UPCDiscounts { get; private set; }
        
        private int _productUPC;

        public UPCBasedDiscount(int productUPC)
        {
            _productUPC = productUPC;
            UPCDiscounts = new Dictionary<int, float>();
        }
        public float getDiscount()
        {
            return UPCDiscounts.GetValueOrDefault(_productUPC);
        }
    }

    public class AllDiscounts : IDiscount
    {
        private IDiscount[] _discounts;
        private float _discountSummation;
        
        public float getDiscount()
        {
            foreach (var discount in _discounts)
            {
                _discountSummation += discount.getDiscount();
            }

            return _discountSummation;
        }
    }
}