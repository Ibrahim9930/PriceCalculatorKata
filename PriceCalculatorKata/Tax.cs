namespace PriceCalculatorKata.Tax
{
    public interface ITax
    {
        float getTax();
    }

    public class UniversalTax : ITax
    {
        private readonly float _tax;

        public float getTax()
        {
            return _tax;
        }
    }

    public class AllTaxes : ITax
    {
        private ITax[] _taxes;
        private float _taxSummation;

        public AllTaxes(ITax[] taxes)
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