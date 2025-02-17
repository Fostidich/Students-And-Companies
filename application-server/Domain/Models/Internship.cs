using System;

public class Internship
{

    public int InternshipId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int StudentId { get; set; }
    public int CompanyId { get; set; }
    public int AdvertisementId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public Internship(Entity.Internship internship)
    {
        InternshipId = internship.InternshipId;
        CreatedAt = internship.CreatedAt;
        StudentId = internship.StudentId;
        CompanyId = internship.CompanyId;
        AdvertisementId = internship.AdvertisementId;
        StartDate = internship.StartDate;
        EndDate = internship.EndDate;
    }

    public DTO.Internship ToDto()
    {
        return new DTO.Internship
        {
            InternshipId = InternshipId,
            CreatedAt = CreatedAt,
            StudentId = StudentId,
            CompanyId = CompanyId,
            AdvertisementId = AdvertisementId,
            StartDate = StartDate,
            EndDate = EndDate,
        };
    }

}