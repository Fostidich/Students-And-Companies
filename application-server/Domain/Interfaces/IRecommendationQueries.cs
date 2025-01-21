using System.Collections.Generic;

public interface IRecommendationQueries {
     
     List<Entity.Advertisement> GetAdvertisementsOfCompany(int studentId);
     List<Entity.Advertisement> GetAdvertisementsForStudent(int studentId);
     bool CreateAdvertisement(int companyId, Entity.Advertisement advertisement);

}
