var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("password", secret: true);

var sqldb = builder.AddSqlServer(name: "sql", password, 1443)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataBindMount(source: @"D:\DataStores\DataVolume");

builder.AddProject<Projects.Aspire9xAppDemo_WeatherApi>("aspire9xappdemo-weatherapi")
        .WithReference(sqldb)
        .WaitFor(sqldb);

await builder.Build().RunAsync();
