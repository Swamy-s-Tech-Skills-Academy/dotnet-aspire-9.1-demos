var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var password = builder.AddParameter("password", secret: true);

var sql = builder.AddSqlServer(name: "sql", password, 1443)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataBindMount(source: @"D:\DataStores\DataVolume");

var sqldb = sql.AddDatabase("sqldb", "master");

var weatherApi = builder.AddProject<Projects.Aspire9xAppDemo_WeatherApi>("weatherapi")
        .WithReference(sqldb)
        .WaitFor(sqldb)
        .WithHttpHealthCheck("/health");

builder.AddProject<Projects.Aspire9xAppDemo_Web>("aspire9xappdemo-web")
        .WithExternalHttpEndpoints()
        .WithReference(cache)
        .WaitFor(cache)
        .WithReference(weatherApi)
        .WaitFor(weatherApi);

await builder.Build().RunAsync().ConfigureAwait(false);
