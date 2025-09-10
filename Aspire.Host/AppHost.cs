
using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);


//TODO use user secrets everywhere

var pgUser = builder.AddParameter("pgUser", "admin");                    // not secret is fine
var pgPass = builder.AddParameter("pgPassword", "admin"); // secret: true);

var postgres = builder.AddPostgres("postgres", pgUser, pgPass, 5432)
    //.WithEnvironment("POSTGRES_USER", "admin")
    //.WithEnvironment("POSTGRES_PASSWORD", "admin")
    .WithDataBindMount("../data/postgres");

//var postgres = builder.AddPostgres("postgres");

var postsDb = postgres.AddDatabase("postsDb");

var redisPass = builder.AddParameter("redisPassword", "admin"); ;
var redis = builder.AddRedis("cache", 6379, redisPass)
    .WithDataBindMount("../data/redis");
    //.WithEnvironment("REDIS_PASSWORD", "redis"));

//var redis = builder.AddRedis("cache");

// kafka - 9092
// Seq - 5341
// jaeger - 16686

var api = builder.AddProject<Projects.Posts_Api>("posts-api")
    .WithReference(postsDb)
    .WithReference(redis)
    .WaitFor(postgres)
    .WaitFor(redis);

builder.Build().Run();
