﻿using Neo4jClient;

namespace Neo4jExample.Repositories;

public class Neo4jClientBookRepository(IBoltGraphClient client) : IBookRepository
{
    private readonly IBoltGraphClient _client = client;

    public async Task<Book?> GetBookByTitleAsync(string title)
    {
        var result = await _client.Cypher
          .Match("(b:Book {title: $title})")
          .WithParam("title", title)
          .Return(b => b.As<BookCamelCase>())
          .ResultsAsync;

        BookCamelCase targetBook = result.SingleOrDefault()!;

        return new Book()
        {
            Title = targetBook.title,
            Pages = targetBook.pages,
        };
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        var result = await _client.Cypher
            .Match("(b:Book)")
            .Return(b => b.As<BookCamelCase>())
            .ResultsAsync;

        return result.Select(r => new Book
        {
            Title = r.title,
            Pages = r.pages
        }).ToList();
    }

    public async Task<bool> CreateBookAsync(Book book)
    {
        var result = await _client.Cypher
            .Create("(b:Book {title: $title, pages: $pages})")
            .WithParams(new { title = book.Title, pages = book.Pages })
            .Return(b => b.CollectAs<BookCamelCase>())
            .ResultsAsync;

        return result.Any(); // Returns true if at least one book was created
    }

    public async Task<bool> UpdateBookAsync(string title, Book book)
    {
        var result = await _client.Cypher
            .Match("(b:Book {title: $title})")
            .Set("b.title = $newTitle, b.pages = $newPages")
            .WithParams(new { title, newTitle = book.Title, newPages = book.Pages })
            .Return(b => b.CollectAs<BookCamelCase>())
            .ResultsAsync;

        return result.Any(); // Returns true if the update affected at least one book
    }

    public async Task<bool> DeleteBookAsync(string title)
    {
        var result = await _client.Cypher
            .Match("(b:Book {title: $title})")
            .DetachDelete("b")
            .WithParam("title", title)
            .Return(b => b.CollectAs<BookCamelCase>())
            .ResultsAsync;

        return result.Any(); // Returns true if the delete operation affected at least one book
    }
}
