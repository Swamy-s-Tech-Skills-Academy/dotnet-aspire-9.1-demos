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

builder.AddAzureFunctionsProject<Projects.Aspire9xAppDemo_AzFuncApp>("aspire9xappdemo-azfuncapp");

builder.AddNpmApp("angular", "../AspireJavaScript.Angular")
    .WithReference(weatherApi)
    .WaitFor(weatherApi)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.AddNpmApp("react", "../AspireJavaScript.React")
    .WithReference(weatherApi)
    .WaitFor(weatherApi)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.AddNpmApp("vue", "../AspireJavaScript.Vue")
    .WithReference(weatherApi)
    .WaitFor(weatherApi)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

await builder.Build().RunAsync().ConfigureAwait(false);
