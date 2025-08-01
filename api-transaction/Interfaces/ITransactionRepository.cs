using api_transaction.Entities;

namespace api_transaction.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> AddAsync(Transaction transaction);
        Task AddDetailAsync(TransactionDetail detail);
        Task<List<TransactionDetail>> GetDetailsAsync(Guid transactionId);

    }
}
