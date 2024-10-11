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
}
