using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;

[Collection("Test collection")]
public class ProfileTest {

    private readonly HttpClient client;
    private readonly TestFixture.LogInHelper logIn;
    private readonly TestSeed.SeedHelper seed;

    public ProfileTest(TestFixture fixture) {
        client = fixture.Client;
        logIn = fixture.LogIn;
        seed = fixture.Seed;
    }

    [Fact]
    public async Task TestGetCompanyFromToken200() {
        // Arrange
        logIn.LogInNewCompany();

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
        logIn.LogInNewStudent();

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
    public async Task TestGetCompanyFromToken404() {
        // Arrange
        logIn.LogInNewCompany();

        // Act
        await client.PostAsync("/api/profile/delete", null);
        var response = await client.GetAsync("/api/profile/company");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(404, (int)response.StatusCode);
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
        logIn.LogInNewStudent();

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
        logIn.LogInNewCompany();

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
    public async Task TestGetStudentFromToken404() {
        // Arrange
        logIn.LogInNewStudent();

        // Act
        var response = await client.PostAsync("/api/profile/delete", null);
        Assert.Equal(200, (int)response.StatusCode);
        response = await client.GetAsync("/api/profile/student");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(404, (int)response.StatusCode);
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
    public async Task TestUpdateProfileCompany400() {
        // Arrange
        logIn.LogInNewStudent();
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
    public async Task TestUpdateProfileCompany401() {
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
            "Crazy FC!", "Crazy VN!", 200)]
    // Empty username, password, email
    [InlineData("", "UpdatePassword", "Update@Company.mail", "Crazy Bio!", "Crazy HQ!",
            "Crazy FC!", "Crazy VN!", 400)]
    [InlineData("UpdateCompany", "", "Update@Company.mail", "Crazy Bio!", "Crazy HQ!",
            "Crazy FC!", "Crazy VN!", 400)]
    [InlineData("UpdateCompany", "UpdatePassword", "", "Crazy Bio!", "Crazy HQ!",
            "Crazy FC!", "Crazy VN!", 400)]
    // Update bio, headquarter, fiscal code, VAT number
    [InlineData(null, null, null, "N3w!", "", "", "", 200)]
    [InlineData(null, null, null, "", "N3w!", "", "", 200)]
    [InlineData(null, null, null, "", "", "N3w!", "", 200)]
    [InlineData(null, null, null, "", "", "", "N3w!", 200)]
    public async Task TestUpdateProfileCompany(
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
        logIn.LogInNewCompany();
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

    [Fact]
    public async Task TestUpdateProfileStudent400() {
        // Arrange
        logIn.LogInNewCompany();
        var updateForm = new {
            Username = "UpdateStudent",
            Password = "UpdatePassword",
            Email = "Update@Student.mail",
            Bio = "Crazy Bio!",
            Name = "Crazy Name!",
            Surname = "Crazy SN!",
            University = "Crazy Uni!",
            CourseOfStudy = "Crazy CoS!",
            Gender = "m",
            BirthDate = "2000-01-01T00:00:00.000Z",
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/profile/student", updateForm);

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestUpdateProfileStudent401() {
        // Arrange
        logIn.LogOut();
        var updateForm = new {
            Username = "UpdateStudent",
            Password = "UpdatePassword",
            Email = "Update@Student.mail",
            Bio = "Crazy Bio!",
            Name = "Crazy Name!",
            Surname = "Crazy SN!",
            University = "Crazy Uni!",
            CourseOfStudy = "Crazy CoS!",
            Gender = "m",
            BirthDate = "2000-01-01T00:00:00.000Z",
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/profile/student", updateForm);

        // Assert
        Assert.Equal(401, (int)response.StatusCode);
    }

    [Theory]
    // Valid form
    [InlineData("UpdateStudent1", "UpdatePassword", "Update1@Student.mail", "Crazy Bio!", "Crazy Name!",
            "Crazy SN!", "Crazy Uni!", "Crazy CoS!", "m", "2000-01-01T00:00:00.000Z", 200)]
    // Empty username, password, email
    [InlineData("", "UpdatePassword", "Update@Student.mail", "Crazy Bio!", "Crazy Name!", "Crazy SN!",
            "Crazy Uni!", "Crazy CoS!", "f", "2000-01-01T00:00:00.000Z", 400)]
    [InlineData("UpdateStudent", "", "Update@Student.mail", "Crazy Bio!", "Crazy Name!", "Crazy SN!",
            "Crazy Uni!", "Crazy CoS!", "f", "2000-01-01T00:00:00.000Z", 400)]
    [InlineData("UpdateStudent", "UpdatePassword", "", "Crazy Bio!", "Crazy Name!", "Crazy SN!",
            "Crazy Uni!", "Crazy CoS!", "f", "2000-01-01T00:00:00.000Z", 400)]
    // Update bio, name, surname, university, course of study, gender, birth date
    [InlineData(null, null, null, "N3w!", "", "", "", "", null, null, 200)]
    [InlineData(null, null, null, "", "N3w!", "", "", "", null, null, 200)]
    [InlineData(null, null, null, "", "", "N3w!", "", "", null, null, 200)]
    [InlineData(null, null, null, "", "", "", "N3w!", "", null, null, 200)]
    [InlineData(null, null, null, "", "", "", "", "N3w!", null, null, 200)]
    [InlineData(null, null, null, "", "", "", "", "", "f", null, 200)]
    [InlineData(null, null, null, "", "", "", "", "", null, "2000-01-01T00:00:00.000Z", 200)]
    public async Task TestUpdateProfileStudent(
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
        logIn.LogInNewStudent();
        var updateForm = new {
            Username = username,
            Password = password,
            Email = email,
            Bio = bio,
            Name = name,
            Surname = surname,
            University = university,
            CourseOfStudy = courseOfStudy,
            Gender = gender,
            BirthDate = birthDate,
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/profile/student", updateForm);

        // Assert
        Assert.Equal(statusCode, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestGetCompanyFromId200() {
        // Arrange
        int id = seed.GetNewCompanyId();
        logIn.LogInNewStudent();

        // Act
        var response = await client.GetAsync($"/api/profile/company/{id}");
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
    public async Task TestGetCompanyFromId400() {
        // Arrange
        logIn.LogInNewCompany();

        // Act
        var response = await client.GetAsync("/api/profile/company/0");
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
    public async Task TestGetCompanyFromId401() {
        // Arrange
        logIn.LogOut();

        // Act
        var response = await client.GetAsync("/api/profile/company/1");
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
    public async Task TestGetCompanyFromId404() {
        // Arrange
        int id = seed.GetNewCompanyId();
        logIn.LogInCompany(id);

        // Act
        await client.PostAsync("/api/profile/delete", null);
        var response = await client.GetAsync($"/api/profile/company/{id}");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(404, (int)response.StatusCode);
        Assert.DoesNotContain("\"companyId\"", responseBody);
        Assert.DoesNotContain("\"username\"", responseBody);
        Assert.DoesNotContain("\"email\"", responseBody);
        Assert.DoesNotContain("\"bio\"", responseBody);
        Assert.DoesNotContain("\"headquarter\"", responseBody);
        Assert.DoesNotContain("\"fiscalCode\"", responseBody);
        Assert.DoesNotContain("\"vatNumber\"", responseBody);
    }

    [Fact]
    public async Task TestGetStudentFromId200() {
        // Arrange
        int id = seed.GetNewStudentId();
        logIn.LogInNewCompany();

        // Act
        var response = await client.GetAsync($"/api/profile/student/{id}");
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
    public async Task TestGetStudentFromId400() {
        // Arrange
        logIn.LogInNewStudent();

        // Act
        var response = await client.GetAsync("/api/profile/student/0");
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
    public async Task TestGetStudentFromId401() {
        // Arrange
        logIn.LogOut();

        // Act
        var response = await client.GetAsync("/api/profile/student/1");
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
    public async Task TestGetStudentFromId404() {
        // Arrange
        int id = seed.GetNewStudentId();
        logIn.LogInStudent(id);

        // Act
        var response = await client.PostAsync("/api/profile/delete", null);

        // Assert
        Assert.Equal(200, (int)response.StatusCode);

        // Act
        response = await client.GetAsync($"/api/profile/student/{id}");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(404, (int)response.StatusCode);
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
    public async Task TestDeleteUser200() {
        // Arrange
        logIn.LogInNewCompany();

        // Act
        var response = await client.PostAsync("/api/profile/delete", null);

        // Assert
        Assert.Equal(200, (int)response.StatusCode);

        // Act
        response = await client.GetAsync("/api/profile/company");

        // Assert
        Assert.Equal(404, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestDeleteUser401() {
        // Arrange
        logIn.LogOut();

        // Act
        var response = await client.PostAsync("/api/profile/delete", null);

        // Assert
        Assert.Equal(401, (int)response.StatusCode);

    }

    [Fact]
    public async Task TestDeleteUser404() {
        // Arrange
        logIn.LogInNewCompany();

        // Act
        await client.PostAsync("/api/profile/delete", null);
        var response = await client.PostAsync("/api/profile/delete", null);

        // Assert
        Assert.Equal(404, (int)response.StatusCode);

    }

    [Theory]
    [InlineData("Test Skill1!!")]
    [InlineData("Test Skill2!!")]
    [InlineData("Test Skill3!!")]
    public async Task TestAddSkill200(string name) {
        // Arrange
        logIn.LogInNewStudent();
        var skill = new { Name = name };

        // Act
        var response = await client.PostAsJsonAsync("/api/profile/skills", skill);

        // Assert
        Assert.Equal(200, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestAddSkill400() {
        // Arrange
        logIn.LogInNewCompany();
        var skill = new { Name = "Bad Skill!!" };

        // Act
        var response = await client.PostAsJsonAsync("/api/profile/skills", skill);

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestAddSkill401() {
        // Arrange
        logIn.LogOut();
        var skill = new { Name = "Bad Skill!!" };

        // Act
        var response = await client.PostAsJsonAsync("/api/profile/skills", skill);

        // Assert
        Assert.Equal(401, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestGetSkillFromToken200() {
        // Arrange
        logIn.LogInNewStudent();

        // Act
        var response = await client.GetAsync("/api/profile/skills");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Contains("\"id\"", responseBody);
        Assert.Contains("\"name\"", responseBody);
    }

    [Fact]
    public async Task TestGetSkillFromToken400() {
        // Arrange
        logIn.LogInNewCompany();

        // Act
        var response = await client.GetAsync("/api/profile/skills");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.DoesNotContain("\"id\"", responseBody);
        Assert.DoesNotContain("\"name\"", responseBody);
    }

    [Fact]
    public async Task TestGetSkillFromToken401() {
        // Arrange
        logIn.LogOut();

        // Act
        var response = await client.GetAsync("/api/profile/skills");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(401, (int)response.StatusCode);
        Assert.DoesNotContain("\"id\"", responseBody);
        Assert.DoesNotContain("\"name\"", responseBody);
    }

    [Fact]
    public async Task TestGetSkillFromToken404() {
        // Arrange
        logIn.LogInNewStudent();

        // Act
        await client.GetAsync("/api/profile/skills/delete/13");
        var response = await client.GetAsync("/api/profile/skills");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(404, (int)response.StatusCode);
        Assert.DoesNotContain("\"id\"", responseBody);
        Assert.DoesNotContain("\"name\"", responseBody);
    }

    [Fact]
    public async Task TestGetSkillFromId200() {
        // Arrange
        int id = seed.GetNewStudentId();
        logIn.LogInNewCompany();

        // Act
        var response = await client.GetAsync($"/api/profile/skills/{id}");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Contains("\"id\"", responseBody);
        Assert.Contains("\"name\"", responseBody);
    }

    [Fact]
    public async Task TestGetSkillFromId400() {
        // Arrange
        logIn.LogInNewStudent();

        // Act
        var response = await client.GetAsync("/api/profile/skills/0");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
        Assert.DoesNotContain("\"id\"", responseBody);
        Assert.DoesNotContain("\"name\"", responseBody);
    }

    [Fact]
    public async Task TestGetSkillFromId401() {
        // Arrange
        logIn.LogOut();

        // Act
        var response = await client.GetAsync("/api/profile/skills/1");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(401, (int)response.StatusCode);
        Assert.DoesNotContain("\"id\"", responseBody);
        Assert.DoesNotContain("\"name\"", responseBody);
    }

    [Fact]
    public async Task TestGetSkillFromId404() {
        // Arrange
        int id = seed.GetNewStudentId();
        logIn.LogInStudent(id);

        // Act
        await client.PostAsync("/api/profile/delete", null);
        var response = await client.GetAsync($"/api/profile/skills/{id}");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(404, (int)response.StatusCode);
        Assert.DoesNotContain("\"id\"", responseBody);
        Assert.DoesNotContain("\"name\"", responseBody);
    }

    [Fact]
    public async Task TestDeleteSkill200() {
        // Arrange
        int id = seed.GetNewStudentId();
        logIn.LogInStudent(id);

        // Act
        var response = await client.PostAsync($"/api/profile/skills/delete/{id}", null);

        // Assert
        Assert.Equal(200, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestDeleteSkill400() {
        // Arrange
        logIn.LogInNewStudent();

        // Act
        var response = await client.PostAsync("/api/profile/skills/delete/0", null);

        // Assert
        Assert.Equal(400, (int)response.StatusCode);

        // Arrange
        logIn.LogInNewCompany();

        // Act
        response = await client.PostAsync("/api/profile/skills/delete/0", null);

        // Assert
        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestDeleteSkill401() {
        // Arrange
        logIn.LogOut();

        // Act
        var response = await client.PostAsync("/api/profile/skills/delete/1", null);

        // Assert
        Assert.Equal(401, (int)response.StatusCode);
    }

    [Fact]
    public async Task TestDeleteSkill404() {
        // Arrange
        logIn.LogInNewStudent();

        // Act
        var response = await client.PostAsync("/api/profile/skills/delete/1", null);

        // Assert
        Assert.Equal(404, (int)response.StatusCode);
    }

}

