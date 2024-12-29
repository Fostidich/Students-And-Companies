using System;
using MySql.Data.MySqlClient;

public static class AuthenticationQueries {

    public static bool RegisterUser(Entity.User user) {
        try {
            string query = @"
                INSERT INTO users (username, email, hashed_password, user_type)
                VALUES (@username, @email, @hashed_password, @user_type)";
            using var db_connection = DataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@hashed_password", user.HashedPassword);
            command.Parameters.AddWithValue("@user_type", user.UserType);
            command.ExecuteNonQuery();

            return true;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public static Entity.User GetUser(int id) {
        var user = new Entity.User();
        try {
            string query = @"
                SELECT *
                FROM users
                WHERE id = @id";
            using var db_connection = DataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();

            user = DataService.MapToUsers(reader)[0];
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }

        return user;
    }

}
