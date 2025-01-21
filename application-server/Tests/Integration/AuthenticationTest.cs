using Xunit;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class AuthenticationTest : IClassFixture<TestServerFixture> {

    private readonly HttpClient client;

    public AuthenticationTest(TestServerFixture fixture) {
        client = fixture.Client;
    }

    [Theory]
    // Already existing users
    [InlineData("SeedStudent", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "Rome (RM), Lazio, Italy", "STDFNCbadab00m", "not a number but ok", 400)]
    [InlineData("SeedCompany", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "Rome (RM), Lazio, Italy", "STDFNCbadab00m", "not a number but ok", 400)]
    [InlineData("registrationTest", "passwordTest", "Seed@Student.mail", "Hi I'm the best!",
            "Rome (RM), Lazio, Italy", "STDFNCbadab00m", "not a number but ok", 400)]
    [InlineData("registrationTest", "passwordTest", "Seed@Company.mail", "Hi I'm the best!",
            "Rome (RM), Lazio, Italy", "STDFNCbadab00m", "not a number but ok", 400)]
    // Username tests
    [InlineData("registrationTest1C", "passwordTest", "registration1C@test.com", "Hi I'm the best!",
            "Rome (RM), Lazio, Italy", "STDFNCbadab00m", "not a number but ok", 200)]
    [InlineData("no spaces", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "Rome (RM), Lazio, Italy", "STDFNCbadab00m", "not a number but ok", 400)]
    [InlineData("no@email.com", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "Rome (RM), Lazio, Italy", "STDFNCbadab00m", "not a number but ok", 400)]
    [InlineData("", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "Rome (RM), Lazio, Italy", "STDFNCbadab00m", "not a number but ok", 400)]
    // Password tests
    [InlineData("registrationTest", "tooshor", "registration@test.com", "Hi I'm the best!",
            "Rome (RM), Lazio, Italy", "STDFNCbadab00m", "not a number but ok", 400)]
    [InlineData("registrationTest", "", "registration@test.com", "Hi I'm the best!",
            "Rome (RM), Lazio, Italy", "STDFNCbadab00m", "not a number but ok", 400)]
    // Email tests
    [InlineData("registrationTest", "passwordTest", "notAMail.com", "Hi I'm the best!",
            "Rome (RM), Lazio, Italy", "STDFNCbadab00m", "not a number but ok", 400)]
    [InlineData("registrationTest", "passwordTest", "", "Hi I'm the best!",
            "Rome (RM), Lazio, Italy", "STDFNCbadab00m", "not a number but ok", 400)]
    // Bio tests
    [InlineData("registrationTest2C", "passwordTest", "registration2C@test.com", "",
            "Rome (RM), Lazio, Italy", "STDFNCbadab00m", "not a number but ok", 200)]
    // Headquarter tests
    [InlineData("registrationTest", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "", "STDFNCbadab00m", "not a number but ok", 400)]
    // Fiscal code tests
    [InlineData("registrationTest", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "Rome (RM), Lazio, Italy", "", "not a number but ok", 400)]
    // VAT number tests
    [InlineData("registrationTest", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "Rome (RM), Lazio, Italy", "STDFNCbadab00m", "", 400)]
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
        var registrationForm = new {
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
    // Already existing users
    [InlineData("SeedStudent", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "CoolName", "CoolSurname", "Polimi", "CS", "f", "2001-12-02T00:00:00.000Z", 400)]
    [InlineData("SeedCompany", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "CoolName", "CoolSurname", "Polimi", "CS", "f", "2001-12-02T00:00:00.000Z", 400)]
    [InlineData("registrationTest", "passwordTest", "Seed@Student.mail", "Hi I'm the best!",
            "CoolName", "CoolSurname", "Polimi", "CS", "f", "2001-12-02T00:00:00.000Z", 400)]
    [InlineData("registrationTest", "passwordTest", "Seed@Company.mail", "Hi I'm the best!",
            "CoolName", "CoolSurname", "Polimi", "CS", "f", "2001-12-02T00:00:00.000Z", 400)]
    // Username tests
    [InlineData("registrationTest1S", "passwordTest", "registration1S@test.com", "Hi I'm the best!",
            "CoolName", "CoolSurname", "Polimi", "CS", "f", "2001-12-02T00:00:00.000Z", 200)]
    [InlineData("no spaces", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "CoolName", "CoolSurname", "Polimi", "CS", "f", "2001-12-02T00:00:00.000Z", 400)]
    [InlineData("no@email.com", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "CoolName", "CoolSurname", "Polimi", "CS", "f", "2001-12-02T00:00:00.000Z", 400)]
    [InlineData("", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "CoolName", "CoolSurname", "Polimi", "CS", "f", "2001-12-02T00:00:00.000Z", 400)]
    // Password tests
    [InlineData("registrationTest", "tooshor", "registration@test.com", "Hi I'm the best!",
            "CoolName", "CoolSurname", "Polimi", "CS", "f", "2001-12-02T00:00:00.000Z", 400)]
    [InlineData("registrationTest", "", "registration@test.com", "Hi I'm the best!",
            "CoolName", "CoolSurname", "Polimi", "CS", "f", "2001-12-02T00:00:00.000Z", 400)]
    // Email tests
    [InlineData("registrationTest", "passwordTest", "notAMail.com", "Hi I'm the best!",
            "CoolName", "CoolSurname", "Polimi", "CS", "f", "2001-12-02T00:00:00.000Z", 400)]
    [InlineData("registrationTest", "passwordTest", "", "Hi I'm the best!",
            "CoolName", "CoolSurname", "Polimi", "CS", "f", "2001-12-02T00:00:00.000Z", 400)]
    // Bio tests
    [InlineData("registrationTest2S", "passwordTest", "registration2S@test.com", "",
            "CoolName", "CoolSurname", "Polimi", "CS", "f", "2001-12-02T00:00:00.000Z", 200)]
    // Name tests
    [InlineData("registrationTest", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "", "CoolSurname", "Polimi", "CS", "f", "2001-12-02T00:00:00.000Z", 400)]
    // Surname tests
    [InlineData("registrationTest", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "CoolName", "", "Polimi", "CS", "f", "2001-12-02T00:00:00.000Z", 400)]
    // University tests
    [InlineData("registrationTest", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "CoolName", "CoolSurname", "", "CS", "f", "2001-12-02T00:00:00.000Z", 400)]
    // Course of study tests
    [InlineData("registrationTest", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "CoolName", "CoolSurname", "Polimi", "", "f", "2001-12-02T00:00:00.000Z", 400)]
    // Gender tests
    [InlineData("registrationTest", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "CoolName", "CoolSurname", "Polimi", "CS", "", "2001-12-02T00:00:00.000Z", 400)]
    [InlineData("registrationTest", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "CoolName", "CoolSurname", "Polimi", "CS", "W", "2001-12-02T00:00:00.000Z", 400)]
    // BirtDate tests
    [InlineData("registrationTest", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "CoolName", "CoolSurname", "Polimi", "CS", "f", "", 400)]
    [InlineData("registrationTest", "passwordTest", "registration@test.com", "Hi I'm the best!",
            "CoolName", "CoolSurname", "Polimi", "CS", "f", "2020-01-02T00:00:00.000Z", 400)]
    [InlineData("registrationTest3S", "passwordTest", "registration3S@test.com", "Hi I'm the best!",
            "CoolName", "CoolSurname", "Polimi", "CS", "f", "2019-12-31T23:59:59.999Z", 200)]
    public async Task TestRegisterStudent(
            string username,
            string password,
            string email,
            string bio,
            string name,
            string surname,
            string university,
            string courseOfStudy,
            string gender,
            string birthDate,
            int statusCode
    ) {
        // Arrange
        var registrationForm = new {
            Username = username,
            Email = email,
            Password = password,
            Bio = bio,
            Name = name,
            Surname = surname,
            University = university,
            CourseOfStudy = courseOfStudy,
            Gender = gender,
			BirthDate = birthDate,
       };

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/register/student", registrationForm);

        // Assert
        Assert.Equal(statusCode, (int)response.StatusCode);
    }

    [Theory]
    [InlineData("SeedStudent", "SeedPassword", 200)]
    [InlineData("Seed@Student.mail", "SeedPassword", 200)]
    [InlineData("SeedCompany", "SeedPassword", 200)]
    [InlineData("Seed@Company.mail", "SeedPassword", 200)]
    [InlineData("loginTest", "noUserRight?", 401)]
    [InlineData("login@test.com", "noUserRight?", 401)]
    [InlineData("nopass", "", 400)]
    [InlineData("", "nouser", 400)]
    [InlineData("nopass", "   ", 400)]
    [InlineData("   ", "nouser", 400)]
    public async Task TestLogin(
            string username,
            string password,
            int statusCode
    ) {
        // Arrange
        var credentials = new {
            Username = username,
            Password = password,
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/login", credentials);
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(statusCode, (int)response.StatusCode);
        if (statusCode == 200) {
            Assert.Contains("\"token\"", responseBody);
            Assert.Contains("\"userType\"", responseBody);
        } else {
            Assert.DoesNotContain("\"token\"", responseBody);
            Assert.DoesNotContain("\"userType\"", responseBody);
        }
    }

    [Theory]
    [InlineData("SeedCompany", "SeedPassword", "Company")]
    [InlineData("SeedStudent", "SeedPassword", "Student")]
    public async Task TestCheckToken(
            string username,
            string password,
            string userType
    ) {
        // Arrange
        var credentials = new {
            Username = username,
            Password = password,
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/login", credentials);
        var responseBody = await response.Content.ReadAsStringAsync();

        var jsonObject = JObject.Parse(responseBody);
        string token = jsonObject["token"].ToString();
        string userTypeRes = jsonObject["userType"].ToString();

        // Assert
        Assert.Equal(userType, userTypeRes);

        // Arrange
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Act
        response = await client.GetAsync("/api/authentication");

        // Assert
        Assert.Equal(200, (int)response.StatusCode);

        // Arrange
        client.DefaultRequestHeaders.Authorization = null;

        // Act
        response = await client.GetAsync("/api/authentication");

        // Assert
        Assert.Equal(401, (int)response.StatusCode);
    }

}
