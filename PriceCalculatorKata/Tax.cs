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

    public class TaxesSummation : ITax
    {
        private ITax[] _taxes;
        private float _taxSummation;

        public TaxesSummation(params ITax[] taxes)
        {
            _taxes = taxes;
            _taxSummation = 0;
        }
        public float getTax()
        {
            foreach (var tax in _taxes)
            {
                _taxSummation += tax.getTax();
            }

            return _taxSummation;
        }
    }
    
}