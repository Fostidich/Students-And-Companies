public class CompanyNotifications {
    
    public int CompanyNotificationId { get; set; }
    public int CompanyId { get; set; }
    public int StudentId { get; set; }
    public int AdvertisementId { get; set; }
    
    
    public CompanyNotifications (Entity.CompanyNotifications companyNotifications) {
        CompanyNotificationId = companyNotifications.CompanyNotificationId;
        CompanyId = companyNotifications.CompanyId;
        StudentId = companyNotifications.StudentId;
        AdvertisementId = companyNotifications.AdvertisementId;
    }
    
    public DTO.CompanyNotifications ToDto() {
        return new DTO.CompanyNotifications {
            CompanyNotificationId = CompanyNotificationId,
            CompanyId = CompanyId,
            StudentId = StudentId,
            AdvertisementId = AdvertisementId,
        };
    }
    
    
}