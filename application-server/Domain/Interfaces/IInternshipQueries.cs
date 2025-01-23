using System.Collections.Generic;

public interface IInternshipQueries {
    Entity.Internship GetInternshipForStudent(int studentId);
    List<Entity.Internship> GetInternshipFromAdvertisement(int advertisementId);
    bool CreateStudentFeedback(int internshipId, Entity.StudentFeedback feedback);
    bool CreateCompanyFeedback(int internshipId, Entity.CompanyFeedback feedback);
    Entity.StudentFeedback GetStudentFeedback(int internshipId);
    Entity.CompanyFeedback GetCompanyFeedback(int internshipId);

}
