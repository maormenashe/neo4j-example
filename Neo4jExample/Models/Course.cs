namespace Neo4jExample.Models;

public class Course
{
    public long id { get; set; }
    public string? identifier { get; set; }
    public string? title { get; set; }
    public string? teacher { get; set; }

    public Course()
    {
    }
}
