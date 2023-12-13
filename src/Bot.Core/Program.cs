using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var configuration = new ConfigurationBuilder().AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "./test.json")).Build();

Console.WriteLine(configuration["key"]);


// var builder = Host.CreateApplicationBuilder(args);

// builder.Services
// 	.AddHostedService<Worker>();

// var host = builder.Build().;

// host.Run();
