namespace Neo4jExample.Abstractions;

public interface ICourseRepository
{
    Task<IEnumerable<Course>> GetAllCoursesAsync();
    Task<Course?> GetCourseByIdentifierAsync(string identifier);
    Task CreateBelongsToRelationAsync(string courseIdentifier, string lessonIdentifier, BelongsTo relationshipData);
    Task<IEnumerable<CourseWithLessons>> GetAllCoursesWithLessonsAsync();
    Task<CourseWithLessons?> GetCourseWithLessonsAsync(string courseIdentifier);
}
