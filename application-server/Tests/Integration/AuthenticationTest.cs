using Xunit;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class AuthenticationTest : IClassFixture<TestServerFixture> {

    private readonly HttpClient client;

    public AuthenticationTest(TestServerFixture fixture) {
        client = fixture.Client;
    }

    [Theory]
    [InlineData("registrationTest", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "Rome (RM), Lazio, Italy", "STDFNCbadab00m", "not a number but ok", 200)]
    [InlineData("no spaces", "passwordTest", "Student", "registration@test.com", 400)]
    [InlineData("no@email.com", "passwordTest", "Student", "registration@test.com", 400)]
    [InlineData("too", "passwordTest", "Student", "registration@test.com", 400)]
    [InlineData("registrationTest", "tooshor", "Student", "registration@test.com", 400)]
    [InlineData("toolooooooooooooooooooooooooooong", "passwordTest", "Student", "registration@test.com", 400)]
    [InlineData("registrationTest", "toolooooooooooooooooooooooooooong", "Student", "registration@test.com", 400)]
    [InlineData("registrationTest", "passwordTest", "InVaLiDfIeLd", "registration@test.com", 400)]
    [InlineData("registrationTest", "passwordTest", "Company", "notAMail.com", 400)]
    [InlineData("", "passwordTest", "Company", "registration@test.com", 400)]
    [InlineData("registrationTest", "", "Company", "registration@test.com", 400)]
    [InlineData("registrationTest", "passwordTest", "", "registration@test.com", 400)]
    [InlineData("registrationTest", "passwordTest", "Company", "", 400)]
    [InlineData("   ", "passwordTest", "Company", "registration@test.com", 400)]
    [InlineData("registrationTest", "   ", "Company", "registration@test.com", 400)]
    [InlineData("registrationTest", "passwordTest", "   ", "registration@test.com", 400)]
    [InlineData("registrationTest", "passwordTest", "Company", "   ", 400)]
    public async Task TestRegisterCompany(
            string username,
            string password,
            string email,
            string bio,
            string headquarter,
            string fiscalCode,
            string vatNumber,
            int statusCode
    ) {
        // Arrange
        var registrationForm = new DTO.RegistrationFormCompany {
            Username = username,
            Email = email,
            Password = password,
            Bio = bio,
            Headquarter = headquarter,
            FiscalCode = fiscalCode,
            VatNumber = vatNumber,
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/register/company", registrationForm);

        // Assert
        Assert.Equal(statusCode, (int)response.StatusCode);
    }

    [Theory]
    [InlineData("loginTest", "noUser", 401)]
    [InlineData("nopass", "", 400)]
    [InlineData("", "nouser", 400)]
    [InlineData("nopass", "   ", 400)]
    [InlineData("   ", "nouser", 400)]
    public async Task TestLogin(string username, string password, int statusCode) {
        // Arrange
        var credentials = new DTO.Credentials {
            Username = username,
            Password = password,
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/login", credentials);
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(statusCode, (int)response.StatusCode);
        Assert.DoesNotContain("\"token\"", responseBody);
    }

    [Fact]
    public async Task TestLogin_200() {
        // Arrange
        var registrationForm = new DTO.RegistrationForm {
            Username = "loginTestNew",
            Password = "good password",
            UserType = "Company",
            Email = "new.user@login.test"
        };
        await client.PostAsJsonAsync("/api/authentication/register", registrationForm);

        var credentials = new DTO.Credentials {
            Username = "loginTestNew",
            Password = "good password",
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/login", credentials);
        var responseBody = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();

        // Assert
        Assert.Equal(200, (int)response.StatusCode);
        Assert.True(responseBody.ContainsKey("token"));
    }

}
