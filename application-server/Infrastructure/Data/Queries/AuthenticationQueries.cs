using System;
using MySql.Data.MySqlClient;

public static class AuthenticationQueries {

    public static bool RegisterUser(Entity.User user) {
        try {
            using var db_connection = DataService.GetConnection();

            string query =
                "INSERT INTO users (username, email, hashed_password, user_type)" +
                "VALUES (@username, @email, @hashed_password, @user_type)";
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

}
