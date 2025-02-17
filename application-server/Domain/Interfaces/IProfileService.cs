using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

public interface IProfileService
{

    Company GetCompany(int id);
    Student GetStudent(int id);
    bool UpdateProfileCompany(int id, DTO.ProfileUpdateCompany updateForm);
    bool UpdateProfileStudent(int id, DTO.ProfileUpdateStudent updateForm);
    bool IsCompanyUpdateFormValid(DTO.ProfileUpdateCompany updateForm);
    bool IsStudentUpdateFormValid(DTO.ProfileUpdateStudent updateForm);
    bool CheckCvValidity(IFormFile file);
    bool StoreCvFile(int id, IFormFile file);
    IFormFile RetrieveCvFile(int id);
    bool DeleteCv(int id);
    bool DeleteUser(UserType type, int id);
    bool AddSkill(int id, string name);
    List<Skill> GetSkills(int id);
    bool DeleteSkill(int skillId, int studentId);

}
