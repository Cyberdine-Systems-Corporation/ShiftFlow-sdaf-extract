var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres("postgres")
    .WithDataVolume()
    // 5433 fijo (pgAdmin): evita colisión con PostgreSQL Windows en 5432
    .WithHostPort(5433)
    .AddDatabase("shiftflow");

var api = builder
    .AddProject<Projects.ShiftFlow_Api>("api")
    .WithReference(postgres)
    .WaitFor(postgres)
    .WithExternalHttpEndpoints();

builder
    .AddProject<Projects.ShiftFlow_Web>("web")
    .WithReference(api)
    .WaitFor(api)
    .WithExternalHttpEndpoints();

builder.Build().Run();
