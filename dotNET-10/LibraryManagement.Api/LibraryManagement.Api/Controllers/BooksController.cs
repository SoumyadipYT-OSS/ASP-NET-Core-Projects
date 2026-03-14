using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc; 


namespace LibraryManagement.Api.Controllers; 


[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase 
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService) 
    {
        _bookService = bookService;
    }

    // GET: api/books
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BooksDto>>> GetAll() 
    {
        var books = await _bookService.GetAllAsync();
        return Ok(books);
    }

    // GET: api/books/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<BooksDto>> GetById(int id) 
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book == null)
            return NotFound(new { Message = "Book not found" });

        return Ok(book);
    }

    // POST: api/books
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateBookRequest request) 
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _bookService.AddAsync(request);
        if (result > 0)
            return CreatedAtAction(nameof(GetById), new { id = result }, request);

        return BadRequest(new { Message = "Failed to create book" });
    }

    // PUT: api/books/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] BooksDto bookDto) 
    {
        if (id != bookDto.BookId)
            return BadRequest(new { Message = "Book ID mismatch" });

        var result = await _bookService.UpdateAsync(bookDto);
        if (result > 0)
            return NoContent();

        return NotFound(new { Message = "Book not found" });
    }

    // DELETE: api/books/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id) 
    {
        var result = await _bookService.DeleteAsync(id);
        if (result > 0)
            return NoContent();

        return NotFound(new { Message = "Book not found" });
    }
}
