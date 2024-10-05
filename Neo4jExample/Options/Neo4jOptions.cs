namespace Neo4jExample.Options;

public class Neo4jOptions
{
    public const string Section = "Neo4j";

    public string Uri { get; init; } = string.Empty;
    public string User { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
