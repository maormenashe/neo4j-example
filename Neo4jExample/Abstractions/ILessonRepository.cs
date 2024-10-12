namespace Neo4jExample.Abstractions;

public interface ILessonRepository
{
    Task<IEnumerable<Lesson>> GetLessonsByCourseIdentifierAsync(string identifier);
}