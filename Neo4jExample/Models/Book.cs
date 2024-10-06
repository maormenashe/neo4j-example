namespace Neo4jExample.Models;

public class Book
{
    public Book()
    {
    }

    public string? Title { get; set; }
    public int Pages { get; set; }
}

public class BookCamelCase
{
    public BookCamelCase()
    {
    }

    public string? title { get; set; }
    public int pages { get; set; }
}
