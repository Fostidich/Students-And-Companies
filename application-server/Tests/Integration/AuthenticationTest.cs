using Xunit;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

[Collection("Test collection")]
public class AuthenticationTest {

    private readonly HttpClient client;
    private readonly TestSeed.SeedHelper seed;

    public AuthenticationTest(TestFixture fixture) {
        client = fixture.Client;
        seed = fixture.Seed;
    }

    [Fact]
    public async Task TestRegisterCompany_CompanyUsername() {
        // Arrange
        var registrationForm = new {
            Username = seed.GetNewCompanyUsername(),
            Email = "registration@test.com",
            Password = "passwordTest",
            Bio = "Hi I'm the best!",
            Headquarter = "Rome (RM), Lazio, Italy",
            FiscalCode = "STDFNCbadab00m",
            VatNumber = "not a number but ok",
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/register/company", registrationForm);

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestRegisterCompany_StudentUsername() {
        // Arrange
        var registrationForm = new {
            Username = seed.GetNewStudentUsername(),
            Email = "registration@test.com",
            Password = "passwordTest",
            Bio = "Hi I'm the best!",
            Headquarter = "Rome (RM), Lazio, Italy",
            FiscalCode = "STDFNCbadab00m",
            VatNumber = "not a number but ok",
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/register/company", registrationForm);

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestRegisterCompany_CompanyEmail() {
        // Arrange
        var registrationForm = new {
            Username = "registrationTest",
            Email = seed.GetNewCompanyEmail(),
            Password = "passwordTest",
            Bio = "Hi I'm the best!",
            Headquarter = "Rome (RM), Lazio, Italy",
            FiscalCode = "STDFNCbadab00m",
            VatNumber = "not a number but ok",
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/register/company", registrationForm);

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestRegisterCompany_StudentEmail() {
        // Arrange
        var registrationForm = new {
            Username = "registrationTest",
            Email = seed.GetNewStudentEmail(),
            Password = "passwordTest",
            Bio = "Hi I'm the best!",
            Headquarter = "Rome (RM), Lazio, Italy",
            FiscalCode = "STDFNCbadab00m",
            VatNumber = "not a number but ok",
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/register/company", registrationForm);

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Theory]
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

    [Fact]
    public async Task TestRegisterStudent_CompanyUsername() {
        // Arrange
        var registrationForm = new {
            Username = seed.GetNewCompanyUsername(),
            Email = "registration@test.mail",
            Password = "passwordTest",
            Bio = "Hi I'm the best!",
            Name = "CoolName",
            Surname = "CoolSurname",
            University = "Polimi",
            CourseOfStudy = "CS",
            Gender = "f",
			BirthDate = "2001-12-02T00:00:00.000Z",
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/register/student", registrationForm);

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestRegisterStudent_StudentUsername() {
        // Arrange
        var registrationForm = new {
            Username = seed.GetNewStudentUsername(),
            Email = "registration@test.mail",
            Password = "passwordTest",
            Bio = "Hi I'm the best!",
            Name = "CoolName",
            Surname = "CoolSurname",
            University = "Polimi",
            CourseOfStudy = "CS",
            Gender = "f",
			BirthDate = "2001-12-02T00:00:00.000Z",
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/register/student", registrationForm);

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestRegisterStudent_CompanyEmail() {
        // Arrange
        var registrationForm = new {
            Username = "registrationTest",
            Email = seed.GetNewCompanyEmail(),
            Password = "passwordTest",
            Bio = "Hi I'm the best!",
            Name = "CoolName",
            Surname = "CoolSurname",
            University = "Polimi",
            CourseOfStudy = "CS",
            Gender = "f",
			BirthDate = "2001-12-02T00:00:00.000Z",
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/register/student", registrationForm);

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestRegisterStudent_StudentEmail() {
        // Arrange
        var registrationForm = new {
            Username = "registrationTest",
            Email = seed.GetNewStudentEmail(),
            Password = "passwordTest",
            Bio = "Hi I'm the best!",
            Name = "CoolName",
            Surname = "CoolSurname",
            University = "Polimi",
            CourseOfStudy = "CS",
            Gender = "f",
			BirthDate = "2001-12-02T00:00:00.000Z",
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/register/student", registrationForm);

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Theory]
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
        Assert.DoesNotContain("\"token\"", responseBody);
        Assert.DoesNotContain("\"userType\"", responseBody);
    }

    [Fact]
    public async Task TestCheckTokenCompany() {
        // Arrange
        var credentials = new {
            Username = seed.GetNewCompanyUsername(),
            Password = seed.Password,
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/login", credentials);
        var responseBody = await response.Content.ReadAsStringAsync();

        var jsonObject = JObject.Parse(responseBody);
        string token = jsonObject["token"].ToString();
        string userTypeRes = jsonObject["userType"].ToString();

        // Assert
        Assert.Equal(UserType.Company.ToString(), userTypeRes);

        // Arrange
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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

    [Fact]
    public async Task TestCheckTokenStudent() {
        // Arrange
        var credentials = new {
            Username = seed.GetNewStudentUsername(),
            Password = seed.Password,
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/login", credentials);
        var responseBody = await response.Content.ReadAsStringAsync();

        var jsonObject = JObject.Parse(responseBody);
        string token = jsonObject["token"].ToString();
        string userTypeRes = jsonObject["userType"].ToString();

        // Assert
        Assert.Equal(UserType.Student.ToString(), userTypeRes);

        // Arrange
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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
