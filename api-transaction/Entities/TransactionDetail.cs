using api_transaction.Enums;

namespace api_transaction.Entities
{
    public class TransactionDetail
    {
        public Guid? Id { get; set; }
        public Guid TransactionId { get; set; }
        public TransactionStatus Status { get; set; }
        public string? AuthorizationCode { get; set; }
        public string? Observation { get; set; }
        public DateTime ChangedAt { get; set; }

        public Transaction? Transaction { get; set; }
    }
}
