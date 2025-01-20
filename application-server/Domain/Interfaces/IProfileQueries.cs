using System;

public interface IProfileQueries {

    Entity.Company FindCompanyFromId(int id);
    Entity.Student FindStudentFromId(int id);
    bool UpdateSaltAndPassword(UserType type, int id, string salt, string hash);
    bool UpdateUsername(UserType type, int id, string username);
    bool UpdateEmail(UserType type, int id, string email);
    bool UpdateBio(UserType type, int id, string bio);
    bool UpdateHeadquarter(int id, string headquater);
    bool UpdateFiscalCode(int id, string fiscalCode);
    bool UpdateVatNumber(int id, string vatNumber);
    bool UpdateName(int id, string name);
    bool UpdateSurname(int id, string surname);
    bool UpdateUniversity(int id, string university);
    bool UpdateCourseOfStudy(int id, string courseOfStudy);
    bool UpdateGender(int id, char gender);
    bool UpdateBirthDate(int id, DateTime birthDate);
    bool DeleteUser(UserType type, int id);
    int FindSkill(string name);
    bool AddSkill(string name);
    bool AddSkillToStudent(int studentId, int skillId);

}
