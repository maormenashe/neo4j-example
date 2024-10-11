using Neo4jClient.Cypher;

namespace Neo4jExample.Repositories;

public class CourseRepository(IBoltGraphClient client) : ICourseRepository
{
    public async Task<IEnumerable<Course>> GetAllCoursesAsync()
    {
        var result = await client.Cypher
            .Match("(lesson:Lesson)-[:BELONGS_TO]->(course:Course)") // Correct direction and syntax
            .Return((course, lesson) => new
            {
                Course = course.As<Course>(),
                Lessons = Return.As<List<Lesson>>("COLLECT(lesson)") // Collect all related lessons
            })
            .ResultsAsync;

        // Assign the lessons directly to each course and return
        return result.Select(r =>
        {
            r.Course.lessons = r.Lessons;
            return r.Course;
        }).ToList();
    }

    public async Task<Course?> GetCourseByIdentifierAsync(string identifier)
    {
        var result = await client.Cypher
          .Match("(c:Course {identifier: $identifier})")
          .WithParam("identifier", identifier)
          .Return(c => c.As<Course>())
          .ResultsAsync;

        return result.SingleOrDefault();
    }
}

