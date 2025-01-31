using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

public class EnrollmentQueries : IEnrollmentQueries {

    private readonly IDataService dataService;

    public EnrollmentQueries(IDataService dataService) {
        this.dataService = dataService;
    }

    public Entity.Application GetApplication(int userId, int advertisementId) {
        try {
            string query = @"
                SELECT *
                FROM application
                WHERE student_id = @StudentId
                AND advertisement_id = @AdvertisementId";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@StudentId", userId);
            command.Parameters.AddWithValue("@AdvertisementId", advertisementId);

            using var reader = command.ExecuteReader();

            var applications = dataService.MapToApplications(reader);
            return applications.FirstOrDefault();
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public Entity.Application GetApplication(int applicationId) {
        try {
            string query = @"
                SELECT *
                FROM application
                WHERE application_id = @ApplicationId";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@ApplicationId", applicationId);

            using var reader = command.ExecuteReader();

            var applications = dataService.MapToApplications(reader);
            return applications.FirstOrDefault();
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public bool CreateApplication(int userId, int advertisementId, string questionnaireAnswer) {
        try {
            string query = $@"
                INSERT INTO application (student_id, advertisement_id, status, questionnaire)
                VALUES (@StudentId, @AdvertisementId, 'PENDING', @Questionnaire)";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@StudentId", userId);
            command.Parameters.AddWithValue("@AdvertisementId", advertisementId);
            command.Parameters.AddWithValue("@Questionnaire", questionnaireAnswer);

	        int rowsAffected = command.ExecuteNonQuery();

	        return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public List<Entity.Application> GetPendingApplications(int id) {
        try {
            string query = @"
                SELECT *
                FROM application
                WHERE advertisement_id = @AdvertisementId
                AND status = 'PENDING'";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@AdvertisementId", id);

            using var reader = command.ExecuteReader();

            var applications = dataService.MapToApplications(reader);
            return applications;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public bool AcceptApplication(int id) {
        try {
            string query = $@"
                UPDATE application
                SET status = 'ACCEPTED'
                WHERE application_id = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@Id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool RejectApplication(int id) {
        try {
            string query = $@"
                UPDATE application
                SET status = 'REJECTED'
                WHERE application_id = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@Id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }


    public bool CreateInternship(int studentId, int companyId, int advertisementId, DateTime start, DateTime end) {
        try {
            string query = $@"
                INSERT INTO internship (student_id, company_id, advertisement_id, start_date, end_date)
                VALUES (@StudentId, @CompanyId, @AdvertisementId, @StartDate, @EndDate)";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@StudentId", studentId);
            command.Parameters.AddWithValue("@CompanyId", companyId);
            command.Parameters.AddWithValue("@AdvertisementId", advertisementId);
            command.Parameters.AddWithValue("@StartDate", start);
            command.Parameters.AddWithValue("@EndDate", end);

	        int rowsAffected = command.ExecuteNonQuery();

	        return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool NotifyStudent(int studentId, int advertisementId, bool accepted) {
        try {
            string outcome = accepted ? "ACCEPTED" : "REJECTED";
            string query = $@"
                INSERT INTO student_notifications (student_id, advertisement_id, type)
                VALUES (@StudentId, @AdvertisementId, '{outcome}')";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@StudentId", studentId);
            command.Parameters.AddWithValue("@AdvertisementId", advertisementId);

	        int rowsAffected = command.ExecuteNonQuery();

	        return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool RejectAllApplications(int id) {
        try {
            string query = $@"
                UPDATE application
                SET status = 'REJECTED'
                WHERE student_id = @Id
                AND status = 'PENDING'";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@Id", id);

            return true;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool UpdateAdvertisementSpots(int id) {
        try {
            string query = @"
                UPDATE advertisement
                SET
                    available = available - 1,
                    open = CASE
                        WHEN available = 0 THEN FALSE
                        ELSE open
                    END
                WHERE advertisement_id = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@Id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

}
