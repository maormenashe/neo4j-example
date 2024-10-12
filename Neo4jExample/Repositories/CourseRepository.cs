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

    public async Task CreateBelongsToRelationAsync(string courseIdentifier, string lessonIdentifier, BelongsTo relationshipData)
    {
        await client.Cypher
         .Match("(c:Course {identifier: $courseIdentifier})", "(l:Lesson {identifier: $lessonIdentifier})")
         .Create("(l)-[r:BELONGS_TO $relationshipData]->(c)")
         .WithParams(new
         {
             courseIdentifier,
             lessonIdentifier,
             relationshipData
         })
         .ExecuteWithoutResultsAsync();
    }

    public async Task<IEnumerable<CourseWithLessons>> GetAllCoursesWithLessonsAsync()
    {
        var result = await client.Cypher
            .Match("(c:Course)<-[r:BELONGS_TO]-(l:Lesson)")
            .Return((c, l, r) => new
            {
                Course = c.As<Course>(),
                Lesson = l.As<Lesson>(),
                Relationship = r.As<BelongsTo>()
            })
            .ResultsAsync;

        // Group lessons and relationships by course ID
        var coursesWithLessons = result
            .GroupBy(x => x.Course.identifier) // Use the Course ID as the key
            .Select(group => new CourseWithLessons
            {
                Course = group.First().Course, // Get the first course in the group
                Lessons = group.Select(g => new LessonWithRelationship
                {
                    Lesson = g.Lesson,
                    Relationship = g.Relationship
                }).ToList()
            })
            .ToList(); // Call ToList() to finalize the query execution

        return coursesWithLessons;
    }

    public async Task<CourseWithLessons?> GetCourseWithLessonsAsync(string courseIdentifier)
    {
        var result = await client.Cypher
            .Match("(c:Course {identifier: $courseIdentifier})<- [r:BELONGS_TO]-(l:Lesson)")
            .WithParam("courseIdentifier", courseIdentifier)
            .Return((c, l, r) => new
            {
                Course = c.As<Course>(),
                Lesson = l.As<Lesson>(),
                Relationship = r.As<BelongsTo>()
            })
            .ResultsAsync;

        var firstResult = result.FirstOrDefault();

        if (firstResult == null)
        {
            // Handle the case when no results are found
            return null; // Or throw an exception, or return a default object
        }

        // Combine the results into a single CourseWithLessons object
        var courseWithLessons = new CourseWithLessons
        {
            Course = firstResult.Course,
            Lessons = result.Select(g => new LessonWithRelationship
            {
                Lesson = g.Lesson,
                Relationship = g.Relationship
            }).ToList()
        };

        return courseWithLessons;
    }
}

