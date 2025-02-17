using System;

public class Advertisement
{
    public int AdvertisementId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CompanyId { get; set; }
    public string Description { get; set; }
    public int Duration { get; set; }
    public int Spots { get; set; }
    public int Available { get; set; }
    public bool Open { get; set; }
    public String Questionnaire { get; set; }


    public Advertisement(Entity.Advertisement advertisement)
    {
        AdvertisementId = advertisement.AdvertisementId;
        Name = advertisement.Name;
        CreatedAt = advertisement.CreatedAt;
        CompanyId = advertisement.CompanyId;
        Description = advertisement.Description;
        Duration = advertisement.Duration;
        Spots = advertisement.Spots;
        Available = advertisement.Available;
        Open = advertisement.Open;
        Questionnaire = advertisement.Questionnaire;
    }

    public Advertisement(DTO.Advertisement advertisement)
    {
        AdvertisementId = advertisement.AdvertisementId;
        Name = advertisement.Name;
        CreatedAt = advertisement.CreatedAt;
        CompanyId = advertisement.CompanyId;
        Description = advertisement.Description;
        Duration = advertisement.Duration;
        Spots = advertisement.Spots;
        Available = advertisement.Available;
        Open = advertisement.Open;
        Questionnaire = advertisement.Questionnaire;
    }

    public Advertisement(DTO.AdvertisementRegistration advertisement)
    {
        Name = advertisement.Name;
        Description = advertisement.Description;
        Duration = advertisement.Duration;
        Spots = advertisement.Spots;
        Questionnaire = advertisement.Questionnaire;
    }

    public DTO.Advertisement ToDto()
    {
        return new DTO.Advertisement
        {
            AdvertisementId = AdvertisementId,
            Name = Name,
            CreatedAt = CreatedAt,
            CompanyId = CompanyId,
            Description = Description,
            Duration = Duration,
            Spots = Spots,
            Available = Available,
            Open = Open,
            Questionnaire = Questionnaire,
        };
    }

    public Entity.Advertisement ToEntity()
    {
        return new Entity.Advertisement
        {
            AdvertisementId = AdvertisementId,
            Name = Name,
            CreatedAt = CreatedAt,
            CompanyId = CompanyId,
            Description = Description,
            Duration = Duration,
            Spots = Spots,
            Available = Available,
            Open = Open,
            Questionnaire = Questionnaire,
        };
    }

}