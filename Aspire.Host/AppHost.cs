
using Aspire.Hosting;
using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
{
    ["ASPIRE_ALLOW_UNSECURED_TRANSPORT"] = "true" //allows aspire to start on http
});

//TODO use user secrets everywhere

var pgUser = builder.AddParameter("pgUser", "admin");     // not secret is fine
var pgPass = builder.AddParameter("pgPassword", "admin"); // secret: true);

var postgres = builder.AddPostgres("postgres", pgUser, pgPass, 5432)
    .WithDataBindMount("../data/postgres");


var postsDb = postgres.AddDatabase("postsDb");

var redisPass = builder.AddParameter("redisPassword", "admin"); ;
var redis = builder.AddRedis("redis", 6379, redisPass)
    .WithRedisInsight(cfg => cfg.WithHostPort(6380))
    .WithDataBindMount("../data/redis");

var seq = builder.AddSeq("seq", 5341)
    .WithDataBindMount("../data/seq");


var kafka = builder.AddKafka("kafka", 9092)
    .WithKafkaUI(cfg => cfg.WithHostPort(9093))
    .WithDataBindMount("../data/kafka");

//OpenTelementry --> Jaeger (distributed tracing), Grafana + Prometheus (metrics + dashboard), Seq (structured logging)

//TODO implement after
//var jaeger = builder.AddJaeger("jaeger", 6831)
//    .WithDataBindMount("../data/jaeger");

// jaeger - 16686

var api = builder.AddProject<Projects.Posts_Api>("posts-api")
    .WithReference(postsDb)
    .WaitFor(postgres)
    .WithReference(redis)
    .WaitFor(redis)
    .WithReference(seq)
    .WaitFor(seq)
    .WithReference(kafka)
    .WaitFor(kafka);

builder.Build().Run();
