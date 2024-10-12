namespace Neo4jExample.Services;

public class LessonService(ILessonRepository lessonsRepository)
{
    public async Task<IEnumerable<Lesson>> GetLessonsByCourseIdentifierAsync(string identifier)
    {
        return await lessonsRepository.GetLessonsByCourseIdentifierAsync(identifier);
    }
}