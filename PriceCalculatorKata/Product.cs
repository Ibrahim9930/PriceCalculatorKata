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
        public Product(int UPC,float basePrice)
        {
            this.UPC = UPC;
            this.BasePrice = basePrice;
            _priceCalculator = new ProductPriceCalculator(this);
            _reporter = new ProductReporter(this);
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

        public float FinalPrice => _priceCalculator.Calculate();

        public void Display(Action<string> displayMethod)
        {
            _reporter.Report(displayMethod);
        }
        
    }
}
