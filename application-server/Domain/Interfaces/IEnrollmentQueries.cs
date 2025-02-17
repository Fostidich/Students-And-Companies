using System.Collections.Generic;
using System;

public interface IEnrollmentQueries
{

    Entity.Application GetApplication(int userId, int advertisementId);
    Entity.Application GetApplication(int applicationId);
    bool CreateApplication(int userId, int advertisementId, string questionnaireAnswer);
    List<Entity.Application> GetPendingApplications(int id);
    bool AcceptApplication(int id);
    bool RejectApplication(int id);
    bool CreateInternship(int studentId, int companyId, int advertisementId, DateTime start, DateTime end);
    bool NotifyStudent(int studentId, int advertisementId, bool accepted);
    bool RejectAllApplications(int id);
    bool UpdateAdvertisementSpots(int id);

}
