using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace otus_interfaces
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            var currencyConverter = new ExchangeRatesApiConverter(new HttpClient(), new MemoryCache(new MemoryCacheOptions()), "a5cf9da55cb835d0a633a7825b3aa8b5");

            var transactionRepository = new InMemoryTransactionRepository();
            var transactionParser = new TransactionParser();

            var budgetApp = new BudjetApplication(transactionRepository, transactionParser, currencyConverter);

            budgetApp.AddTransaction("Трата -400 RUB Продукты Пятерочка");
            budgetApp.AddTransaction("Трата -2000 RUB Бензин IRBIS");
            budgetApp.AddTransaction("Трата -500 RUB Кафе Шоколадница");

            budgetApp.OutputTransactions();

            budgetApp.OutputBalanceInCurrency("USD");

            Console.Read();
        }
    }
}
