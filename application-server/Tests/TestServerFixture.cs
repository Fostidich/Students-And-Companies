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

        // Optional test data
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

