﻿using System.Threading.Tasks;

namespace otus_interfaces
{
    public interface ICurrencyConverter
    {
        //ICurrencyAmount ConvertCurrency(ICurrencyAmount amount, string currencyCode);
        Task<ICurrencyAmount> ConvertCurrencyAsync(ICurrencyAmount amount, string currencyCode);
    }
}
