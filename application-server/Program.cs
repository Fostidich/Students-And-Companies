using dotenv.net;
using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.Extensions.Logging;

Console.WriteLine("Starting application server....\n");

// Retrieve .env values
DotEnv.Load();
// TODO put .env values in builder configuration

// Load DB connection string
DataService.LoadDefaultConnection();

// Find routes controllers
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Configure console logging
builder.Logging.SetMinimumLevel(LogLevel.Warning);

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

// Add JWT authentication
string jwtIssuer = "students_and_companies_application_server";
string jwtAudience = "students_and_companies_web_server";
string jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
if (string.IsNullOrEmpty(jwtSecret)) {
    jwtSecret = "SeCEVerOSfig1R9MzN2eGuqJ4uRtDV1y";
    Console.WriteLine($"No JWT secret found in .env: using \"{jwtSecret}\".");
}
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))};
});
builder.Services.AddAuthorization();

// Add keys data protection
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "students_and_companies", "keys")))
    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration() {
        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
        ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
    });

// Build and start application
var app = builder.Build();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.Run();

Console.WriteLine("\nStopping application server....");

