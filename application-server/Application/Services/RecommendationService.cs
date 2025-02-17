using System.Collections.Generic;
using System.Linq;

public class RecommendationService : IRecommendationService
{

    private readonly IRecommendationQueries queries;

    public RecommendationService(IRecommendationQueries queries)
    {
        this.queries = queries;
    }

    public List<Advertisement> GetAdvertisementsForStudent(int studentId)
    {
        // Get recommendation advertisements for a student
        List<Entity.Advertisement> advertisements = queries.GetAdvertisementsForStudent(studentId);

        // If exceptions occur, return null
        if (advertisements == null)
        {
            return null;
        }

        // Convert Entity.Advertisement to Advertisement
        List<Advertisement> adv = advertisements
            .Select(adv => new Advertisement(adv))
            .ToList();

        return adv;
    }

    public List<Advertisement> GetAdvertisementsOfCompany(int companyId)
    {
        // Get advertisements of a company
        List<Entity.Advertisement> advertisements = queries.GetAdvertisementsOfCompany(companyId);

        // If exceptions occur, return null
        if (advertisements == null)
        {
            return null;
        }

        // Convert Entity.Advertisement to Advertisement
        List<Advertisement> adv = advertisements
            .Select(adv => new Advertisement(adv))
            .ToList();

        return adv;
    }

    public bool CreateAdvertisement(int companyId, DTO.AdvertisementRegistration advertisement)
    {
        Advertisement adv = new Advertisement(advertisement);

        // Convert Skills to DTO.SkillRegistration
        List<DTO.SkillRegistration> skillsDto = advertisement.Skills
            .Select(skill => new DTO.SkillRegistration { Name = skill })
            .ToList();

        // Convert DTO.SkillRegistration to Skill
        List<Skill> skills = skillsDto
            .Select(skill => new Skill(skill))
            .ToList();

        // Convert Skill to Entity.Skill
        List<Entity.Skill> skillsEntity = skills
            .Select(skill => skill.ToEntity())
            .ToList();

        // Create advertisement
        int? advId = queries.CreateAdvertisement(companyId, adv.ToEntity(), skillsEntity);

        if (advId == -1)
        {
            return false;
        }

        if (advId != null)
        {
            queries.MatchAdvertisementForStudent(advId.Value);
            return true;
        }
        return false;
    }

    public Advertisement GetAdvertisement(int advertisementId)
    {
        // Get advertisement by ID
        Entity.Advertisement advertisement = queries.GetAdvertisement(advertisementId);

        // If no advertisement is found, return null
        if (advertisement == null)
        {
            return null;
        }

        // Convert Entity.Advertisement to Advertisement
        Advertisement adv = new Advertisement(advertisement);

        return adv;
    }


    public List<Student> GetRecommendedCandidates(int companyId, int advertisementId)
    {
        // Get recommended candidates for a company
        List<Entity.Student> studentsEntity = queries.GetRecommendedCandidates(companyId, advertisementId);

        // if you don't have the permission to get the candidates for this advertisement because you are not the owner of the advertisement
        if (studentsEntity == null)
        {
            return null;
        }

        // Convert Entity.Student to Student
        List<Student> students = studentsEntity.Select(student => new Student(student)).ToList();

        return students;
    }


    public bool CreateSuggestionsForStudent(int advertisementId, int studentId, int companyId)
    {
        // Create suggestion for a student
        return queries.CreateSuggestionsForStudent(advertisementId, studentId, companyId);
    }

    public bool DeleteAdvertisement(int advertisementId, int companyId)
    {
        // Delete advertisement
        return queries.DeleteAdvertisement(advertisementId, companyId);
    }

}
