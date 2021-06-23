using System;
using System.Collections.Generic;
using System.Text;

namespace PriceCalculatorKata
{
    public class Product
    {
        public string Name { get; set; }
        public int UPC { get; }

        private float _basePrice;
        public float BasePrice
        {
            get
            {
                return _basePrice;
            } set {
                if(value > 0)
                {
                    _basePrice = (float)Math.Round(value, 2);
                }   
            }
        }

        public float PriceAfterTax
        {
            get
            {
                return (float)Math.Round(BasePrice + (TaxPercentage / 100.0f) * BasePrice,2);
            }
        }
        public int TaxPercentage { get; set; }

        public Product(int UPC)
        {
            this.UPC = UPC;
            this.TaxPercentage = 20;
        }

        public void Display(Action<string> displayMethod)
        {
            displayMethod($"{Name}'s price before tax : {BasePrice:0.00}$ and after a {TaxPercentage}% tax : {PriceAfterTax:0.00}$");
        }


    }
}
