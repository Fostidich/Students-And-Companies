using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;

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

    [Fact]
    public async Task TestGetStudentFromToken200() {
        // Arrange
        logIn.LogInStudent();

        // Act
        var response = await client.GetAsync("/api/profile/student");
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
    public async Task TestGetStudentFromToken400() {
        // Arrange
        logIn.LogInCompany();

        // Act
        var response = await client.GetAsync("/api/profile/student");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.DoesNotContain("\"studentId\"", responseBody);
        Assert.DoesNotContain("\"username\"", responseBody);
        Assert.DoesNotContain("\"email\"", responseBody);
        Assert.DoesNotContain("\"bio\"", responseBody);
        Assert.DoesNotContain("\"name\"", responseBody);
        Assert.DoesNotContain("\"surname\"", responseBody);
        Assert.DoesNotContain("\"university\"", responseBody);
        Assert.DoesNotContain("\"courseOfStudy\"", responseBody);
        Assert.DoesNotContain("\"gender\"", responseBody);
        Assert.DoesNotContain("\"birthDate\"", responseBody);
    }

    [Fact]
    public async Task TestGetStudentFromToken401() {
        // Arrange
        logIn.LogOut();

        // Act
        var response = await client.GetAsync("/api/profile/student");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(401, (int)response.StatusCode);
        Assert.DoesNotContain("\"studentId\"", responseBody);
        Assert.DoesNotContain("\"username\"", responseBody);
        Assert.DoesNotContain("\"email\"", responseBody);
        Assert.DoesNotContain("\"bio\"", responseBody);
        Assert.DoesNotContain("\"name\"", responseBody);
        Assert.DoesNotContain("\"surname\"", responseBody);
        Assert.DoesNotContain("\"university\"", responseBody);
        Assert.DoesNotContain("\"courseOfStudy\"", responseBody);
        Assert.DoesNotContain("\"gender\"", responseBody);
        Assert.DoesNotContain("\"birthDate\"", responseBody);
    }

    [Fact]
    public async Task TestUpdateProfileCompany_400() {
        // Arrange
        logIn.LogInStudent();
        var updateForm = new {
            Username = "UpdateCompany",
            Email = "UpdatePassword",
            Password = "Update@Company.mail",
            Bio = "Crazy Bio!",
            Headquarter = "Crazy HQ!",
            FiscalCode = "Crazy FC!",
            VatNumber = "Crazy VN!"
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/profile/company", updateForm);

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestUpdateProfileCompany_401() {
        // Arrange
        logIn.LogOut();
        var updateForm = new {
            Username = "UpdateCompany",
            Email = "UpdatePassword",
            Password = "Update@Company.mail",
            Bio = "Crazy Bio!",
            Headquarter = "Crazy HQ!",
            FiscalCode = "Crazy FC!",
            VatNumber = "Crazy VN!"
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/profile/company", updateForm);

        // Assert
        Assert.Equal(401, (int)response.StatusCode);
    }

    [Theory]
    // Valid form
    [InlineData("UpdateCompany1", "UpdatePassword", "Update1@Company.mail", "Crazy Bio!", "Crazy HQ!",
            "Crazy FC!", "Crazy VN!", 200, "SeedCompany1")]
    // Empty username, password, email
    [InlineData("", "UpdatePassword", "Update@Company.mail", "Crazy Bio!", "Crazy HQ!",
            "Crazy FC!", "Crazy VN!", 400, "SeedCompany2")]
    [InlineData("UpdateCompany", "", "Update@Company.mail", "Crazy Bio!", "Crazy HQ!",
            "Crazy FC!", "Crazy VN!", 400, "SeedCompany2")]
    [InlineData("UpdateCompany", "UpdatePassword", "", "Crazy Bio!", "Crazy HQ!",
            "Crazy FC!", "Crazy VN!", 400, "SeedCompany2")]
    // Update bio, headquarter, fiscal code, VAT number
    [InlineData(null, null, null, "N3w!", "", "", "", 200, "SeedCompany3")]
    [InlineData(null, null, null, "", "N3w!", "", "", 200, "SeedCompany4")]
    [InlineData(null, null, null, "", "", "N3w!", "", 200, "SeedCompany5")]
    [InlineData(null, null, null, "", "", "", "N3w!", 200, "SeedCompany6")]
    public async Task TestUpdateProfileCompany(
        string username,
        string password,
        string email,
        string bio,
        string headquarter,
        string fiscalCode,
        string vatNumber,
        int statusCode,
        string oldName
    ) {
        // Arrange
        logIn.LogIn(oldName);
        var updateForm = new {
            Username = username,
            Email = email,
            Password = password,
            Bio = bio,
            Headquarter = headquarter,
            FiscalCode = fiscalCode,
            VatNumber = vatNumber
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/profile/company", updateForm);

        // Assert
        Assert.Equal(statusCode, (int)response.StatusCode);
    }
}

