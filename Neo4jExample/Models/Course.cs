namespace Neo4jExample.Models;

public class Course
{
    public long Id { get; set; }
    public string? Identifier { get; set; }
    public string? Title { get; set; }
    public string? Teacher { get; set; }

    public Course()
    {
    }
}
