using api_transaction.DTOs;
using api_transaction.Entities;
using api_transaction.Interfaces;
using api_transaction.Middleware;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace api_transaction.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IValidator<TransactionDTO> _validator;
        private readonly ILogger<TransactionsController> _logger;
        public TransactionsController(
            ITransactionService transactionService, 
            IValidator<TransactionDTO> validator,
            ILogger<TransactionsController> logger)
        {
            _transactionService = transactionService;
            _validator = validator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TransactionDTO request)
        {
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var response = await _transactionService.ProcessTransactionAsync(request);
            return Ok(response);
        }

        [HttpGet("{id:guid}/details")]
        public async Task<IActionResult> GetDetails(Guid id)
        {
            _logger.LogInformation("Information about the transaction has been requested: {Id}", id);
            var details = await _transactionService.GetTransactionDetailsAsync(id);
            if (details.Count == 0) return NotFound();
            return Ok(details);
        }

    }
}
