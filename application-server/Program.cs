using System;
using System.IO;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.OpenApi.Models;
using dotenv.net;

Console.WriteLine("Starting application server...");

// Retrieve .env values
DotEnv.Load();

// Manage app settings configuration with dotenv
var builder = WebApplication.CreateBuilder(args);
string jwtSecretEnv = Environment.GetEnvironmentVariable("JWT_SECRET");
if (string.IsNullOrWhiteSpace(jwtSecretEnv)) {
    string jwtSecretStd = builder.Configuration["Jwt:Secret"];
    Console.WriteLine($"No JWT secret found in .env: using \"{jwtSecretStd}\".");
} else {
    builder.Configuration["Jwt:Secret"] = jwtSecretEnv;
}
string dbDefaultConnectionEnv = Environment.GetEnvironmentVariable("DB_DEFAULT_CONNECTION");
if (string.IsNullOrWhiteSpace(dbDefaultConnectionEnv)) {
    string dbDefaultConnectionStd = builder.Configuration["DbDefaultConnection"];
    Console.WriteLine($"No DB default connection found in .env: using \"{dbDefaultConnectionStd}\".");
} else {
    builder.Configuration["DbDefaultConnection"] = dbDefaultConnectionEnv;
}

// Configure console logging
builder.Logging.SetMinimumLevel(LogLevel.Warning);

// Find routes controllers
builder.Services.AddControllers();

// Add interfaces for constructors
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAuthenticationQueries, AuthenticationQueries>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IEnrollmentQueries, EnrollmentQueries>();
builder.Services.AddScoped<IInternshipService, InternshipService>();
builder.Services.AddScoped<IInternshipQueries, InternshipQueries>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<INotificationQueries, NotificationQueries>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IProfileQueries, ProfileQueries>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IRecommendationQueries, RecommendationQueries>();

// Add JWT authentication
string jwtIssuer = builder.Configuration["Jwt:Issuer"];
string jwtAudience = builder.Configuration["Jwt:Audience"];
string jwtSecret = builder.Configuration["Jwt:Secret"];
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

// Add APIs summary web page
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo {
            Title = "Students and Companies - Application server",
            Version = "v1"
        }
    );
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Input: Bearer {token}",
        }
    );
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
            },
            new string[] {}
        }
    });
});

// Build and link application services
var app = builder.Build();
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

// Start server
app.Run();

Console.WriteLine("Stopping application server...");

