using System.Collections.Generic;

public interface IRecommendationQueries {
     
     List<Entity.Advertisement> GetAdvertisementsOfCompany(int companyId);
     List<Entity.Advertisement> GetAdvertisementsForStudent(int studentId);
     int? CreateAdvertisement(int companyId, Entity.Advertisement advertisement, List<Entity.Skill> skills);
     void MatchAdvertisementForStudent(int advertisementId);
     Entity.Advertisement GetAdvertisement(int advertisementId);
     List<Entity.Student> GetRecommendedCandidates(int companyId, int advertisementId);
     bool CreateSuggestionsForStudent(int advertisementId, int studentId, int companyId);
     bool DeleteAdvertisement(int advertisementId, int companyId);

}
