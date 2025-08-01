using api_transaction.Entities;
using api_transaction.Persistence;
using api_transaction.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace api_transaction.test.Repositories
{
    public class TransactionRepositoryTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddTransaction()
        {
            var context = GetDbContext();
            var repository = new TransactionRepository(context);
            var transaction = new Transaction
            {
                Pan = "4111111111111111",
                Expiry = "12/25",
                Amount = 1000,
                Currency = "CLP",
                Cvv = "123",
                MerchantId = "M123"
            };

            var result = await repository.AddAsync(transaction);

            result.Should().NotBeNull();
            context.Transactions.Should().ContainSingle();
        }

        [Fact]
        public async Task AddDetailAsync_ShouldAddDetail()
        {
            var context = GetDbContext();
            var repository = new TransactionRepository(context);
            var detail = new TransactionDetail { TransactionId = Guid.NewGuid() };

            await repository.AddDetailAsync(detail);

            context.TransactionDetails.Should().ContainSingle();
        }

    }
}
