IDistributedApplicationBuilder? builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresDatabaseResource>? postgres = builder
    .AddPostgres("postgres")
    .WithDataVolume()
    // 5433 fijo (pgAdmin): evita colisión con PostgreSQL Windows en 5432
    .WithHostPort(5433)
    .AddDatabase("shiftflow");

IResourceBuilder<ProjectResource>? api = builder
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
