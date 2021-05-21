using System.IO;
using System.Threading.Tasks;

namespace otus_interfaces
{
    public interface IBudgetApplication
    {
        void AddTransaction(string input);
        void OutputTransactions();
        Task OutputBalanceInCurrency(string currencyCode);
    }
}
