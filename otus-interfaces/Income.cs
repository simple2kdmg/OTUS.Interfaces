using System;

namespace otus_interfaces
{
    public class Income : ITransaction
    {
        public ICurrencyAmount Amount { get; }
        public DateTimeOffset Date { get; }

        public string Source { get; }

        public override string ToString() => $"Зачисление {Amount} от {Source}";
    }
}
