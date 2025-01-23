using System.Collections.Generic;

public interface IInternshipService {
    Internship GetInternshipForStudent(int studentId);
    List<Internship> GetInternshipFromAdvertisement(int advertisementId);
    bool CreateStudentFeedback(int internshipId, DTO.Feedback feedback);
    bool CreateCompanyFeedback(int internshipId, DTO.Feedback feedback);
    DTO.Feedback GetStudentFeedback(int internshipId);
    DTO.Feedback GetCompanyFeedback(int internshipId);

}
