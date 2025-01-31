using System.Collections.Generic;
using System;

public interface IEnrollmentService {

    Application GetApplication(int userId, int advertisementId);
    Application GetApplication(int applicationId);
    bool CreateApplication(int userId, int advertisementId, string questionnaireAnswer);
    Advertisement GetAdvertisement(int id);
    List<Application> GetPendingApplications(int id);
    bool CheckStartDateValidity(DateTime date);
    bool AcceptApplication(int id);
    bool RejectApplication(int id);
    bool CreateInternship(int studentId, int companyId, int advertisementId, DateTime start);
    bool NotifyStudent(int studentId, int advertisementId, bool accepted);
    Internship GetInternship(int id);
    bool RejectAllApplications(int id);
    bool UpdateAdvertisementSpots(int id);

}

