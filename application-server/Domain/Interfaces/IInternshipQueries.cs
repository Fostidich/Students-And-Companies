using System.Collections.Generic;

public interface IInternshipQueries {
    Entity.Internship GetInternshipForStudent(int studentId);
    List<Entity.Internship> GetInternshipFromAdvertisement(int advertisementId);
    bool CreateStudentFeedback(int internshipId, Entity.StudentFeedback feedback, int studentId);
    bool CreateCompanyFeedback(int internshipId, Entity.CompanyFeedback feedback, int companyId);
    Entity.StudentFeedback GetStudentFeedback(int internshipId, int userId, string role);
    Entity.CompanyFeedback GetCompanyFeedback(int internshipId, int userId, string role);

}
