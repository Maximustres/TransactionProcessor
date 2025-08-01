using api_transaction.DTOs;
using api_transaction.Entities;
using api_transaction.Enums;

namespace api_transaction.Acquirers
{
    public interface ITransactionAcquirer
    {
        Task<(TransactionStatus status, string code)> AuthorizeAsync(Transaction request);
    }
}
