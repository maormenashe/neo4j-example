namespace Neo4jExample.Abstractions;

public interface ICourseRepository
{
    Task<IEnumerable<Course>> GetAllCoursesAsync();
    Task<Course?> GetCourseByIdentifierAsync(string identifier);
}
