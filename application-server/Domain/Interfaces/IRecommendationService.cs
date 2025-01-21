using System.Collections.Generic;

public interface IRecommendationService {
    
    List<Advertisement> GetAdvertisementsOfCompany(int studentId);
    List<Advertisement> GetAdvertisementsForStudent(int studentId);
    bool CreateAdvertisement(int companyId, DTO.AdvertisementRegistration advertisement);

}
