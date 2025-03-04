using System.Collections.Generic;
using System.Linq;

public class InternshipService : IInternshipService
{

    private readonly IInternshipQueries queries;

    public InternshipService(IInternshipQueries queries)
    {
        this.queries = queries;
    }

    public List<Internship> GetInternshipForStudent(int studentId)
    {
        // Get internships for a student
        List<Entity.Internship> internships = queries.GetInternshipForStudent(studentId);

        // If exceptions occur, return null
        if (internships == null)
        {
            return null;
        }

        // Convert Entity.Internship to Internship
        List<Internship> intern = internships
            .Select(intern => new Internship(intern))
            .ToList();

        return intern;
    }

    public List<Internship> GetInternshipFromAdvertisement(int advertisementId, int companyId)
    {
        // Get internships from an advertisement
        List<Entity.Internship> internships = queries.GetInternshipFromAdvertisement(advertisementId, companyId);

        // If exceptions occur, return null
        if (internships == null)
        {
            return null;
        }

        // Convert Entity.Internship to Internship
        List<Internship> intern = internships
            .Select(intern => new Internship(intern))
            .ToList();

        return intern;
    }

    public bool CreateStudentFeedback(int internshipId, DTO.Feedback feedback, int studentId)
    {
        Feedback feed = new Feedback(feedback);

        // Convert DTO.FeedbackRegistration to Entity.Feedback
        Entity.StudentFeedback feedEntity = feed.ToStudentFeedbackEntity();

        // Create feedback
        bool result = queries.CreateStudentFeedback(internshipId, feedEntity, studentId);

        return result;
    }

    public bool CreateCompanyFeedback(int internshipId, DTO.Feedback feedback, int companyId)
    {
        Feedback feed = new Feedback(feedback);

        // Convert DTO.FeedbackRegistration to Entity.Feedback
        Entity.CompanyFeedback feedEntity = feed.ToCompanyFeedbackEntity();

        // Create feedback
        bool result = queries.CreateCompanyFeedback(internshipId, feedEntity, companyId);

        return result;
    }

    public DTO.Feedback GetStudentFeedback(int internshipId, int userId, string role)
    {
        // Get student feedback
        Entity.StudentFeedback feedback = queries.GetStudentFeedback(internshipId, userId, role);

        // null propagation
        if (feedback == null)
        {
            return null;
        }

        // Convert Entity.StudentFeedback to DTO.Feedback
        Feedback feed = new Feedback(feedback);
        DTO.Feedback feedbackDto = feed.ToDto();

        return feedbackDto;
    }

    public DTO.Feedback GetCompanyFeedback(int internshipId, int userId, string role)
    {
        // Get company feedback
        Entity.CompanyFeedback feedback = queries.GetCompanyFeedback(internshipId, userId, role);

        // null propagation
        if (feedback == null)
        {
            return null;
        }

        // Convert Entity.CompanyFeedback to DTO.Feedback
        Feedback feed = new Feedback(feedback);
        DTO.Feedback feedbackDto = feed.ToDto();

        return feedbackDto;
    }

    public bool DeleteInternship(int internshipId, int userId, string role)
    {
        // Delete internship
        bool result = queries.DeleteInternship(internshipId, userId, role);

        return result;
    }

    public bool DeleteFeedback(int internshipId, int userId, string role)
    {

        if (role == UserType.Student.ToString())
        {
            return queries.DeleteStudentFeedback(internshipId, userId);
        }

        return queries.DeleteCompanyFeedback(internshipId, userId);
    }

}
