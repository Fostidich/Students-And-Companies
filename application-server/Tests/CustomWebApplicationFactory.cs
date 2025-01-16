using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;

public class TestWebApplicationFactory : WebApplicationFactory<Program>, IDisposable {

    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        builder.ConfigureAppConfiguration((context, config) => {
        // Retrieve the original connection string
        var originalConnectionString = config.Build()["DbDefaultConnection"];

        // Modify the connection string by prepending "test_" to the database name
        if (!string.IsNullOrEmpty(originalConnectionString)) {
            var newConnectionString = ModifyConnectionStringForTest(originalConnectionString);
            // Override the connection string in the configuration
            config.AddInMemoryCollection(new Dictionary<string, string> {
                { "DbDefaultConnection", newConnectionString }
            });
            Console.WriteLine($"[Tests] Overriding DB default connection: using \"{newConnectionString}\"");
        }});
    }

    private string ModifyConnectionStringForTest(string originalConnectionString) {
        // Assuming the original format is "Server=localhost;Database=some_db;User ID=some_id;Password=;"
        var connectionStringParts = originalConnectionString.Split(';');
        for (int i = 0; i < connectionStringParts.Length; i++) {
            if (connectionStringParts[i].StartsWith("Database=", StringComparison.OrdinalIgnoreCase)) {
                // Prepend "test_" to the database name
                connectionStringParts[i] = "Database=test_" + connectionStringParts[i].Substring("Database=".Length);
                break;
            }
        }
        return string.Join(";", connectionStringParts);
    }

}

