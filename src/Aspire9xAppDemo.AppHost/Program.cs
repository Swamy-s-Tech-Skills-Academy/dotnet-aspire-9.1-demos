var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("password", secret: true);

var sql = builder.AddSqlServer(name: "sql", password, 1443)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataBindMount(source: @"D:\DataStores\DataVolume");

var sqldb = sql.AddDatabase("sqldb", "master");

builder.AddProject<Projects.Aspire9xAppDemo_WeatherApi>("weatherapi")
        .WithReference(sqldb)
        .WaitFor(sqldb);

await builder.Build().RunAsync();
