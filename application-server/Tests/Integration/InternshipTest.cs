using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using System.Text.Json;

[Collection("Test collection")]
public class InternshipTest
{

    private readonly HttpClient client;
    private readonly TestFixture.LogInHelper logIn;
    private readonly TestSeed.SeedHelper seed;
    private readonly ITestOutputHelper _output;

    public InternshipTest(TestFixture fixture, ITestOutputHelper output)
    {
        client = fixture.Client;
        logIn = fixture.LogIn;
        seed = fixture.Seed;
        _output = output;
    }

    [Fact]
    public async Task GetInternshipForStudent200()
    {
        // Arrange
        logIn.LogInNewStudent();

        // Act
        var response = await client.GetAsync("api/internship");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Contains("\"internshipId\"", responseBody);
        Assert.Contains("\"createdAt\"", responseBody);
        Assert.Contains("\"studentId\"", responseBody);
        Assert.Contains("\"companyId\"", responseBody);
        Assert.Contains("\"advertisementId\"", responseBody);
        Assert.Contains("\"startDate\"", responseBody);
        Assert.Contains("\"endDate\"", responseBody);
    }

    [Fact]
    public async Task GetInternshipForStudent400() {
        // Arrange
        logIn.LogInNewCompany();

        // Act
        var response = await client.GetAsync("api/internship");

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task GetDeletedInternshipForStudent404() {
        // Arrange
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);

        // Act
        await client.PostAsync($"api/internship/delete/{studentId}", null);
        var response = await client.GetAsync("api/internship");

        // Assert
        Assert.Equal(404, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteInternshipFromStudent200() {
        // Arrange
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);
        
        // Act
        var response = await client.PostAsync($"api/internship/delete/{studentId}", null);
        
        // Assert
        Assert.Equal(200, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteInternshipFromStudent404() {
        // Arrange
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);
        
        int internshipId = studentId + 1;
        
        // Act
        var response = await client.PostAsync($"api/internship/delete/{internshipId}", null);
        
        // Assert
        Assert.Equal(404, (int)response.StatusCode);
    }
    
    [Fact ]
    public async Task DeleteInternshipFromCompany200() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);
        
        // Act
        var response = await client.PostAsync($"api/internship/delete/{companyId}", null);
        
        // Assert
        Assert.Equal(200, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteInternshipFromCompany404() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);
        
        int internshipId = companyId + 1;
        
        // Act
        var response = await client.PostAsync($"api/internship/delete/{internshipId}", null);
        
        // Assert
        Assert.Equal(404, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetInternshipFromAdvertisement200() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);

        // Act
        var response = await client.GetAsync($"api/internship/advertisements/{companyId}");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Contains("\"internshipId\"", responseBody);
        Assert.Contains("\"createdAt\"", responseBody);
        Assert.Contains("\"studentId\"", responseBody);
        Assert.Contains("\"companyId\"", responseBody);
        Assert.Contains("\"advertisementId\"", responseBody);
        Assert.Contains("\"startDate\"", responseBody);
        Assert.Contains("\"endDate\"", responseBody);
    }
    
    [Fact]
    public async Task GetInternshipFromAdvertisement400() {
        // Arrange
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);

        // Act
        var response = await client.GetAsync($"api/internship/advertisements/{studentId}");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.Contains("Invalid role", responseBody);
    }
    
    [Fact]
    public async Task GetInternshipFromAdvertisementNotYour400() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);
        
        int advertisementId = companyId + 1;

        // Act
        var response = await client.GetAsync($"api/internship/advertisements/{advertisementId}");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.Contains("You don't have the permission to see this internship", responseBody);
    }
    
    [Fact]
    public async Task DeleteStudentFeedback200() {
        // Arrange
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);
        
        _output.WriteLine("Student ID: " + studentId);
        
        // Act
        var response = await client.PostAsync($"api/internship/delete/feedback/{studentId}", null);
        
        // Assert
        Assert.Equal(200, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteStudentFeedbackNotYour404() {
        // Arrange
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);
        
        int internshipId = studentId + 1;
        
        // Act
        var response = await client.PostAsync($"api/internship/delete/feedback/{internshipId}", null);
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(404, (int)response.StatusCode);
        Assert.Contains("You don't have this feedback", responseBody);
    }
    
    [Fact]
    public async Task DeleteStudentFeedbackNotPresent404() {
        // Arrange
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);
        
        // Act
        await client.PostAsync($"api/internship/delete/feedback/{studentId}", null);
        var response2 = await client.PostAsync($"api/internship/delete/feedback/{studentId}", null);
        var responseBody = await response2.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(404, (int)response2.StatusCode);
        Assert.Contains("You don't have this feedback", responseBody);
    }
    
    [Fact]
    public async Task DeleteCompanyFeedback200() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);
        
        // Act
        var response = await client.PostAsync($"api/internship/delete/feedback/{companyId}", null);
        
        // Assert
        Assert.Equal(200, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteCompanyFeedbackNotYour404() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);
        
        int internshipId = companyId + 1;
        
        // Act
        var response = await client.PostAsync($"api/internship/delete/feedback/{internshipId}", null);
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(404, (int)response.StatusCode);
        Assert.Contains("You don't have this feedback", responseBody);
    }
    
    [Fact]
    public async Task GetStudentFeedbackFromStudent200() {
        // Arrange
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);
        
        // Act
        var response = await client.GetAsync($"api/internship/{studentId}/feedback/student");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        _output.WriteLine("Response: " + responseBody);
        
        // Assert
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Contains("\"rating\"", responseBody);
        Assert.Contains("\"comment\"", responseBody);
    }
    
    [Fact]
    public async Task GetStudentFeedbackFromStudent400() {
        // Arrange
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);
        
        int internshipId = studentId + 1;
        
        // Act
        var response = await client.GetAsync($"api/internship/{internshipId}/feedback/student");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.Contains("You don't have the permission to see this feedback or the feedback doesn't exist yet", responseBody);
    }
    
    [Fact]
    public async Task GetStudentFeedbackThatNotExistFromStudent400() {
        // Arrange
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);
        
        // Act
        await client.PostAsync($"api/internship/delete/feedback/{studentId}", null);
        var response2 = await client.GetAsync($"api/internship/{studentId}/feedback/student");
        var responseBody = await response2.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(400, (int)response2.StatusCode);
        Assert.Contains("You don't have the permission to see this feedback or the feedback doesn't exist yet", responseBody);
    }
    
    [Fact]
    public async Task GetStudentFeedbackFromCompany200() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);
        
        // Act
        var response = await client.GetAsync($"api/internship/{companyId}/feedback/student");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        _output.WriteLine("Response: " + companyId);
        
        // Assert
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Contains("\"rating\"", responseBody);
        Assert.Contains("\"comment\"", responseBody);
    }
    
    [Fact]
    public async Task GetStudentFeedbackFromCompany400() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);
        
        int internshipId = companyId + 1;
        
        // Act
        var response = await client.GetAsync($"api/internship/{internshipId}/feedback/student");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.Contains("You don't have the permission to see this feedback or the feedback doesn't exist yet", responseBody);
    }
    
    [Fact]
    public async Task GetCompanyFeedbackFromStudent200() {
        // Arrange
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);
        
        // Act
        var response = await client.GetAsync($"api/internship/{studentId}/feedback/company");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Contains("\"rating\"", responseBody);
        Assert.Contains("\"comment\"", responseBody);
    }
    
    [Fact]
    public async Task GetCompanyFeedbackFromStudent404() {
        // Arrange
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);
        
        int internshipId = studentId + 1;
        
        // Act
        var response = await client.GetAsync($"api/internship/{internshipId}/feedback/company");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.Contains("You don't have the permission to see this feedback or the feedback doesn't exist yet", responseBody);
    }
    
    [Fact]
    public async Task GetCompanyFeedbackFromCompany200() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);
        
        // Act
        var response = await client.GetAsync($"api/internship/{companyId}/feedback/company");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Contains("\"rating\"", responseBody);
        Assert.Contains("\"comment\"", responseBody);
    }
    
    [Fact]
    public async Task GetCompanyFeedbackFromCompany400() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);
        
        int internshipId = companyId + 1;
        
        // Act
        var response = await client.GetAsync($"api/internship/{internshipId}/feedback/company");
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.Contains("You don't have the permission to see this feedback or the feedback doesn't exist yet", responseBody);
    }
    
    [Theory]
    [InlineData(10, "Great internship", 200)]
    [InlineData(1, "Bad internship", 200)]
    [InlineData(3, "", 400)]
    [InlineData(15, "Great internship", 400)]
    [InlineData(0, "Very Bad internship", 400)]
    [InlineData(1, null, 400)]
    [InlineData(-10, "Great internship", 400)]
    public async Task CreateStudentFeedback200(
            int rating, 
            string comment,
            int statusCode
    ) {
        // Arrange
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);
        
        var feedback = new {
            Rating = rating,
            Comment = comment
        };
        
        // Act
        await client.PostAsync($"api/internship/delete/feedback/{studentId}", null);
        var response = await client.PostAsJsonAsync($"api/internship/{studentId}/feedback/student", feedback);
        
        // Assert
        Assert.Equal(statusCode, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task CreateStudentFeedbackAlreadyPresent400() {
        // Arrange
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);
        
        var feedback = new {
            rating = 5,
            comment = "Great internship"
        };
        
        // Act
        var response = await client.PostAsJsonAsync($"api/internship/{studentId}/feedback/student", feedback);
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.Contains("You can't create this feedback, and there are 3 possible reasons: " +
                        "1. The internship isn't finished yet; " +
                        "2. You don't have the permission to create this feedback; " +
                        "3. The feedback already exists\n", responseBody);
    }
    
    [Fact]
    public async Task CreateStudentFeedbackNotYour400() {
        // Arrange
        int studentId = seed.GetNewStudentId();
        int studentId2 = seed.GetNewStudentId();
        logIn.LogInStudent(studentId2);
        
        var feedback = new {
            rating = 5,
            comment = "Great internship"
        };
        
        // Act
        await client.PostAsync($"api/internship/delete/feedback/{studentId2}", null);
        
        logIn.LogInStudent(studentId);
        
        var response = await client.PostAsJsonAsync($"api/internship/{studentId2}/feedback/student", feedback);
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.Contains("You can't create this feedback, and there are 3 possible reasons: " +
                        "1. The internship isn't finished yet; " +
                        "2. You don't have the permission to create this feedback; " +
                        "3. The feedback already exists\n", responseBody);
    }
    
    [Fact]
    public async Task CreateStudentFeedbackFromCompany400() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);
        
        var feedback = new {
            rating = 5,
            comment = "Great internship"
        };
        
        // Act
        var response = await client.PostAsJsonAsync($"api/internship/{companyId}/feedback/student", feedback);
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.Contains("Invalid role\n", responseBody);
    }
    
    [Theory]
    [InlineData(10, "Great internship", 200)]
    [InlineData(1, "Bad internship", 200)]
    [InlineData(3, "", 400)]
    [InlineData(15, "Great internship", 400)]
    [InlineData(0, "Very Bad internship", 400)]
    [InlineData(1, null, 400)]
    [InlineData(-10, "Great internship", 400)]
    public async Task CreateCompanyFeedback200(
        int rating,
        string comment,
        int statusCode
    ) {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);
        
        var feedback = new {
            Rating = rating,
            Comment = comment
        };
        
        // Act
        await client.PostAsync($"api/internship/delete/feedback/{companyId}", null);
        var response = await client.PostAsJsonAsync($"api/internship/{companyId}/feedback/company", feedback);
        
        // Assert
        Assert.Equal(statusCode, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task CreateCompanyFeedbackAlreadyPresent400() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId);
        
        var feedback = new {
            rating = 5,
            comment = "Great internship"
        };
        
        // Act
        var response = await client.PostAsJsonAsync($"api/internship/{companyId}/feedback/company", feedback);
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.Contains("You can't create this feedback, and there are 3 possible reasons: " +
                        "1. The internship isn't finished yet; " +
                        "2. You don't have the permission to create this feedback; " +
                        "3. The feedback already exists\n", responseBody);
    }
    
    [Fact]
    public async Task CreateCompanyFeedbackNotYour400() {
        // Arrange
        int companyId = seed.GetNewCompanyId();
        int companyId2 = seed.GetNewCompanyId();
        logIn.LogInCompany(companyId2);
        
        var feedback = new {
            rating = 5,
            comment = "Great internship"
        };
        
        // Act
        await client.PostAsync($"api/internship/delete/feedback/{companyId2}", null);
        
        logIn.LogInCompany(companyId);
        
        var response = await client.PostAsJsonAsync($"api/internship/{companyId2}/feedback/company", feedback);
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.Contains("You can't create this feedback, and there are 3 possible reasons: " +
                        "1. The internship isn't finished yet; " +
                        "2. You don't have the permission to create this feedback; " +
                        "3. The feedback already exists\n", responseBody);
    }
    
    [Fact]
    public async Task CreateCompanyFeedbackFromStudent400() {
        // Arrange
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);
        
        var feedback = new {
            rating = 5,
            comment = "Great internship"
        };
        
        // Act
        var response = await client.PostAsJsonAsync($"api/internship/{studentId}/feedback/company", feedback);
        var responseBody = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.Contains("Invalid role\n", responseBody);
    }
    
    
}
