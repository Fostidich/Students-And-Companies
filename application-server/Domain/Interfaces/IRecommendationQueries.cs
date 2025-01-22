using System.Collections.Generic;

public interface IRecommendationQueries {
     
     List<Entity.Advertisement> GetAdvertisementsOfCompany(int studentId);
     List<Entity.Advertisement> GetAdvertisementsForStudent(int studentId);
     int? CreateAdvertisement(int companyId, Entity.Advertisement advertisement, List<Entity.Skill> skills);
     void MatchAdvertisementForStudent(int advertisementId);
     void MatchAdvertisementForCompany(int advertisementId);

}
