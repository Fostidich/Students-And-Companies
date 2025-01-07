using Xunit;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using dotenv.net;

public class AuthenticationTest {

    private readonly HttpClient client;
    private string defaultConnection;

    public AuthenticationTest() {
        DotEnv.Load();
        defaultConnection = Environment.GetEnvironmentVariable("DB_DEFAULT_CONNECTION");
        client = new HttpClient { BaseAddress = new System.Uri("http://localhost:5000") };
    }

    [Theory]
    [InlineData("username", "password", "Student", "mail@mail.com", 200)]
    public async Task TestRegister(string username, string password, string userType, string email, int statusCode) {
        // Arrange
        var registrationForm = new DTO.RegistrationForm {
            Username = username,
            Password = password,
            UserType = userType,
            Email = email
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/register", registrationForm);

        // Assert
        Assert.Equal(statusCode, (int)response.StatusCode);

        // Reset
        if ((int)response.StatusCode != 200) DeleteUser(username);
    }

    private void DeleteUser(string username) {
        string query = @"
            DELETE FROM users
            WHERE username = @username";
        var connection = new MySqlConnection(defaultConnection);
        connection.Open();
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@username", username);
        command.ExecuteNonQuery();
    }

}
