using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace otus_interfaces.Models
{
    class ExchangeRatesApiConverter : ICurrencyConverter
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        private readonly string _apiKey;
        private DateTime _exchangeRatesCollectionDate;

        public ExchangeRatesApiConverter(HttpClient httpClient, IMemoryCache memoryCache, string apiKey)
        {
            this._httpClient = httpClient;
            this._memoryCache = memoryCache;
            this._apiKey = apiKey;
        }

        public async Task<ICurrencyAmount> ConvertCurrencyAsync(ICurrencyAmount amount, string currencyCode)
        {
            var actualExchangeRates = await GetExchangeRatesFromMemoryAsync();
            var exchangeRate = (decimal)(actualExchangeRates.Rates[currencyCode] / actualExchangeRates.Rates[amount.CurrencyCode]);
            return new CurrencyAmount(currencyCode, amount.Amount * exchangeRate);
        }

        private async Task<ExchangeRatesApiResponse> GetExchangeRatesFromMemoryAsync()
        {
            var currentDate = DateTime.Today.Date;
            var inMemoryExchangeRates = _memoryCache.Get<ExchangeRatesApiResponse>(this._exchangeRatesCollectionDate);

            if (inMemoryExchangeRates == null || inMemoryExchangeRates.Date.Date != currentDate)
            {
                inMemoryExchangeRates = await GetExchangeRatesAsync();
                this._exchangeRatesCollectionDate = inMemoryExchangeRates.Date.Date;
                Trace.TraceInformation($"received exсhange rate collection date = {this._exchangeRatesCollectionDate}");
                _memoryCache.Set(this._exchangeRatesCollectionDate, inMemoryExchangeRates, TimeSpan.FromHours(1));
            }

            return inMemoryExchangeRates;
        }

        private async Task<ExchangeRatesApiResponse> GetExchangeRatesAsync()
        {
            Trace.TraceInformation("Request to exchangeratesapi is sending");
            HttpResponseMessage response = await _httpClient.GetAsync($"http://api.exchangeratesapi.io/v1/latest?access_key={_apiKey}");
            Trace.TraceInformation("Response from exchangeratesapi is received");
            response = response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ExchangeRatesApiResponse>(json);
        }
    }
}
