using api_transaction.Enums;

namespace api_transaction.DTOs
{
    public class TransactionDetailDTO
    {
        public Guid? Id { get; set; }
        public Guid? TransactionId { get; set; }
        public string? Status { get; set; }
        public string? AuthorizationCode { get; set; }
        public string? Observation { get; set; }
        public DateTime? ChangedAt { get; set; }
    }
}
