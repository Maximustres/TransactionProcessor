using api_transaction.DTOs;
using api_transaction.Entities;
using api_transaction.Enums;

namespace api_transaction.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionResponseDTO> ProcessTransactionAsync(TransactionDTO request);
        Task<List<TransactionDetailDTO>> GetTransactionDetailsAsync(Guid transactionId);
    }
}
