using System.Collections.Generic;

namespace otus_interfaces
{
    public class InMemoryTransactionRepository : ITransactionRepository
    {
        private readonly List<ITransaction> _transactions = new List<ITransaction>();

        public void AddTransaction(ITransaction transaction)
        {
            _transactions.Add(transaction);
        }

        public ITransaction[] GetTransactions()
        {
            return _transactions.ToArray();
        }
    }
}
