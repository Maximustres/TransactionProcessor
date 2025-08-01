using api_transaction.Acquirers;
using api_transaction.Entities;
using api_transaction.Enums;
using FluentAssertions;

namespace api_transaction.test.Acquires
{
    public class MockIso8583AcquirerTests
    {
        [Fact]
        public async Task AuthorizeAsync_ShouldReturnStatusAndCode()
        {
            var acquirer = new MockIso8583Acquirer();
            var result = await acquirer.AuthorizeAsync(new Transaction
            {
                Pan = "4111111111111111",
                Expiry = "12/25",
                Amount = 1000,
                Currency = "CLP",
                Cvv = "123",
                MerchantId = "M123"
            });

            result.code.Should().NotBeNullOrWhiteSpace();
            Enum.IsDefined(typeof(TransactionStatus), result.status).Should().BeTrue();
        }
    }
}
