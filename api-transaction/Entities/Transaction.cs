using api_transaction.Enums;

namespace api_transaction.Entities
{
    public class Transaction
    {
        public Guid? Id { get; set; }
        public required string Pan { get; set; }
        public required string Expiry { get; set; }
        public int Amount { get; set; }
        public required string Currency { get; set; }
        public required string Cvv { get; set; }
        public required string MerchantId { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<TransactionDetail> Details { get; set; } = new();
    }
}
