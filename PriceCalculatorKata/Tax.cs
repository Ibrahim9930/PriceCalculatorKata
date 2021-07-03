namespace PriceCalculatorKata.Tax
{
    public interface ITax
    {
        float getTax();
    }

    public class UniversalTax : ITax
    {
        public static float Tax = 0;

    
        public float getTax()
        {
            return Tax;
        }
    }
    
    
}