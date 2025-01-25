using System.Collections.Generic;

public interface IRecommendationService {
    
    List<Advertisement> GetAdvertisementsOfCompany(int companyId);
    List<Advertisement> GetAdvertisementsForStudent(int studentId);
    bool CreateAdvertisement(int companyId, DTO.AdvertisementRegistration advertisement);
    Advertisement GetAdvertisement(int advertisementId);
    List<Student> GetRecommendedCandidates(int companyId, int advertisementId);
    bool CreateSuggestionsForStudent(int notificationId, int companyId);
    bool DeleteAdvertisement(int advertisementId, int companyId);
    
}
