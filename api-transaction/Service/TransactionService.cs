using api_transaction.Acquirers;
using api_transaction.Controllers;
using api_transaction.DTOs;
using api_transaction.Entities;
using api_transaction.Enums;
using api_transaction.Interfaces;
using api_transaction.Repositories;
using AutoMapper;
using Polly;

namespace api_transaction.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionAcquirer _transactionAcquirer;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(
            ITransactionRepository transactionRepository, 
            ITransactionAcquirer transactionAcquirer,
            IMapper mapper,
            ILogger<TransactionService> logger)
        {
            _transactionAcquirer = transactionAcquirer;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<TransactionDetailDTO>> GetTransactionDetailsAsync(Guid transactionId)
        {
            var detailTransaction = await _transactionRepository.GetDetailsAsync(transactionId);
            return _mapper.Map<List<TransactionDetailDTO>>(detailTransaction);
        }

        public async Task<TransactionResponseDTO> ProcessTransactionAsync(TransactionDTO request)
        {
            var transactionRequest = _mapper.Map<Transaction>(request);

            var transactionSaved = await _transactionRepository.AddAsync(transactionRequest);

            await _transactionRepository.AddDetailAsync(new TransactionDetail
            {
                TransactionId = transactionSaved.Id.Value,
                Status = TransactionStatus.Pending,
                Observation = "Init registry"
            });

            _logger.LogInformation("The transaction with ID: {Id} has been registered as pending.", transactionSaved.Id);

            var policyContext = new Context
            {
                ["transactionId"] = transactionSaved.Id
            };

            var retryPolicy = Policy
                .HandleResult<(TransactionStatus, string)>(r => r.Item1 == TransactionStatus.Timeout)
                .RetryAsync(3, onRetryAsync: async (outcome, retryCount, context) =>
                {
                    if (context.TryGetValue("transactionId", out var objId) && objId is Guid transactionId)
                    {
                        _logger.LogInformation("The transaction with id: {TransactionId} is being retried.", transactionId);

                        await _transactionRepository.AddDetailAsync(new TransactionDetail
                        {
                            TransactionId = transactionId,
                            Status = TransactionStatus.Pending,
                            Observation = $"Retry #{retryCount} by code: {outcome.Result.Item2}",
                            AuthorizationCode = outcome.Result.Item2
                        });
                    }
                });


            var (status, code) = await retryPolicy.ExecuteAsync(
                async ctx => await _transactionAcquirer.AuthorizeAsync(transactionSaved),
                policyContext
            );

            await _transactionRepository.AddDetailAsync(new TransactionDetail
            {
                TransactionId = transactionSaved.Id.Value,
                Status = status,
                AuthorizationCode = code,
                Observation = "Transaction processed"
            });

            _logger.LogInformation("The transaction process with ID: {Id} has been completed with status: {Status}.", transactionSaved.Id, status.ToString());

            return new TransactionResponseDTO
            {
                Code = code,
                Status = status.ToString(),
            };
        }
    }
}
