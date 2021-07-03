using System;
using System.Collections.Generic;
using System.Text;
using PriceCalculatorKata.Report;

namespace PriceCalculatorKata
{
    public class Product
    {
        public string Name { get; set; }
        private IPriceCalculator _priceCalculator;
        private IReporter _reporter;
        public Product(int UPC,float basePrice,IPriceCalculator priceCalculator,IReporter reporter)
        {
            this.UPC = UPC;
            this.BasePrice = basePrice;
            _priceCalculator = priceCalculator;
            _reporter = reporter;
        }
        public int UPC { get; private set; }
        private float _basePrice;
        public float BasePrice
        {
            get => _basePrice;
            set
            {
                if (value > 0)
                {
                    _basePrice = (float)Math.Round(value, 2);
                }
            }
        }

        public float FinalPrice => _priceCalculator.Calculate(this);

        public void Report(Action<string> displayMethod)
        {
            _reporter.Report(this,displayMethod);
        }
        
    }
}
