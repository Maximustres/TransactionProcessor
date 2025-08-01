using api_transaction.DTOs;
using FluentValidation;

namespace api_transaction.Validators
{
    public class TransactionRequestValidator : AbstractValidator<TransactionDTO>
    {
        public TransactionRequestValidator()
        {
            RuleFor(x => x.Pan).CreditCard();
            RuleFor(x => x.Expiry).Matches(@"^(0[1-9]|1[0-2])/\d{2}$");
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.Currency).NotEmpty();
            RuleFor(x => x.Cvv).Matches(@"^\d{3,4}$");
            RuleFor(x => x.MerchantId).NotEmpty();
        }
    }
}
