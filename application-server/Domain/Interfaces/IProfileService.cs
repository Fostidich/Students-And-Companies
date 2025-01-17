using Microsoft.AspNetCore.Http;

public interface IProfileService {

    User GetUser(int id);
    bool UpdateProfile(int userId, DTO.ProfileUpdate updateForm);
    bool IsUpdateFormValid(DTO.ProfileUpdate updateForm);
    bool CheckCvValidity(IFormFile file);
    bool StoreCvFile(int userId, IFormFile file);
    IFormFile RetrieveCvFile(int userId);
    bool DeleteCv(int userId);

}
