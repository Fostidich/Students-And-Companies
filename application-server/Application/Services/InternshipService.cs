using System.Collections.Generic;
using System.Linq;

public class InternshipService : IInternshipService {

    private readonly IInternshipQueries queries;

    public InternshipService(IInternshipQueries queries) {
        this.queries = queries;
    }
    
    public Internship GetInternshipForStudent(int studentId) {
        // Get internships for a student
        Entity.Internship internships = queries.GetInternshipForStudent(studentId);
        
        // If exceptions occur, return null
        if(internships == null) {
            return null;
        }
        
        // Convert Entity.Internship to Internship
        Internship intern = new Internship(internships);
        
        return intern;
    }
    
    public List<Internship> GetInternshipFromAdvertisement(int advertisementId) {
        // Get internships from an advertisement
        List<Entity.Internship> internships = queries.GetInternshipFromAdvertisement(advertisementId);
        
        // If exceptions occur, return null
        if(internships == null) {
            return null;
        }
        
        // Convert Entity.Internship to Internship
        List<Internship> intern = internships
            .Select(intern => new Internship(intern))
            .ToList();
        
        return intern;
    }
    
    public bool CreateStudentFeedback(int internshipId, DTO.Feedback feedback) {
        Feedback feed = new Feedback(feedback);
        
        // Convert DTO.FeedbackRegistration to Entity.Feedback
        Entity.StudentFeedback feedEntity = feed.ToStudentFeedbackEntity();
        
        // Create feedback
        bool result = queries.CreateStudentFeedback(internshipId, feedEntity);
        
        return result;
    }
    
    public bool CreateCompanyFeedback(int internshipId, DTO.Feedback feedback) {
        Feedback feed = new Feedback(feedback);
        
        // Convert DTO.FeedbackRegistration to Entity.Feedback
        Entity.CompanyFeedback feedEntity = feed.ToCompanyFeedbackEntity();
        
        // Create feedback
        bool result = queries.CreateCompanyFeedback(internshipId, feedEntity);
        
        return result;
    }
    
    public DTO.Feedback GetStudentFeedback(int internshipId) {
        // Get student feedback
        Entity.StudentFeedback feedback = queries.GetStudentFeedback(internshipId);
        
        // null propagation
        if(feedback == null) {
            return null;
        }
        
        // Convert Entity.StudentFeedback to DTO.Feedback
        Feedback feed = new Feedback(feedback);
        DTO.Feedback feedbackDto = feed.ToDto();
        
        return feedbackDto;
    }
    
    public DTO.Feedback GetCompanyFeedback(int internshipId) {
        // Get company feedback
        Entity.CompanyFeedback feedback = queries.GetCompanyFeedback(internshipId);
        
        // null propagation
        if(feedback == null) {
            return null;
        }
        
        // Convert Entity.CompanyFeedback to DTO.Feedback
        Feedback feed = new Feedback(feedback);
        DTO.Feedback feedbackDto = feed.ToDto();
        
        return feedbackDto;
    }

}
