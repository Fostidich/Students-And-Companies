using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

[Collection("Test collection")]
public class EnrollmentTest {

    private readonly HttpClient client;
    private readonly TestFixture.LogInHelper logIn;
    private readonly TestSeed.SeedHelper seed;

    public EnrollmentTest(TestFixture fixture) {
        client = fixture.Client;
        logIn = fixture.LogIn;
        seed = fixture.Seed;
    }

    [Fact]
    public async Task TestCreateApplication200() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInNewStudent();

        // Act
        var response = await client.PostAsJsonAsync($"/api/enrollment/applications/{companyId}", new { QuestionnaireAnswer = "I'm the best!" });

        // Assert
        Assert.Equal(200, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestCreateApplication400InvalidId() {
        // Arrange
        logIn.LogInNewStudent();

        // Act
        var response = await client.PostAsJsonAsync("/api/enrollment/applications/0", new { QuestionnaireAnswer = "I'm the best!" });

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestCreateApplication400InvalidRole() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);

        // Act
        var response = await client.PostAsJsonAsync($"/api/enrollment/applications/{companyId}", new { QuestionnaireAnswer = "I'm the best!" });

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestCreateApplication400NoAdv() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);

        // Act
        await client.PostAsync($"/api/recommendation/delete/{companyId}", null);

        // Arrange
        logIn.LogInNewStudent();

        // Act
        var response = await client.PostAsJsonAsync($"/api/enrollment/applications/{companyId}", new { QuestionnaireAnswer = "I'm the best!" });

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestCreateApplication400AlreadyInInternship() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);

        // Apply and start an internship
        var response = await client.PostAsJsonAsync($"/api/enrollment/applications/{companyId}", new { QuestionnaireAnswer = "I'm the best! pt.1" });
        Assert.Equal(200, (int)response.StatusCode);

        logIn.LogInCompany(companyId);
        response = await client.GetAsync($"/api/enrollment/applications/{companyId}");
        Assert.Equal(200, (int)response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        var obj = JsonSerializer.Deserialize<List<DTO.Application>>(responseBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var date = DateTime.Today;
        date = date.AddYears(1);
        response = await client.PostAsJsonAsync($"/api/enrollment/accept/{obj[0].ApplicationId}", new { DateTime = date });
        responseBody = await response.Content.ReadAsStringAsync();
        Assert.Equal(200, (int)response.StatusCode);

        companyId = seed.GetNewCompanyId();
        logIn.LogInStudent(studentId);

        // Additional tests
        Assert.Equal(studentId, obj[0].StudentId);
        response = await client.GetAsync($"/api/internship");
        responseBody = await response.Content.ReadAsStringAsync();
        var obj2 = JsonSerializer.Deserialize<List<DTO.Internship>>(responseBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Act
        response = await client.PostAsJsonAsync($"/api/enrollment/applications/{companyId}", new { QuestionnaireAnswer = "I'm the best! pt.2" });

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestCreateApplication400AlreadyApplied() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInNewStudent();

        // Act
        var response = await client.PostAsJsonAsync($"/api/enrollment/applications/{companyId}", new { QuestionnaireAnswer = "I'm the best!" });
        Assert.Equal(200, (int)response.StatusCode);
        response = await client.PostAsJsonAsync($"/api/enrollment/applications/{companyId}", new { QuestionnaireAnswer = "I'm the best!" });

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestCreateApplication401() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogOut();

        // Act
        var response = await client.PostAsJsonAsync($"/api/enrollment/applications/{companyId}", new { QuestionnaireAnswer = "I'm the best!" });

        // Assert
        Assert.Equal(401, (int)response.StatusCode);
    }





}
