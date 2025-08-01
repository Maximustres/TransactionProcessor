using api_transaction.Controllers;
using api_transaction.DTOs;
using api_transaction.Interfaces;
using api_transaction.Validators;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace api_transaction.test.Controller
{
    public class TransactionControllerTests
    {
        [Fact]
        public async Task Post_Should_Return_BadRequest_When_Invalid()
        {
            var serviceMock = new Mock<ITransactionService>();
            var validator = new TransactionRequestValidator();
            var loggerMock = new Mock<ILogger<TransactionsController>>();

            var controller = new TransactionsController(serviceMock.Object, validator, loggerMock.Object);
            controller.ModelState.AddModelError("Amount", "Required");

            var result = await controller.Post(new TransactionDTO());

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Post_Should_Return_Ok_When_Valid()
        {
            // Arrange
            var serviceMock = new Mock<ITransactionService>();
            var loggerMock = new Mock<ILogger<TransactionsController>>();

            var dto = new TransactionDTO
            {
                Pan = "4111111111111111",
                Expiry = "12/25",
                Amount = 1000,
                Currency = "CLP",
                Cvv = "123",
                MerchantId = "M123"
            };

            var validatorMock = new Mock<IValidator<TransactionDTO>>();
            validatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult()); // valid

            var controller = new TransactionsController(serviceMock.Object, validatorMock.Object, loggerMock.Object);

            var expectedResponse = new TransactionResponseDTO
            {
                Code = "00",
                Status = "Approved"
            };

            serviceMock.Setup(s => s.ProcessTransactionAsync(dto))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await controller.Post(dto);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GetDetails_Should_Return_Ok_When_TransactionExists()
        {
            // Arrange
            var serviceMock = new Mock<ITransactionService>();
            var validator = new Mock<IValidator<TransactionDTO>>();
            var loggerMock = new Mock<ILogger<TransactionsController>>();

            var controller = new TransactionsController(serviceMock.Object, validator.Object, loggerMock.Object);
            var transactionId = Guid.NewGuid();
            var details = new List<TransactionDetailDTO>
            {
                new TransactionDetailDTO { Status = "Approved" }
            };

            serviceMock.Setup(s => s.GetTransactionDetailsAsync(transactionId))
                       .ReturnsAsync(details);

            // Act
            var result = await controller.GetDetails(transactionId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(details);
        }

        [Fact]
        public async Task GetDetails_Should_Return_NotFound_When_NoDetailsFound()
        {
            // Arrange
            var serviceMock = new Mock<ITransactionService>();
            var validator = new Mock<IValidator<TransactionDTO>>();
            var loggerMock = new Mock<ILogger<TransactionsController>>();

            var controller = new TransactionsController(serviceMock.Object, validator.Object, loggerMock.Object);
            var transactionId = Guid.NewGuid();

            serviceMock.Setup(s => s.GetTransactionDetailsAsync(transactionId))
                       .ReturnsAsync(new List<TransactionDetailDTO>());

            // Act
            var result = await controller.GetDetails(transactionId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
