using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

[Collection("Test collection")]
public class RecommendationTest {
    
    private readonly HttpClient client;
    private readonly TestFixture.LogInHelper logIn;
    private readonly TestSeed.SeedHelper seed;
    private readonly ITestOutputHelper _output;

    public RecommendationTest(TestFixture fixture, ITestOutputHelper output) {
        client = fixture.Client;
        logIn = fixture.LogIn;
        seed = fixture.Seed;
        _output = output;
    }

    [Fact]
    public async Task GetRecommendationsFromTokenForStudent200() {
        // Arrange
        logIn.LogInNewStudent();
        
        // Act
        var response = await client.GetAsync("api/recommendation/advertisements");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Contains("\"advertisementId\"", responseBody);
        Assert.Contains("\"name\"", responseBody);
        Assert.Contains("\"createdAt\"", responseBody);
        Assert.Contains("\"companyId\"", responseBody);
        Assert.Contains("\"description\"", responseBody);
        Assert.Contains("\"duration\"", responseBody);
        Assert.Contains("\"spots\"", responseBody);
        Assert.Contains("\"available\"", responseBody);
        Assert.Contains("\"open\"", responseBody);
        Assert.Contains("\"questionnaire\"", responseBody);
    }
    
    [Fact]
    public async Task GetRecommendationsFromTokenForCompany200() {
        // Arrange
        logIn.LogInNewCompany();
        
        // Act
        var response = await client.GetAsync("api/recommendation/advertisements");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Contains("\"advertisementId\"", responseBody);
        Assert.Contains("\"name\"", responseBody);
        Assert.Contains("\"createdAt\"", responseBody);
        Assert.Contains("\"companyId\"", responseBody);
        Assert.Contains("\"description\"", responseBody);
        Assert.Contains("\"duration\"", responseBody);
        Assert.Contains("\"spots\"", responseBody);
        Assert.Contains("\"available\"", responseBody);
        Assert.Contains("\"open\"", responseBody);
        Assert.Contains("\"questionnaire\"", responseBody);
    }
    
    [Fact]
    public async Task GetRecommendationsFromTokenForCompany404() {
        // Arrange
        int id = seed.GetNewCompanyId();
        logIn.LogInCompany(id);
        
        // Act
        await client.PostAsync($"api/recommendation/delete/{id}", null);
        var response = await client.GetAsync("api/recommendation/advertisements");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(404, (int)response.StatusCode);
        Assert.Contains("No advertisements found", responseBody);
    }
    
    [Fact]
    public async Task DeleteAdvertisement200() {
        // Arrange
        int id = seed.GetNewCompanyId();
        logIn.LogInCompany(id);
        
        // Act
        var response = await client.PostAsync($"api/recommendation/delete/{id}", null);;
        
        // Assert
        Assert.Equal(200, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteAdvertisement400() {
        // Arrange
        int id = seed.GetNewStudentId();
        logIn.LogInStudent(id);
        
        // Act
        var response = await client.PostAsync($"api/recommendation/delete/{id}", null);
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.Contains("Invalid role", responseBody);
    }
    
    [Fact]
    public async Task DeleteAdvertisement404() {
        // Arrange
        int id = seed.GetNewCompanyId();
        logIn.LogInCompany(id);
        
        // Act
        var response = await client.PostAsync($"api/recommendation/delete/{id + 1}", null);
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(404, (int)response.StatusCode);
        Assert.Contains("You don't have this advertisement", responseBody);
    }
    
    [Fact]
    public async Task GetYourAdvertisementFromIdForCompany200() {
        // Arrange
        int id = seed.GetNewCompanyId();
        logIn.LogInCompany(id);
        
        // Act
        var response = await client.GetAsync($"api/recommendation/advertisements/{id}");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Contains("\"advertisementId\"", responseBody);
        Assert.Contains("\"name\"", responseBody);
        Assert.Contains("\"createdAt\"", responseBody);
        Assert.Contains("\"companyId\"", responseBody);
        Assert.Contains("\"description\"", responseBody);
        Assert.Contains("\"duration\"", responseBody);
        Assert.Contains("\"spots\"", responseBody);
        Assert.Contains("\"available\"", responseBody);
        Assert.Contains("\"open\"", responseBody);
        Assert.Contains("\"questionnaire\"", responseBody);
    }
    
    [Fact]
    public async Task GetNotYourAdvertisementFromIdForCompany200() {
        // Arrange
        int id = seed.GetNewStudentId();
        logIn.LogInStudent(id);
        
        int advertisementId = id + 1;
        
        // Act
        var response = await client.GetAsync($"api/recommendation/advertisements/{advertisementId}");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Contains("\"advertisementId\"", responseBody);
        Assert.Contains("\"name\"", responseBody);
        Assert.Contains("\"createdAt\"", responseBody);
        Assert.Contains("\"companyId\"", responseBody);
        Assert.Contains("\"description\"", responseBody);
        Assert.Contains("\"duration\"", responseBody);
        Assert.Contains("\"spots\"", responseBody);
        Assert.Contains("\"available\"", responseBody);
        Assert.Contains("\"open\"", responseBody);
        Assert.Contains("\"questionnaire\"", responseBody);
    }
    
    [Fact]
    public async Task GetAdvertisementFromIdForStudent200() {
        // Arrange
        int id = seed.GetNewStudentId();
        logIn.LogInStudent(id);
        
        // Act
        var response = await client.GetAsync("api/recommendation/advertisements/1");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Contains("\"advertisementId\"", responseBody);
        Assert.Contains("\"name\"", responseBody);
        Assert.Contains("\"createdAt\"", responseBody);
        Assert.Contains("\"companyId\"", responseBody);
        Assert.Contains("\"description\"", responseBody);
        Assert.Contains("\"duration\"", responseBody);
        Assert.Contains("\"spots\"", responseBody);
        Assert.Contains("\"available\"", responseBody);
        Assert.Contains("\"open\"", responseBody);
        Assert.Contains("\"questionnaire\"", responseBody);
    }
    
    [Fact]
    public async Task GetYourAdvertisementFromIdForCompany404() {
        // Arrange
        int id = seed.GetNewCompanyId();
        logIn.LogInCompany(id);
        
        // Act
        await client.PostAsync($"api/recommendation/delete/{id}", null);
        var response = await client.GetAsync($"api/recommendation/advertisements/{id}");
        
        // Assert
        Assert.Equal(404, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetAdvertisementFromIdForStudent404() {
        // Arrange
        int studentId = seed.GetNewStudentId();
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);
        
        // Act
        await client.PostAsync($"api/recommendation/delete/{companyId}", null);
        
        logIn.LogInStudent(studentId);
        
        var response = await client.GetAsync($"api/recommendation/advertisements/{companyId}");
        
        
        // Assert
        Assert.Equal(404, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetNotYourAdvertisementFromIdForCompany404() {
        // Arrange
        int companyId1 = seed.GetNewCompanyId();
        int companyId2 = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId1);
        
        // Act
        await client.PostAsync($"api/recommendation/delete/{companyId1}", null);
        
        logIn.LogInCompany(companyId2);
        
        var response = await client.GetAsync($"api/recommendation/advertisements/{companyId1}");
        
        // Assert
        Assert.Equal(404, (int)response.StatusCode);
    }
    
    [Theory]
    [InlineData("name", "Description", 10, 10, "Questionnaire", new string[] { "Skill1", "Skill2" }, 200)]
    [InlineData("name", "Description", 10, 10, "Questionnaire", new string[] { "Skill1" }, 200)]
    [InlineData("name", "Description", 10, 10, "Questionnaire", new string[] { "" }, 200)]
    [InlineData("name", "Description", 10, 10, "Questionnaire", new string[] {}, 200)]
    [InlineData("", "Description", 10, 10, "Questionnaire", new string[] { "Skill1", "Skill2" }, 400)]
    [InlineData("name", "", 10, 10, "Questionnaire", new string[] { "Skill1", "Skill2" }, 400)]
    [InlineData("name", "Description", 0, 10, "Questionnaire", new string[] { "Skill1", "Skill2" }, 400)]
    [InlineData("name", "Description", 10, 0, "Questionnaire", new string[] { "Skill1", "Skill2" }, 400)]
    [InlineData("name", "Description", 10, 10, "", new string[] { "Skill1", "Skill2" }, 400)]
    [InlineData("name", "Description", 10, 10, "Questionnaire", null, 400)]
    [InlineData(null, "Description", 10, 10, "Questionnaire", new string[] { "Skill1", "Skill2" }, 400)]
    [InlineData("name", null, 10, 10, "Questionnaire", new string[] { "Skill1", "Skill2" }, 400)]
    [InlineData("name", "Description", -1, 10, "Questionnaire", new string[] { "Skill1", "Skill2" }, 400)]
    [InlineData("name", "Description", 10, -1, "Questionnaire", new string[] { "Skill1", "Skill2" }, 400)]
    [InlineData("name", "Description", 10, 10, null, new string[] { "Skill1", "Skill2" }, 400)]
    public async Task CreateAdvertisement200(
        string name,
        string description, 
        int duration,
        int spots,
        string questionnaire,
        string[] skills,
        int statusCode
    ) {
        // Arrange
        logIn.LogInNewCompany();
        
        var advertisement = new {
            Name = name,
            Description = description,
            Duration = duration,
            Spots = spots,
            Questionnaire = questionnaire,
            Skills = skills
        };
        
        // Act
        var response = await client.PostAsJsonAsync("api/recommendation/advertisements", advertisement);
        
        // Assert
        Assert.Equal(statusCode, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task CreateAdvertisement400() {
        // Arrange
        logIn.LogInNewCompany();
        
        var advertisement = new {
            Name = "name",
            Description = "Description",
            Duration = 10,
            Spots = 10,
            Questionnaire = "Questionnaire",
            Skills = new string[] { "Skill1", "Skill2" }
        };
        
        // Act
        await client.PostAsJsonAsync("api/recommendation/advertisements", advertisement);
        var response = await client.PostAsJsonAsync("api/recommendation/advertisements", advertisement);
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.Contains("invalid registration", responseBody);
    }
    
    [Fact]
    public async Task CreateAdvertisementFromStudent400() {
        // Arrange
        logIn.LogInNewStudent();
        
        var advertisement = new {
            Name = "new",
            Description = "Description",
            Duration = 10,
            Spots = 10,
            Questionnaire = "Questionnaire",
            Skills = new string[] { "Skill1", "Skill2" }
        };
        
        // Act
        var response = await client.PostAsJsonAsync("api/recommendation/advertisements", advertisement);
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.Contains("Invalid role", responseBody);
    }

    [Fact]
    public async Task GetRecommendedCandidates200()
    {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);

        // Act
        var response = await client.GetAsync($"api/recommendation/candidates/advertisements/{companyId}");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Contains("\"studentId\"", responseBody);
        Assert.Contains("\"username\"", responseBody);
        Assert.Contains("\"email\"", responseBody);
        Assert.Contains("\"bio\"", responseBody);
        Assert.Contains("\"name\"", responseBody);
        Assert.Contains("\"surname\"", responseBody);
        Assert.Contains("\"university\"", responseBody);
        Assert.Contains("\"courseOfStudy\"", responseBody);
        Assert.Contains("\"gender\"", responseBody);
        Assert.Contains("\"birthDate\"", responseBody);
    }
    
    [Fact]
    public async Task GetRecommendedCandidatesFromStudent400() {
        // Arrange
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);

        // Act
        var response = await client.GetAsync($"api/recommendation/candidates/advertisements/{studentId}");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.Contains("Invalid role", responseBody);
    }
    
    [Fact]
    public async Task GetRecommendedCandidatesFromAdvThatNotExist400() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);
        
        // Act
        await client.PostAsync($"api/recommendation/delete/{companyId}", null);
        var response = await client.GetAsync($"api/recommendation/candidates/advertisements/{companyId}");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.Contains("You are not the owner of the advertisement or it doesn't exist", responseBody);
    }

    [Fact]
    public async Task GetRecommendedCandidatesFromAdvNotYour400() {
        // Arrange
        int companyId1 = seed.GetNewCompanyId();
        int advetisementId = companyId1 + 1;
        logIn.LogInCompany(companyId1);
        
        // Act
        var response = await client.GetAsync($"api/recommendation/candidates/advertisements/{advetisementId}");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.Contains("You are not the owner of the advertisement or it doesn't exist", responseBody);
    }

    [Fact]
    public async Task GetRecommendedCandidates404() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);
        
        var advertisement = new {
            Name = "new",
            Description = "Description",
            Duration = 10,
            Spots = 10,
            Questionnaire = "Questionnaire",
            Skills = new string[] {}
        };
        
        var options = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        };
        
        // Act
        await client.PostAsync($"api/recommendation/delete/{companyId}", null);
        var response = await client.PostAsJsonAsync("api/recommendation/advertisements", advertisement);
        
        Assert.Equal(200, (int)response.StatusCode);
        
        var response2 = await client.GetAsync("api/recommendation/advertisements");
        var responseBody = await response2.Content.ReadAsStringAsync();
        
        var advertisements = JsonSerializer.Deserialize<List<DTO.Advertisement>>(responseBody, options);
        
        var advId = advertisements[0].AdvertisementId;
    
        var response3 = await client.GetAsync($"api/recommendation/candidates/advertisements/{advId}");
        var responseBody2 = await response3.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(404, (int)response3.StatusCode);
        Assert.Contains("No students found", responseBody2);
    }
    
    [Fact]
    public async Task CreateSuggestionForOneStudentFromCompany200() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);
        
        int studentId = seed.GetNewStudentId();
        
        // Act
        var response = await client.PostAsync($"api/recommendation/suggestions/advertisement/{companyId}/student/{studentId}", null);
        
        // Assert
        Assert.Equal(200, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task CreateSuggestionForOneStudentFromStudent400() {
        // Arrange
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);
        
        int companyId = seed.GetNewCompanyId();
        
        // Act
        var response = await client.PostAsync($"api/recommendation/suggestions/advertisement/{companyId}/student/{studentId}", null);
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.Contains("Invalid role", responseBody);
    }
    
    [Fact]
    public async Task CreateSuggestionForOneStudentFromCompany404() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);
        
        int studentId = seed.GetNewStudentId();
        
        // Act
        await client.PostAsync($"api/recommendation/delete/{companyId}", null);
        var response = await client.PostAsync($"api/recommendation/suggestions/advertisement/{companyId}/student/{studentId}", null);
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(404, (int)response.StatusCode);
        Assert.Contains("Student or advertisement not found", responseBody);
    }
    
    [Fact]
    public async Task CreateSuggestionForOneStudentFromCompanyAdvNotYour404() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);
        
        int advertisementId = companyId + 1;
        
        int studentId = seed.GetNewStudentId();
        
        // Act
        var response = await client.PostAsync($"api/recommendation/suggestions/advertisement/{advertisementId}/student/{studentId}", null);
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(404, (int)response.StatusCode);
        Assert.Contains("Student or advertisement not found", responseBody);
    }

    [Fact]
    public async Task CreateSuggestionForOneStudentFromCompanyStudentNotExist404() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);
        
        // Act
        await client.PostAsync($"api/profile/delete", null);

        logIn.LogInCompany(companyId);
        
        var response = await client.PostAsync($"api/recommendation/suggestions/advertisement/{companyId}/student/{studentId}", null);
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(404, (int)response.StatusCode);
        Assert.Contains("Student or advertisement not found", responseBody);
    }

    [Fact]
    public async Task CheckIfSuggestionIsCreated200() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);
        
        int studentId = seed.GetNewStudentId();
        
        // Act
        await client.PostAsync($"api/recommendation/suggestions/advertisement/{companyId}/student/{studentId}", null);

        logIn.LogInStudent(studentId);
        
        var response = await client.GetAsync("api/notification");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        
        // Assert
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Contains("INVITED", responseBody);
    }
    
    
    [Fact]
    public async Task CheckIfRecommendedSuggestionIsCreated200() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        
        int studentId = seed.GetNewStudentId();
        
        var skills = new string[] { "Skill"+studentId, "Skill"+studentId+1, "Skill"+studentId+2, "Skill"+studentId+3, "Skill"+studentId+4 };
        
        var advertisement = new {
            Name = "new",
            Description = "Description",
            Duration = 10,
            Spots = 10,
            Questionnaire = "Questionnaire",
            Skills = new string[] { "Skill"+studentId, "Skill"+studentId+1, "Skill"+studentId+2, "Skill"+studentId+3, "Skill"+studentId+4 }
        };
        
        var options = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        };
        
        logIn.LogInStudent(studentId);
        
        
        // Act
        await client.PostAsJsonAsync("/api/profile/skills", skills);
        
        
        logIn.LogInCompany(companyId);
        
        await client.PostAsync($"api/recommendation/delete/{companyId}", null);
        await client.PostAsJsonAsync("api/recommendation/advertisements", advertisement);
        
        var response = await client.GetAsync("api/recommendation/advertisements");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        var advertisements = JsonSerializer.Deserialize<List<DTO.Advertisement>>(responseBody, options);
        
        var advId = advertisements[0].AdvertisementId;

        logIn.LogInStudent(studentId);
        
        var response2 = await client.GetAsync("api/notification");
        var responseBody2 = await response2.Content.ReadAsStringAsync();
        
        var notifications = JsonSerializer.Deserialize<List<DTO.StudentNotifications>>(responseBody2, options);
        
        _output.WriteLine(notifications.Count.ToString());

        
        // Assert
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Contains(advId.ToString(), responseBody2);
    }
}