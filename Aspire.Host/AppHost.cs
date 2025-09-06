
var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Posts_Api>("posts-api");

builder.Build().Run();

var cache = "cache";


var postgres = builder.AddPostgres("postgres").WithDataVolume();
var postsDb = postgres.AddDatabase("postsDb");  

var redis = builder.AddRedis(cache);

var api = builder.AddProject<Projects.Posts_Api>("posts-api")
    .WithReference(postsDb)
    .WithReference(redis);
