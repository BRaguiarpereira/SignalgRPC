var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.TesteApp_ApiService>("testeapp-apiservice");
builder.AddProject <Projects.TesteGrpcApp_Web>("testegrpcapp-web");

builder.Build().Run();
