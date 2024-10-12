namespace Neo4jExample.Models;

public class Course
{
    public long id { get; set; }
    public string? identifier { get; set; }
    public string? title { get; set; }
    public string? teacher { get; set; }
    public List<Lesson> lessons { get; set; } = [];


    public Course()
    {
    }
}

public class CourseWithLessons
{
    public Course? Course { get; set; }
    public List<LessonWithRelationship>? Lessons { get; set; }
}

public class LessonWithRelationship
{
    public Lesson? Lesson { get; set; }
    public BelongsTo? Relationship { get; set; }
}