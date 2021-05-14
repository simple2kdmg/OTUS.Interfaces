using System;

namespace otus_interfaces
{
    public class Transfer : ITransaction
    {
        public ICurrencyAmount Amount { get; }
        public DateTimeOffset Date { get; }

        public string Destination { get; }
        public string Message { get; }

        public override string ToString() => $"Перевод {Amount} на имя {Destination} с сообщением {Message}";
    }
}
