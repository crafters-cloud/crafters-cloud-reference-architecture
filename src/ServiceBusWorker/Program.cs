using CraftersCloud.ReferenceArchitecture.ServiceBusWorker;
using CraftersCloud.ReferenceArchitecture.ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.AddAzureServiceBusClient("sbemulator");

builder.Services.AddHostedService<Consumer>();
builder.Services.AddHostedService<Producer>();

var host = builder.Build();

await host.RunAsync();
