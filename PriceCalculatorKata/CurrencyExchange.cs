using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PriceCalculatorKata
{
    public struct Currency
    {
        public static List<string> AvailableCurrencies { get; private set; }
        private string _currencyCode;

        public string CurrencyCode
        {
            get => _currencyCode;
            set
            {
                if (CheckCurrencyAvailability(value))
                {
                    _currencyCode = value;
                }
                else
                {
                    throw new Exception("Specified currency isn't available");
                }
            }
        }

        private static IWebAPI _webAPI;

        public override bool Equals(object? otherCurrency)
        {
            return otherCurrency is Currency ? CurrencyCode.Equals(((Currency) otherCurrency).CurrencyCode) : false;
        }

        public async Task<float> ConvertTo(Currency toCurrency, float amount)
        {
            var response = await _webAPI.GetResponse("convert", new Dictionary<string, object>()
            {
                {"from", CurrencyCode},
                {"to", toCurrency.CurrencyCode},
                {"amount", amount},
            });

            if (CheckIfKeyExists(response, "result"))
            {
                throw new Exception("No result field in the response");
            }

            return Convert.ToSingle(response["result"]);
        }

        private static bool CheckIfKeyExists(Dictionary<string, object> response, string field)
        {
            return !response.ContainsKey(field);
        }

        public static bool CheckCurrencyAvailability(string currencyCode)
        {
            if (AvailableCurrencies == null)
            {
                return false;
            }

            return AvailableCurrencies.Contains(currencyCode);
        }

        static Currency()
        {
            _webAPI = new HttpClientWebAPI {BaseAddress = "https://api.exchangerate.host/"};
            FillAvailableCurrenciesList();
        }

        private static void FillAvailableCurrenciesList()
        {
            var response = _webAPI.GetResponse("symbols", new Dictionary<string, object>() { }).Result;

            if (CheckIfKeyExists(response, "symbols"))
            {
                throw new Exception("No symbols field in the available currencies  response");
            }

            AvailableCurrencies = new Dictionary<string, object>((IDictionary<string, object>) response["symbols"]).Keys
                .ToList();
        }
    }
}