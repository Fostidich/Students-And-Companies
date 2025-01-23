using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;

public class TestFixture : IDisposable {

    public HttpClient Client { get; }
    public LogInHelper LogIn { get; private set; }
    public TestSeed.SeedHelper Seed { get; }

    private readonly TestFactory factory;

    public TestFixture() {
        factory = new TestFactory();

        // Create a client to use in tests
        Client = factory.CreateClient(new WebApplicationFactoryClientOptions {
            BaseAddress = new Uri("https://localhost:5001")
        });

        // Retrieve services
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var authentication = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();

        // Prepare logins
        var seeder = new TestSeed(authentication);
        Seed = seeder.Seed;
        LogIn = new LogInHelper(Client, seeder.Password, Seed);

        // Setup database
        SetupDatabase(context, seeder);
    }

    private void SetupDatabase(AppDbContext context, TestSeed seed) {
        // Delete and recreate the database
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Seed database
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
        private readonly string password;
        private readonly TestSeed.SeedHelper seed;

        protected internal LogInHelper(HttpClient client, string password, TestSeed.SeedHelper seed) {
            this.client = client;
            this.password = password;
            this.seed = seed;
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

        public void LogInCompany(int id) {
            var token = GetLoginToken(seed.GetCompanyUsername(id), password);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void LogInStudent(int id) {
            var token = GetLoginToken(seed.GetStudentUsername(id), password);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void LogInNewCompany() {
            var token = GetLoginToken(seed.GetNewCompanyUsername(), password);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void LogInNewStudent() {
            var token = GetLoginToken(seed.GetNewStudentUsername(), password);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void LogOut() {
            client.DefaultRequestHeaders.Authorization = null;
        }

    }

}
