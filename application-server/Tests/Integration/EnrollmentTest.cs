using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

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
        int studentId = seed.GetNewStudentId();
        logIn.LogInStudent(studentId);

        // Act
        var response = await client.PostAsJsonAsync($"/api/enrollment/applications/{companyId}", new { QuestionnaireAnswer = "I'm the best!" });

        // Assert
        Assert.Equal(200, (int)response.StatusCode);
    }

}
