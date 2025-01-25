using System.Collections.Generic;
using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;

[Collection("Test collection")]
public class NotificationTest {

    private readonly HttpClient client;
    private readonly TestFixture.LogInHelper logIn;
    private readonly TestSeed.SeedHelper seed;

    public NotificationTest(TestFixture fixture)
    {
        client = fixture.Client;
        logIn = fixture.LogIn;
        seed = fixture.Seed;
    }

    [Fact]
    public async Task GetNotificationsFromToken200() {
        // Arrange
        logIn.LogInNewStudent();
        
        // Act
        var response = await client.GetAsync("api/notification");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Contains("\"studentNotificationId\"", responseBody);
        Assert.Contains("\"studentId\"", responseBody);
        Assert.Contains("\"advertisementId\"", responseBody);
        Assert.Contains("\"type\"", responseBody);
    }
    
    [Fact]
    public async Task GetNotificationsFromId200() {
        // Arrange
        logIn.LogInStudent(210);
        
        // Act
        var response = await client.GetAsync("api/notification");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(404, (int)response.StatusCode);
    }
}

