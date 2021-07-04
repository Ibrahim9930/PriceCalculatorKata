using System.Collections.Generic;


namespace PriceCalculatorKata.PriceModifier
{
    public interface IPriceModifier
    {
        double getModificationPercentage(Product product);
    }

    public class UniversalDiscount : IPriceModifier
    {
        public static double Discount = 0;

        public double getModificationPercentage(Product product)
        {
            return Discount;
        }
    }

    public class UPCBasedDiscount : IPriceModifier
    {
        public static Dictionary<int, double> UPCDiscounts { get; private set; } = new Dictionary<int, double>();


        public double getModificationPercentage(Product product)
        {
            return UPCDiscounts.GetValueOrDefault(product.UPC);
        }
    }

    public class UniversalTax : IPriceModifier
    {
        public static double Tax = 0;

        public double getModificationPercentage(Product product)
        {
            return Tax;
        }
    }

    public class AbsoluteExpense : IPriceModifier
    {
        public string Description { get; private set; }
        public double Amount { get; private set; }

        public AbsoluteExpense(double amount, string description)
        {
            Amount = amount;
            Description = description;
        }


        public double getModificationPercentage(Product product)
        {
            return (Amount / product.BasePrice) * 100;
        }
    }

    public class RelativeExpense : IPriceModifier
    {
        public string Description { get; private set; }
        public double Amount { get; private set; }

        public RelativeExpense(double amount, string description)
        {
            Amount = amount;
            Description = description;
        }

        public double getModificationPercentage(Product product)
        {
            return Amount;
        }
    }
}