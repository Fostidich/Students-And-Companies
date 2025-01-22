using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

public class TestFixture : IDisposable {

    public HttpClient Client { get; }
    public LogInHelper LogIn { get; }

    private TestFactory factory;

    public TestFixture() {
        factory = new TestFactory();

        // Create a client to use in tests
        Client = factory.CreateClient();

        // Setup database
        SetupDatabase();

        // Prepare logins
        LogIn = new LogInHelper(Client);
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
        string salt = authentication.GenerateSalt();
        string password = "SeedPassword";
        string hashedPassword = authentication.HashPassword(salt, password);

        var student1 = new Entity.Student {
			Email = "Seed@Student.mail",
			Username = "SeedStudent",
			Salt = salt,
			HashedPassword = hashedPassword,
			Name = "SeedName",
			Surname = "SeedSurname",
			University = "SeedUniversity",
			CourseOfStudy = "SeedCourseOfStudy",
			Gender = 'f',
			BirthDate = new DateTime(2001, 12, 2, 19, 55, 0),
	    };

        var company1 = new Entity.Company {
            Email = "Seed@Company.mail",
            Username = "SeedCompany",
            Salt = salt,
            HashedPassword = hashedPassword,
            Headquarter = "SeedHeadquarter",
            FiscalCode = "SeedFiscalCode",
            VatNumber = "SeedVatNumber",
        };

        // Add entities to context
        context.Student.AddRange(student1);
        context.Company.AddRange(company1);

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

        protected internal LogInHelper(HttpClient client) {
            this.client = client;
            tokenCompany = GetLoginToken("SeedCompany", "SeedPassword");
            tokenStudent = GetLoginToken("SeedStudent", "SeedPassword");
        }

        private string GetLoginToken(string username, string password) {
            var credentials = new {Username = username, Password = password};
            var response = client.PostAsJsonAsync("/api/authentication/login", credentials).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var jsonObject = JObject.Parse(responseBody);
            return jsonObject["token"].ToString();
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
