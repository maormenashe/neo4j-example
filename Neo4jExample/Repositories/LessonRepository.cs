
namespace Neo4jExample.Repositories;

public class LessonRepository(IBoltGraphClient client) : ILessonRepository
{
    public async Task<IEnumerable<Lesson>> GetLessonsByCourseIdentifierAsync(string identifier)
    {
        var result = await client.Cypher
            .Match("(:Course {identifier: $identifier})<-[:BELONGS_TO]-(lessons:Lesson)")
            .WithParam("identifier", identifier)
            .Return(lessons => lessons.As<Lesson>())
            .ResultsAsync;

        return result;
    }
}