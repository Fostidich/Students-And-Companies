using System.Collections.Generic;
using System.Linq;
using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Xunit.Abstractions;

[Collection("Test collection")]
public class NotificationTest {

    private readonly HttpClient client;
    private readonly TestFixture.LogInHelper logIn;
    private readonly TestSeed.SeedHelper seed;
    private readonly ITestOutputHelper _output;

    public NotificationTest(TestFixture fixture, ITestOutputHelper output)
    {
        client = fixture.Client;
        logIn = fixture.LogIn;
        seed = fixture.Seed;
        _output = output;
    }

    [Fact]
    public async Task GetNotificationsFromToken200() {
        // Arrange
        logIn.LogInNewStudent();
        
        // Act
        var response = await client.GetAsync("api/notification");
        var responseBody = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseBody);
        
        // Assert
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Contains("\"studentNotificationId\"", responseBody);
        Assert.Contains("\"studentId\"", responseBody);
        Assert.Contains("\"advertisementId\"", responseBody);
        Assert.Contains("\"type\"", responseBody);
    }
    
    [Fact]
    public async Task GetNotificationsFromToken400() {
        // Arrange
        logIn.LogInNewCompany();
        
        // Act
        var response = await client.GetAsync("api/notification");
        
        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetNotificationsFromToken404() {
        // Arrange
        logIn.LogInNewStudent();
        
        var options = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        };
        
        // Act
        var firstResponse = await client.GetAsync("api/notification");
        var responseBody = await firstResponse.Content.ReadAsStringAsync();
        
        List<int> notificationIds = new List<int>();
        
        List<DTO.StudentNotifications> notifications = JsonSerializer.Deserialize<List<DTO.StudentNotifications>>(responseBody, options);
        
        foreach(var notification in notifications) {
            notificationIds.Add(notification.StudentNotificationId);
        }
        
        foreach(var notificationId in notificationIds) {
            await client.PostAsync($"api/notification/delete/{notificationId}" , null);
        }
        
        var response = await client.GetAsync("api/notification");
        
        //assert
        Assert.Equal(404, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteNotification200() {
        // Arrange
        logIn.LogInNewStudent();
        
        var options = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        };
        
        // Act
        var firstResponse = await client.GetAsync("api/notification");
        var responseBody = await firstResponse.Content.ReadAsStringAsync();
        
        List<int> notificationIds = new List<int>();
        
        List<DTO.StudentNotifications> notifications = JsonSerializer.Deserialize<List<DTO.StudentNotifications>>(responseBody, options);
        
        foreach(var notification in notifications) {
            notificationIds.Add(notification.StudentNotificationId);
        }

        var notificationId = notificationIds.FirstOrDefault();
        var response2 = await client.PostAsync($"api/notification/delete/{notificationId}" , null);
        
        //assert
        Assert.Equal(200, (int)response2.StatusCode);
    }
    
    [Fact]
    public async Task DeleteNotification400() {
        // Arrange
        logIn.LogInNewCompany();
        
        // Act
        var response = await client.PostAsync("api/notification/delete/1" , null);
        
        //assert
        Assert.Equal(400, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteNotification404() {
        // Arrange
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);
        
        int notificationId = studentId == 1 ? 4 : 1;
        
        // Act
        var response = await client.PostAsync($"api/notification/delete/{notificationId}" , null);
        
        //assert
        Assert.Equal(404, (int)response.StatusCode);
    }
}

