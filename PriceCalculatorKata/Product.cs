using System;
using System.Collections.Generic;
using System.Text;
using PriceCalculatorKata.Report;

namespace PriceCalculatorKata
{
    public class Product
    {
        public string Name { get; set; }
        private readonly IPriceCalculator _priceCalculator;
        private readonly IReporter _reporter;
        public Currency Currency { get; set; }
        public Product(int UPC,float basePrice,IPriceCalculator priceCalculator,IReporter reporter,Currency? currency=null)
        {
            this.UPC = UPC;
            this.BasePrice = basePrice;
            _priceCalculator = priceCalculator;
            _reporter = reporter;
            Currency = currency??new Currency(){CurrencyCode = "USD"};
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
