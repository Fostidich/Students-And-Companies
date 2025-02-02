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
        seed.BlackListStudent(companyId);
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

    [Fact]
    public async Task TestGetPendingApplications200() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);

        // Apply and start an internship
        var response = await client.PostAsJsonAsync($"/api/enrollment/applications/{companyId}", new { QuestionnaireAnswer = "I'm the best! pt.1" });
        Assert.Equal(200, (int)response.StatusCode);

        // Act
        logIn.LogInCompany(companyId);
        response = await client.GetAsync($"/api/enrollment/applications/{companyId}");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Contains("\"applicationId\"", responseBody);
        Assert.Contains("\"studentId\"", responseBody);
        Assert.Contains("\"advertisementId\"", responseBody);
        Assert.Contains("\"createdAt\"", responseBody);
        Assert.Contains("\"status\"", responseBody);
        Assert.Contains("\"questionnaire\"", responseBody);
    }

    [Fact]
    public async Task TestGetPendingApplications400InvalidId() {
        // Arrange
        logIn.LogInNewCompany();

        // Act
        var response = await client.GetAsync("/api/enrollment/applications/0");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.DoesNotContain("\"applicationId\"", responseBody);
        Assert.DoesNotContain("\"studentId\"", responseBody);
        Assert.DoesNotContain("\"advertisementId\"", responseBody);
        Assert.DoesNotContain("\"createdAt\"", responseBody);
        Assert.DoesNotContain("\"status\"", responseBody);
        Assert.DoesNotContain("\"questionnaire\"", responseBody);
    }

    [Fact]
    public async Task TestGetPendingApplications400InvalidRole() {
        // Arrange
        logIn.LogInNewStudent();

        // Act
        var response = await client.GetAsync("/api/enrollment/applications/1");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.DoesNotContain("\"applicationId\"", responseBody);
        Assert.DoesNotContain("\"studentId\"", responseBody);
        Assert.DoesNotContain("\"advertisementId\"", responseBody);
        Assert.DoesNotContain("\"createdAt\"", responseBody);
        Assert.DoesNotContain("\"status\"", responseBody);
        Assert.DoesNotContain("\"questionnaire\"", responseBody);
    }

    [Fact]
    public async Task TestGetPendingApplications400NotProprietary() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);

        // Act
        var response = await client.GetAsync($"/api/enrollment/applications/{companyId + 1}");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.DoesNotContain("\"applicationId\"", responseBody);
        Assert.DoesNotContain("\"studentId\"", responseBody);
        Assert.DoesNotContain("\"advertisementId\"", responseBody);
        Assert.DoesNotContain("\"createdAt\"", responseBody);
        Assert.DoesNotContain("\"status\"", responseBody);
        Assert.DoesNotContain("\"questionnaire\"", responseBody);
    }

    [Fact]
    public async Task TestGetPendingApplications401() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogOut();

        // Act
        var response = await client.GetAsync($"/api/enrollment/applications/{companyId}");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(401, (int)response.StatusCode);
        Assert.DoesNotContain("\"applicationId\"", responseBody);
        Assert.DoesNotContain("\"studentId\"", responseBody);
        Assert.DoesNotContain("\"advertisementId\"", responseBody);
        Assert.DoesNotContain("\"createdAt\"", responseBody);
        Assert.DoesNotContain("\"status\"", responseBody);
        Assert.DoesNotContain("\"questionnaire\"", responseBody);
    }

    [Fact]
    public async Task TestGetPendingApplications404NoAdv() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        seed.BlackListStudent(companyId);
        logIn.LogInCompany(companyId);

        // Act
        await client.PostAsync($"/api/recommendation/delete/{companyId}", null);
        var response = await client.GetAsync($"/api/enrollment/applications/{companyId}");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(404, (int)response.StatusCode);
        Assert.DoesNotContain("\"applicationId\"", responseBody);
        Assert.DoesNotContain("\"studentId\"", responseBody);
        Assert.DoesNotContain("\"advertisementId\"", responseBody);
        Assert.DoesNotContain("\"createdAt\"", responseBody);
        Assert.DoesNotContain("\"status\"", responseBody);
        Assert.DoesNotContain("\"questionnaire\"", responseBody);
    }

    [Fact]
    public async Task TestGetPendingApplications404NoApplication() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);

        // Act
        var response = await client.GetAsync($"/api/enrollment/applications/{companyId}");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(404, (int)response.StatusCode);
        Assert.DoesNotContain("\"applicationId\"", responseBody);
        Assert.DoesNotContain("\"studentId\"", responseBody);
        Assert.DoesNotContain("\"advertisementId\"", responseBody);
        Assert.DoesNotContain("\"createdAt\"", responseBody);
        Assert.DoesNotContain("\"status\"", responseBody);
        Assert.DoesNotContain("\"questionnaire\"", responseBody);
    }

    [Fact]
    public async Task TestAcceptApplication200() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);

        // Apply for internship
        var response = await client.PostAsJsonAsync($"/api/enrollment/applications/{companyId}", new { QuestionnaireAnswer = "I'm the best! pt.1" });
        Assert.Equal(200, (int)response.StatusCode);

        logIn.LogInCompany(companyId);
        response = await client.GetAsync($"/api/enrollment/applications/{companyId}");
        Assert.Equal(200, (int)response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        var obj = JsonSerializer.Deserialize<List<DTO.Application>>(responseBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Act
        var date = DateTime.Today;
        date = date.AddYears(1);
        response = await client.PostAsJsonAsync($"/api/enrollment/accept/{obj[0].ApplicationId}", new { DateTime = date });

        // Assert
        Assert.Equal(200, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestAcceptApplication400InvalidId() {
        // Arrange
        logIn.LogInNewCompany();

        // Act
        var date = DateTime.Today;
        date = date.AddYears(1);
        var response = await client.PostAsJsonAsync("/api/enrollment/accept/0", new { DateTime = date });

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestAcceptApplication400InvalidDate() {
        // Arrange
        logIn.LogInNewCompany();

        // Act
        var date = DateTime.Today;
        date = date.AddYears(-1);
        var response = await client.PostAsJsonAsync("/api/enrollment/accept/1", new { DateTime = date });

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestAcceptApplication400InvalidRole() {
        // Arrange
        logIn.LogInNewStudent();

        // Act
        var date = DateTime.Today;
        date = date.AddYears(1);
        var response = await client.PostAsJsonAsync("/api/enrollment/accept/1", new { DateTime = date });

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestAcceptApplication400AlreadyAccepted() {
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

        // Act
        var date = DateTime.Today;
        date = date.AddYears(1);
        response = await client.PostAsJsonAsync($"/api/enrollment/accept/{obj[0].ApplicationId}", new { DateTime = date });
        Assert.Equal(200, (int)response.StatusCode);
        response = await client.PostAsJsonAsync($"/api/enrollment/accept/{obj[0].ApplicationId}", new { DateTime = date });

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestAcceptedApplication400AlreadyInInternship() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        int companyId2 = seed.GetNewCompanyId();
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);

        // Apply for two advertisements
        var response = await client.PostAsJsonAsync($"/api/enrollment/applications/{companyId}", new { QuestionnaireAnswer = "I'm the best! pt.1" });
        Assert.Equal(200, (int)response.StatusCode);
        response = await client.PostAsJsonAsync($"/api/enrollment/applications/{companyId2}", new { QuestionnaireAnswer = "I'm the best! pt.1" });
        Assert.Equal(200, (int)response.StatusCode);

        // Accept the first internship
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

        // Accept the second internship
        logIn.LogInCompany(companyId2);
        response = await client.GetAsync($"/api/enrollment/applications/{companyId2}");
        Assert.Equal(200, (int)response.StatusCode);
        responseBody = await response.Content.ReadAsStringAsync();
        obj = JsonSerializer.Deserialize<List<DTO.Application>>(responseBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        date = DateTime.Today;
        date = date.AddYears(1);
        response = await client.PostAsJsonAsync($"/api/enrollment/accept/{obj[0].ApplicationId}", new { DateTime = date });
        responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestAcceptApplication401() {
        // Arrange
        logIn.LogOut();

        // Act
        var date = DateTime.Today;
        date = date.AddYears(1);
        var response = await client.PostAsJsonAsync("/api/enrollment/accept/1", new { DateTime = date });

        // Assert
        Assert.Equal(401, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestAcceptApplication404NoApplication() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);

        // Act
        var date = DateTime.Today;
        date = date.AddYears(1);
        var response = await client.PostAsJsonAsync($"/api/enrollment/accept/{companyId + 1}", new { DateTime = date });

        // Assert
        Assert.Equal(404, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestRejectApplication200() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);

        // Apply for internship
        var response = await client.PostAsJsonAsync($"/api/enrollment/applications/{companyId}", new { QuestionnaireAnswer = "I'm the best! pt.1" });
        Assert.Equal(200, (int)response.StatusCode);

        logIn.LogInCompany(companyId);
        response = await client.GetAsync($"/api/enrollment/applications/{companyId}");
        Assert.Equal(200, (int)response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        var obj = JsonSerializer.Deserialize<List<DTO.Application>>(responseBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Act
        response = await client.PostAsync($"/api/enrollment/reject/{obj[0].ApplicationId}", null);

        // Assert
        Assert.Equal(200, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestRejectApplication400InvalidId() {
        // Arrange
        logIn.LogInNewCompany();

        // Act
        var response = await client.PostAsync("/api/enrollment/reject/0", null);

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestRejectApplication400InvalidRole() {
        // Arrange
        logIn.LogInNewStudent();

        // Act
        var response = await client.PostAsync("/api/enrollment/reject/1", null);

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestRejectApplication400AlreadyAccepted() {
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

        // Act
        var date = DateTime.Today;
        date = date.AddYears(1);
        response = await client.PostAsJsonAsync($"/api/enrollment/accept/{obj[0].ApplicationId}", new { DateTime = date });
        Assert.Equal(200, (int)response.StatusCode);
        response = await client.PostAsync($"/api/enrollment/reject/{obj[0].ApplicationId}", null);

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestRejectApplication401() {
        // Arrange
        logIn.LogOut();

        // Act
        var response = await client.PostAsync("/api/enrollment/reject/1", null);

        // Assert
        Assert.Equal(401, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestRejectApplication404NoApplication() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);

        // Act
        var response = await client.PostAsync($"/api/enrollment/reject/{companyId + 1}", null);

        // Assert
        Assert.Equal(404, (int)response.StatusCode);
    }

}
