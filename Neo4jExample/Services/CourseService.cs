namespace Neo4jExample.Services;

public class CourseService(ICourseRepository courseRepository)
{
    public async Task<IEnumerable<Course>> GetAllCoursesAsync()
    {
        return await courseRepository.GetAllCoursesAsync();
    }

    public async Task<Course?> GetCourseByIdentifierAsync(string identifier)
    {
        return await courseRepository.GetCourseByIdentifierAsync(identifier);
    }
    public async Task CreateBelongsToRelationAsync(string courseIdentifier, string lessonIdentifier, BelongsTo relationshipData)
    {
        await courseRepository.CreateBelongsToRelationAsync(courseIdentifier, lessonIdentifier, relationshipData);
    }

    public async Task<CourseWithLessons?> GetCourseWithLessonsAsync(string courseIdentifier)
    {
        return await courseRepository.GetCourseWithLessonsAsync(courseIdentifier);
    }
    public async Task<IEnumerable<CourseWithLessons>> GetAllCoursesWithLessonsAsync()
    {
        return await courseRepository.GetAllCoursesWithLessonsAsync();
    }
}
