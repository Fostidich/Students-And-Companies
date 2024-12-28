using System;
using dotenv.net;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Starting application server....\n");

// Retrieve .env values
DotEnv.Load();

// Find routes controllers and start application
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();
app.UseAuthorization();
app.Run();

Console.WriteLine("\nStopping application server....");

