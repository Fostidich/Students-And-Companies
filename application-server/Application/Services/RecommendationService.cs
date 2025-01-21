using System.Collections.Generic;
using System.Linq;

public class RecommendationService : IRecommendationService {

    private readonly IRecommendationQueries queries;

    public RecommendationService(IRecommendationQueries queries) {
        this.queries = queries;
    }
    
    public List<Advertisement> GetAdvertisementsForStudent(int studentId) {
        // Get recommendation advertisements for a student
        List<Entity.Advertisement> advertisements = queries.GetAdvertisementsForStudent(studentId);
        
        // If no advertisements are found, return null
        if(advertisements == null) {
            return null;
        }
        
        // Convert Entity.Advertisement to Advertisement
        List<Advertisement> adv = advertisements.Select(adv => new Advertisement(adv)).ToList();
        
        return adv;
    }
    
    public List<Advertisement> GetAdvertisementsOfCompany(int companyId) {
        // Get advertisements of a company
        List<Entity.Advertisement> advertisements = queries.GetAdvertisementsOfCompany(companyId);
        
        // If no advertisements are found, return null
        if(advertisements == null) {
            return null;
        }
        
        // Convert Entity.Advertisement to Advertisement
        List<Advertisement> adv = advertisements.Select(adv => new Advertisement(adv)).ToList();
        
        return adv;
    }
    
    public bool CreateAdvertisement(int companyId, DTO.AdvertisementRegistration advertisement) {
        Advertisement adv = new Advertisement(advertisement);
        
        // Add advertisement data to DB
        return queries.CreateAdvertisement(companyId, adv.ToEntity());
    }

}
