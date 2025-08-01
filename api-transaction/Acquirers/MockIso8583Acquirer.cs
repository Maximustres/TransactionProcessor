using api_transaction.Entities;
using api_transaction.Enums;

namespace api_transaction.Acquirers
{
    public class MockIso8583Acquirer : ITransactionAcquirer
    {
        private static readonly string[] Codes = { "00", "05", "51", "91", "87" };
        private static readonly Random Rand = new();

        public Task<(TransactionStatus status, string code)> AuthorizeAsync(Transaction request)
        {
            var code = Codes[Rand.Next(Codes.Length)];

            var status = code switch
            {
                "00" => TransactionStatus.Approved,
                "05" => TransactionStatus.Declined,
                "51" => TransactionStatus.Declined,
                "91" => TransactionStatus.Timeout,
                _ => TransactionStatus.Error,
            };


            return Task.FromResult((status, code));
        }
    }
}
