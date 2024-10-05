using Microsoft.AspNetCore.Mvc;
using Neo4jExample.Abstractions;
using Neo4jExample.Models;

namespace Neo4jExample.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly ILogger<BookController> _logger;
    private readonly IBookRepository _bookRepository;

    public BookController(ILogger<BookController> logger, IBookRepository bookRepository)
    {
        _logger = logger;
        _bookRepository = bookRepository;
    }

    [HttpGet("{title}")]
    public async Task<IActionResult> GetBookByTitle(string title)
    {
        var book = await _bookRepository.GetBookByTitleAsync(title);
        if (book != null)
        {
            return Ok(book);
        }
        return NotFound("Book not found.");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _bookRepository.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] Book book)
    {
        bool isSuccess = await _bookRepository.CreateBookAsync(book);
        if (isSuccess)
        {
            return Ok("Book created successfully.");
        }
        return BadRequest("Failed to create book.");
    }

    [HttpPut("{title}")]
    public async Task<IActionResult> UpdateBook(string title, [FromBody] Book book)
    {
        bool isSuccess = await _bookRepository.UpdateBookAsync(title, book);
        if (isSuccess)
        {
            return Ok("Book updated successfully.");
        }
        return BadRequest("Failed to update book.");
    }

    [HttpDelete("{title}")]
    public async Task<IActionResult> DeleteBook(string title)
    {
        bool isSuccess = await _bookRepository.DeleteBookAsync(title);
        if (isSuccess)
        {
            return Ok("Book deleted successfully.");
        }
        return BadRequest("Failed to delete book.");
    }
}
