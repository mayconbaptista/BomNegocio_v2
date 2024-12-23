var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Order_WebApi>("order-webapi");

builder.Build().Run();
