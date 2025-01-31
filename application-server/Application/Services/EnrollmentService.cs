using System.Collections.Generic;
using System.Linq;
using System;

public class EnrollmentService : IEnrollmentService {

    private readonly IEnrollmentQueries queries;
    private readonly IRecommendationQueries recommendationQueries;
    private readonly IInternshipService internshipService;

    public EnrollmentService(IEnrollmentQueries queries,
            IRecommendationQueries recommendationQueries, IInternshipService internshipService) {
        this.queries = queries;
        this.recommendationQueries = recommendationQueries;
        this.internshipService = internshipService;
    }

    public Application GetApplication(int userId, int advertisementId) {
        // Get application
        var found = queries.GetApplication(userId, advertisementId);

        // Return application if found
        if (found != null)
            return new Application(found);
        else
            return null;
    }

    public Application GetApplication(int applicationId) {
        // Get application
        var found = queries.GetApplication(applicationId);

        // Return application if found
        if (found != null)
            return new Application(found);
        else
            return null;
    }

    public bool CreateApplication(int userId, int advertisementId, string questionnaireAnswer) {
        return queries.CreateApplication(userId, advertisementId, questionnaireAnswer);
    }

    public Advertisement GetAdvertisement(int advertisementId) {
        // Get advertisement
        var found = recommendationQueries.GetAdvertisement(advertisementId);

        // Return advertisement if found
        if (found != null)
            return new Advertisement(found);
        else
            return null;
    }

    public List<Application> GetPendingApplications(int id) {
        // Get application
        var found = queries.GetPendingApplications(id);

        // Return applications found
        if (found != null)
            return found.Select(a => new Application(a)).ToList();
        else
            return null;
    }

    public bool CheckStartDateValidity(DateTime date) {
        var today = DateTime.Today;
        var maxDate = today.AddYears(5);
        return date > today && date < maxDate;
    }

    public bool AcceptApplication(int id) {
        return queries.AcceptApplication(id);
    }

    public bool RejectApplication(int id) {
        return queries.RejectApplication(id);
    }

    public bool CreateInternship(int studentId, int companyId, int advertisementId, DateTime start) {
        // Find advertisement
        var advertisement = recommendationQueries.GetAdvertisement(advertisementId);

        // Check error
        if (advertisement == null)
            return false;

        // Compute end date
        var end = start.AddMonths(advertisement.Duration);

        return queries.CreateInternship(studentId, companyId, advertisementId, start, end);
    }

    public bool UpdateAdvertisementSpots(int id) {
        return queries.UpdateAdvertisementSpots(id);
    }

    public bool NotifyStudent(int studentId, int advertisementId, bool accepted) {
        return queries.NotifyStudent(studentId, advertisementId, accepted);
    }

    public bool RejectAllApplications(int id) {
        return queries.RejectAllApplications(id);
    }

    public Internship GetInternship(int id) {
        return internshipService.GetInternshipForStudent(id);
    }

}
