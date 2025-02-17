using System;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

public class DataService : IDataService
{

    private string defaultConnection;

    public DataService(IConfiguration configuration)
    {
        defaultConnection = configuration["DbDefaultConnection"];
    }

    public MySqlConnection GetConnection()
    {
        var connection = new MySqlConnection(defaultConnection);
        connection.Open();
        return connection;
    }

    public List<Entity.Student> MapToStudents(IDataReader reader)
    {
        var students = new List<Entity.Student>();

        while (reader.Read())
        {
            students.Add(new Entity.Student(reader));
        }

        return students;
    }

    public List<Entity.Company> MapToCompanies(IDataReader reader)
    {
        var companies = new List<Entity.Company>();

        while (reader.Read())
        {
            companies.Add(new Entity.Company(reader));
        }

        return companies;
    }

    public List<Entity.Skill> MapToSkills(IDataReader reader)
    {
        var skills = new List<Entity.Skill>();

        while (reader.Read())
        {
            skills.Add(new Entity.Skill(reader));
        }

        return skills;
    }

    public List<Entity.Advertisement> MapToAdvertisements(IDataReader reader)
    {
        var advertisements = new List<Entity.Advertisement>();

        while (reader.Read())
        {
            advertisements.Add(new Entity.Advertisement(reader));
        }

        return advertisements;
    }

    public List<Entity.StudentNotifications> MapToStudentNotifications(IDataReader reader)
    {
        var studentNotifications = new List<Entity.StudentNotifications>();

        while (reader.Read())
        {
            studentNotifications.Add(new Entity.StudentNotifications(reader));
        }

        return studentNotifications;
    }

    public List<Entity.Application> MapToApplications(IDataReader reader)
    {
        var applications = new List<Entity.Application>();

        while (reader.Read())
        {
            applications.Add(new Entity.Application(reader));
        }

        return applications;
    }

    public List<Entity.Internship> MapToInternships(IDataReader reader)
    {
        var internships = new List<Entity.Internship>();

        while (reader.Read())
        {
            internships.Add(new Entity.Internship(reader));
        }

        return internships;
    }

    public List<Entity.StudentFeedback> MapToStudentFeedback(IDataReader reader)
    {
        var studentFeedback = new List<Entity.StudentFeedback>();

        while (reader.Read())
        {
            studentFeedback.Add(new Entity.StudentFeedback(reader));
        }

        return studentFeedback;
    }

    public List<Entity.CompanyFeedback> MapToCompanyFeedback(IDataReader reader)
    {
        var companyFeedback = new List<Entity.CompanyFeedback>();

        while (reader.Read())
        {
            companyFeedback.Add(new Entity.CompanyFeedback(reader));
        }

        return companyFeedback;
    }


    public List<string> MapToStrings(IDataReader reader, string fieldName)
    {
        var result = new List<string>();

        while (reader.Read())
        {
            result.Add(reader[fieldName].ToString());
        }

        return result;
    }

    public List<int> MapToInts(IDataReader reader, string fieldName)
    {
        var result = new List<int>();

        while (reader.Read())
        {
            result.Add(Convert.ToInt32(reader[fieldName].ToString()));
        }

        return result;
    }

}

