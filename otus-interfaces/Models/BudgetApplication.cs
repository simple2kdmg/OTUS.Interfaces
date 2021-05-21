using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace otus_interfaces
{
    public class BudjetApplication : IBudgetApplication
    {
        private readonly ICurrencyConverter _currencyConverter;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionParser _transactionParser;

        public BudjetApplication(ITransactionRepository transactionRepository, ITransactionParser transactionParser, ICurrencyConverter currencyConverter)
        {
            _currencyConverter = currencyConverter;
            _transactionRepository = transactionRepository;
            _transactionParser = transactionParser;
        }

        public void AddTransaction(string input)
        {
            var transaction = _transactionParser.Parse(input);
            _transactionRepository.AddTransaction(transaction);
        }

        public void AddTransactionsFromFile(string path)
        {
            if (File.Exists(path))
            {
                ITransaction[] transactions = File.ReadAllLines(path).Select(str => _transactionParser.Parse(str)).ToArray();
                foreach (var transaction in transactions)
                {
                    _transactionRepository.AddTransaction(transaction);
                }
            }
        }

        public void OutputTransactions()
        {
            foreach (var transaction in _transactionRepository.GetTransactions())
            {
                Console.WriteLine(transaction);
            }
        }

        public void ReadTransactionFromConsole()
        {
            Console.WriteLine("Input transaction.");
            string input;
            do
            {
                input = Console.ReadLine();
                if (!String.IsNullOrEmpty(input))
                {
                    _transactionRepository.AddTransaction(_transactionParser.Parse(input));
                }
            } while (!String.IsNullOrEmpty(input));
        }

        public async Task OutputBalanceInCurrency(string currencyCode)
        {
            var totalCurrencyAmount = new CurrencyAmount(currencyCode, 0);

            foreach (var transaction in _transactionRepository.GetTransactions())
            {
                var amount = transaction.Amount;
                if (amount.CurrencyCode != currencyCode)
                {
                    amount = await _currencyConverter.ConvertCurrencyAsync(amount, currencyCode);
                }
                totalCurrencyAmount += amount;
            }

            Console.WriteLine($"Balance: {totalCurrencyAmount}");
        }
    }
}
