using Microsoft.AspNetCore.Http;

public interface IProfileService {

    Company GetCompany(int id);
    Student GetStudent(int id);
    bool UpdateProfileCompany(int userId, DTO.ProfileUpdateCompany updateForm);
    bool UpdateProfileStudent(int userId, DTO.ProfileUpdateStudent updateForm);
    bool IsCompanyUpdateFormValid(DTO.ProfileUpdateCompany updateForm);
    bool IsStudentUpdateFormValid(DTO.ProfileUpdateStudent updateForm);
    bool CheckCvValidity(IFormFile file);
    bool StoreCvFile(int userId, IFormFile file);
    IFormFile RetrieveCvFile(int userId);
    bool DeleteCv(int userId);

}
