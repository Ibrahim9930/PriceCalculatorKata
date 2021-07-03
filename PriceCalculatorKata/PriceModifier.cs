using System.Collections.Generic;


namespace PriceCalculatorKata.PriceModifier
{
    public interface IPriceModifier
    {
        float getModificationPercentage(Product product);
    }

    public class UniversalDiscount : IPriceModifier
    {
        public static float Discount = 0;

        public float getModificationPercentage(Product product)
        {
            return Discount;
        }
    }

    public class UPCBasedDiscount : IPriceModifier
    {
        public static Dictionary<int, float> UPCDiscounts { get; private set; } = new Dictionary<int, float>();


        public float getModificationPercentage(Product product)
        {
            return UPCDiscounts.GetValueOrDefault(product.UPC);
        }
    }

    public class UniversalTax : IPriceModifier
    {
        public static float Tax = 0;

        public float getModificationPercentage(Product product)
        {
            return Tax;
        }
    }

    public class AbsoluteExpense : IPriceModifier
    {
        public string Description { get; private set; }
        public float Amount { get; private set; }

        public AbsoluteExpense(float amount, string description)
        {
            Amount = amount;
            Description = description;
        }


        public float getModificationPercentage(Product product)
        {
            return (Amount / product.BasePrice) * 100;
        }
    }

    public class RelativeExpense : IPriceModifier
    {
        public string Description { get; private set; }
        public float Amount { get; private set; }

        public RelativeExpense(float amount, string description)
        {
            Amount = amount;
            Description = description;
        }

        public float getModificationPercentage(Product product)
        {
            return Amount;
        }
    }
}