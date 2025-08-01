namespace api_transaction.DTOs
{
    public class TransactionDTO
    {
        public Guid? Id { get; set; }
        public string? Pan { get; set; }
        public string? Expiry { get; set; }
        public int? Amount { get; set; }
        public string? Currency { get; set; }
        public string? Cvv { get; set; }
        public string? MerchantId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
