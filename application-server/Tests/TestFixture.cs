using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;

public class TestFixture : IDisposable {

    public HttpClient Client { get; }
    public LogInHelper LogIn { get; }

    private readonly TestFactory factory;
    private readonly string password = "SeedPassword";

    public TestFixture() {
        factory = new TestFactory();

        // Create a client to use in tests
        Client = factory.CreateClient(new WebApplicationFactoryClientOptions {
            BaseAddress = new Uri("https://localhost:5001")
        });

        // Setup database
        SetupDatabase();

        // Prepare logins
        LogIn = new LogInHelper(Client, password);
    }

    private void SetupDatabase() {
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Delete and recreate the database
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Retrieve services
        var authentication = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();

        // Add test data for testing
        var seed = new TestSeed(authentication, password);
        seed.SeedDatabase(context);

        // Save changes to the database
        context.SaveChanges();
    }

    public void Dispose() {
        // Cleanup the database
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        Client.Dispose();
        factory.Dispose();
    }

    public class LogInHelper {

        private readonly HttpClient client;
        private readonly string tokenCompany;
        private readonly string tokenStudent;
        private readonly string password;

        protected internal LogInHelper(HttpClient client, string password) {
            this.client = client;
            this.password = password;
            tokenCompany = GetLoginToken("SeedCompany", password);
            tokenStudent = GetLoginToken("SeedStudent", password);
        }

        private string GetLoginToken(string username, string password) {
            var credentials = new {Username = username, Password = password};
            var response = client.PostAsJsonAsync("/api/authentication/login", credentials).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var jsonObject = JObject.Parse(responseBody);
            return jsonObject["token"].ToString();
        }

        public void LogIn(string username, string password) {
            var token = GetLoginToken(username, password);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void LogIn(string username) {
            var token = GetLoginToken(username, password);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void LogInCompany() {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenCompany);
        }

        public void LogInStudent() {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenStudent);
        }

        public void LogOut() {
            client.DefaultRequestHeaders.Authorization = null;
        }

    }

}
