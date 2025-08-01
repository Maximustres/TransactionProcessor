using api_transaction.Entities;
using api_transaction.Interfaces;
using api_transaction.Persistence;
using Microsoft.EntityFrameworkCore;

namespace api_transaction.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context) => _context = context;

        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            _context.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task AddDetailAsync(TransactionDetail detail)
        {
            _context.TransactionDetails.Add(detail);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TransactionDetail>> GetDetailsAsync(Guid transactionId)
        {
            return await _context.TransactionDetails
                .Where(x => x.TransactionId.Equals(transactionId))
                .OrderBy(x => x.ChangedAt)
                .ToListAsync();
        }
    }
}
