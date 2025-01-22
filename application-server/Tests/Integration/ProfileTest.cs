using Xunit;
using System.Net.Http;
using System.Threading.Tasks;

[Collection("Test collection")]
public class ProfileTest {

    private readonly HttpClient client;
    private readonly TestFixture.LogInHelper logIn;

    public ProfileTest(TestFixture fixture) {
        client = fixture.Client;
        logIn = fixture.LogIn;
    }

    [Fact]
    public async Task TestGetCompanyFromToken200() {
        // Arrange
        logIn.LogInCompany();

        // Act
        var response = await client.GetAsync("/api/profile/company");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Contains("\"companyId\"", responseBody);
        Assert.Contains("\"username\"", responseBody);
        Assert.Contains("\"email\"", responseBody);
        Assert.Contains("\"bio\"", responseBody);
        Assert.Contains("\"headquarter\"", responseBody);
        Assert.Contains("\"fiscalCode\"", responseBody);
        Assert.Contains("\"vatNumber\"", responseBody);
    }

    [Fact]
    public async Task TestGetCompanyFromToken400() {
        // Arrange
        logIn.LogInStudent();

        // Act
        var response = await client.GetAsync("/api/profile/company");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.DoesNotContain("\"companyId\"", responseBody);
        Assert.DoesNotContain("\"username\"", responseBody);
        Assert.DoesNotContain("\"email\"", responseBody);
        Assert.DoesNotContain("\"bio\"", responseBody);
        Assert.DoesNotContain("\"headquarter\"", responseBody);
        Assert.DoesNotContain("\"fiscalCode\"", responseBody);
        Assert.DoesNotContain("\"vatNumber\"", responseBody);
    }

    [Fact]
    public async Task TestGetCompanyFromToken401() {
        // Arrange
        logIn.LogOut();

        // Act
        var response = await client.GetAsync("/api/profile/company");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(401, (int)response.StatusCode);
        Assert.DoesNotContain("\"companyId\"", responseBody);
        Assert.DoesNotContain("\"username\"", responseBody);
        Assert.DoesNotContain("\"email\"", responseBody);
        Assert.DoesNotContain("\"bio\"", responseBody);
        Assert.DoesNotContain("\"headquarter\"", responseBody);
        Assert.DoesNotContain("\"fiscalCode\"", responseBody);
        Assert.DoesNotContain("\"vatNumber\"", responseBody);
    }


}

