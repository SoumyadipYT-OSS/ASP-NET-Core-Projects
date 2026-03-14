using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Models;
using LibraryManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class BorrowController : ControllerBase 
{
    private readonly IBorrowService _borrowService;

    public BorrowController(IBorrowService borrowService) 
    {
        _borrowService = borrowService;
    }

    // POST: api/borrow
    [HttpPost]
    public async Task<ActionResult<BorrowResponseDto>> BorrowBook([FromBody] BorrowRequestDto request) 
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _borrowService.BorrowBookAsync(request);
        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }

    // PUT: api/borrow/{transactionId}/return
    [HttpPut("{transactionId}/return")]
    public async Task<ActionResult<BorrowResponseDto>> ReturnBook(int transactionId) 
    {
        var response = await _borrowService.ReturnBookAsync(transactionId);
        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }

    // GET: api/borrow
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BorrowTransactions>>> GetAllTransactions() 
    {
        var transactions = await _borrowService.GetAllTransactionsAsync();
        return Ok(transactions);
    }

    // GET: api/borrow/{transactionId}
    [HttpGet("{transactionId}")]
    public async Task<ActionResult<BorrowTransactions>> GetTransactionById(int transactionId) 
    {
        var transaction = await _borrowService.GetTransactionByIdAsync(transactionId);
        if (transaction == null)
            return NotFound(new { Message = "Transaction not found" });

        return Ok(transaction);
    }
}
