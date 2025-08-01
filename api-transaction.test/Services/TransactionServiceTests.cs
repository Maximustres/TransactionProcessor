using api_transaction.Acquirers;
using api_transaction.DTOs;
using api_transaction.Entities;
using api_transaction.Enums;
using api_transaction.Interfaces;
using api_transaction.Service;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace api_transaction.test.Services
{
    public class TransactionServiceTests
    {
        private readonly Mock<ITransactionRepository> _repositoryMock;
        private readonly Mock<ITransactionAcquirer> _acquirerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<TransactionService>> _loggerMock;

        private readonly TransactionService _service;

        public TransactionServiceTests()
        {
            _repositoryMock = new Mock<ITransactionRepository>();
            _acquirerMock = new Mock<ITransactionAcquirer>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<TransactionService>>();

            _service = new TransactionService(
                _repositoryMock.Object,
                _acquirerMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task ProcessTransactionAsync_ShouldReturnApprovedStatus()
        {
            // Arrange
            var dto = new TransactionDTO
            {
                Pan = "4111111111111111",
                Expiry = "12/25",
                Amount = 1000,
                Currency = "CLP",
                Cvv = "123",
                MerchantId = "M123"
            };

            var entity = new Transaction
            {
                Id = Guid.NewGuid(),
                Amount = dto.Amount.Value,
                Pan = dto.Pan,
                MerchantId = dto.MerchantId,
                Expiry = dto.Expiry,
                Currency = dto.Currency,
                Cvv = dto.Cvv,
            };

            _mapperMock.Setup(m => m.Map<Transaction>(dto)).Returns(entity);
            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Transaction>())).ReturnsAsync(entity);
            _acquirerMock.Setup(a => a.AuthorizeAsync(It.IsAny<Transaction>())).ReturnsAsync((TransactionStatus.Approved, "00"));

            // Act
            var result = await _service.ProcessTransactionAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be("Approved");
            result.Code.Should().Be("00");

            _repositoryMock.Verify(r => r.AddDetailAsync(It.IsAny<TransactionDetail>()), Times.AtLeast(2));
        }

        [Fact]
        public async Task GetTransactionDetailsAsync_ShouldReturnList()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            var details = new List<TransactionDetail>
            {
                new TransactionDetail { TransactionId = transactionId, Status = TransactionStatus.Pending },
                new TransactionDetail { TransactionId = transactionId, Status = TransactionStatus.Approved }
            };

            _repositoryMock.Setup(r => r.GetDetailsAsync(transactionId)).ReturnsAsync(details);
            _mapperMock.Setup(m => m.Map<List<TransactionDetailDTO>>(details)).Returns(new List<TransactionDetailDTO>
            {
                new TransactionDetailDTO { Status = TransactionStatus.Pending.ToString() },
                new TransactionDetailDTO { Status = TransactionStatus.Approved.ToString() }
            });

            // Act
            var result = await _service.GetTransactionDetailsAsync(transactionId);

            // Assert
            result.Should().HaveCount(2);
            result.First().Status.Should().Be("Pending");
        }
    }
}
