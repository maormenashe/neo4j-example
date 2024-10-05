using Neo4j.Driver;

namespace Neo4jExample.Repositories;

public class Neo4jDriverBookRepository(IDriver driver) : IBookRepository
{
    private readonly IDriver _driver = driver;

    public async Task<Book?> GetBookByTitleAsync(string title)
    {
        var query = @"MATCH (b:Book {title: $title}) RETURN b.title AS Title, b.pages AS Pages";

        var session = _driver.AsyncSession();
        try
        {
            var result = await session.RunAsync(query, new { title });
            var record = await result.SingleAsync();

            return record != null ? new Book
            {
                Title = record["Title"].As<string>(),
                Pages = record["Pages"].As<int>()
            } : null;
        }
        catch (Neo4jException ex) when (ex.Code == "Neo.ClientError.Statement.NoSuchNode")
        {
            // Handle case where no records are found
            return null;
        }
        finally
        {
            await session.CloseAsync();
        }
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        var query = @"MATCH (b:Book) RETURN b.title AS Title, b.pages AS Pages";

        var session = _driver.AsyncSession();
        try
        {
            var result = await session.RunAsync(query);
            return await result.ToListAsync(record => new Book
            {
                Title = record["Title"].As<string>(),
                Pages = record["Pages"].As<int>()
            });
        }
        finally
        {
            await session.CloseAsync();
        }
    }

    public async Task<bool> CreateBookAsync(Book book)
    {
        var query = @"CREATE (b:Book {title: $title, pages: $pages})";

        var session = _driver.AsyncSession();
        try
        {
            await session.RunAsync(query, new { title = book.Title, pages = book.Pages });
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            await session.CloseAsync();
        }
    }

    public async Task<bool> UpdateBookAsync(string title, Book book)
    {
        var query = @"MATCH (b:Book {title: $title})
                      SET b.title = $newTitle, b.pages = $newPages
                      RETURN count(b) AS updatedCount";

        var session = _driver.AsyncSession();
        try
        {
            IResultCursor result = await session.RunAsync(query, new { title, newTitle = book.Title, newPages = book.Pages });

            var record = await result.SingleAsync();
            return record["updatedCount"].As<int>() > 0;
        }
        catch
        {
            return false;
        }
        finally
        {
            await session.CloseAsync();
        }
    }

    public async Task<bool> DeleteBookAsync(string title)
    {
        var query = @"MATCH (b:Book {title: $title}) DETACH DELETE b
                      RETURN count(b) AS deletedCount";

        var session = _driver.AsyncSession();
        try
        {
            IResultCursor result = await session.RunAsync(query, new { title });

            var record = await result.SingleAsync();
            return record["deletedCount"].As<int>() > 0;
        }
        catch
        {
            return false;
        }
        finally
        {
            await session.CloseAsync();
        }
    }


}
