using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

public class TestServerFixture : IDisposable {

    public HttpClient Client { get; }
    public IServiceProvider Services { get; }

    private readonly TestWebApplicationFactory factory;

    public TestServerFixture() {
        factory = new TestWebApplicationFactory();

        // Create a client to use in tests
        Client = factory.CreateClient();

        // Access services for DB setup and teardown
        Services = factory.Services;

        // Setup database
        SetupDatabase();
    }

    private void SetupDatabase() {
        using var scope = Services.CreateScope();
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
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        Client.Dispose();
        factory.Dispose();
    }

}

