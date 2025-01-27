using System;

public class Application {

    public int ApplicationId { get; set; }
    public int StudentId { get; set; }
    public int AdvertisementId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }
    public String Questionnaire { get; set; }

    public Application(Entity.Application entity) {
        ApplicationId = entity.ApplicationId;
        StudentId = entity.StudentId;
        AdvertisementId = entity.AdvertisementId;
        CreatedAt = entity.CreatedAt;
        Status = entity.Status;
        Questionnaire = entity.Questionnaire;
    }

    public DTO.Application ToDto() {
        return new DTO.Application {
            ApplicationId = ApplicationId,
            StudentId = StudentId,
            AdvertisementId = AdvertisementId,
            CreatedAt = CreatedAt,
            Status = Status,
            Questionnaire = Questionnaire,
        };
    }

}
