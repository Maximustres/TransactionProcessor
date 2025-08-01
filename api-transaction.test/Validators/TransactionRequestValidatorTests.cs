using api_transaction.DTOs;
using api_transaction.Validators;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api_transaction.test.Validators
{
    public class TransactionRequestValidatorTests
    {
        private readonly TransactionRequestValidator _validator;

        public TransactionRequestValidatorTests()
        {
            _validator = new TransactionRequestValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Amount_Is_Zero()
        {
            var dto = new TransactionDTO { Amount = 0, Pan = "123456", MerchantId = "ABC" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.Amount);
        }

        [Fact]
        public void Should_Have_Error_When_Expiry_Is_Invalid_Format()
        {
            var dto = new TransactionDTO
            {
                Pan = "4111111111111111",
                Expiry = "12/2025",
                Amount = 1000,
                Currency = "CLP",
                Cvv = "123",
                MerchantId = "M123"
            };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.Expiry);
        }

        [Fact]
        public void Should_Have_Error_When_Pan_Is_Invalid_Format()
        {
            var dto = new TransactionDTO
            {
                Pan = "41111111111111111",
                Expiry = "12/25",
                Amount = 1000,
                Currency = "CLP",
                Cvv = "123",
                MerchantId = "M123"
            };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.Pan);
        }

        [Fact]
        public void Should_Have_Error_When_CVV_Exceeds_Permitted_Values()
        {
            var dto = new TransactionDTO
            {
                Pan = "4111111111111111",
                Expiry = "12/25",
                Amount = 1000,
                Currency = "CLP",
                Cvv = "12345",
                MerchantId = "M123"
            };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.Cvv);
        }

        [Fact]
        public void Should_Have_Error_When_CVV_Is_Invalid_Format()
        {
            var dto = new TransactionDTO
            {
                Pan = "4111111111111111",
                Expiry = "12/25",
                Amount = 1000,
                Currency = "CLP",
                Cvv = "12",
                MerchantId = "M123"
            };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.Cvv);
        }

        [Fact]
        public void Should_Have_Error_When_MerchantId_Not_Exist()
        {
            var dto = new TransactionDTO
            {
                Pan = "4111111111111111",
                Expiry = "12/25",
                Amount = 1000,
                Currency = "CLP",
                Cvv = "12"
            };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.MerchantId);
        }

        [Fact]
        public void Should_Not_Have_Errors_When_Valid()
        {
            var dto = new TransactionDTO
            {
                Pan = "4111111111111111",
                Expiry = "12/25",
                Amount = 1000,
                Currency = "CLP",
                Cvv = "123",
                MerchantId = "M123"
            };
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
