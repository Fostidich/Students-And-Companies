using System;
using dotenv.net;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Starting application server....\n");

// Retrieve .env values
DotEnv.Load();

// Load DB connection string
DataService.LoadDefaultConnection();

// Find routes controllers
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Add interfaces to scope
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAuthenticationQueries, AuthenticationQueries>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IEnrollmentQueries, EnrollmentQueries>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IFeedbackQueries, FeedbackQueries>();
builder.Services.AddScoped<IInternshipService, InternshipService>();
builder.Services.AddScoped<IInternshipQueries, InternshipQueries>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<INotificationQueries, NotificationQueries>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IProfileQueries, ProfileQueries>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IRecommendationQueries, RecommendationQueries>();

// Build and start application
var app = builder.Build();
app.MapControllers();
app.UseAuthorization();
app.Run();

Console.WriteLine("\nStopping application server....");

