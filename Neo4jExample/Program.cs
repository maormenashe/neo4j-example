using Neo4j.Driver;
using Neo4jExample.Abstractions;
using Neo4jExample.Options;
using Neo4jExample.Repositories;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();

var neo4jOptions = builder.Configuration.GetSection(Neo4jOptions.Section).Get<Neo4jOptions>();
ArgumentNullException.ThrowIfNull(neo4jOptions);

// Example Configure Of - Neo4j .NET .Signed Driver
builder.Services.AddSingleton(GraphDatabase.Driver(
    neo4jOptions.Uri,
    AuthTokens.Basic(neo4jOptions.User, neo4jOptions.Password)));

//// Example Configure Of - Neo4jClient
//builder.Services.AddSingleton<IBoltGraphClient>(provider =>
//{
//    var client = new BoltGraphClient(neo4jOptions.Uri, neo4jOptions.User, neo4jOptions.Password);
//    client.ConnectAsync().Wait(); // Connect asynchronously to the Neo4j database
//    return client;
//});

builder.Services.AddScoped<IBookRepository, Neo4jDriverBookRepository>(); // For Neo4j .NET .Signed Driver


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
