public interface IProfileService {

    DTO.User GetUser(int id);
    bool UpdateProfile(int userId, DTO.ProfileUpdate updateForm);
    bool IsUpdateFormValid(DTO.ProfileUpdate updateForm);

}
