public class Company : User
{

    public string Bio { get; set; }
    public string Headquarter { get; set; }
    public string FiscalCode { get; set; }
    public string VatNumber { get; set; }

    public Company(DTO.RegistrationFormCompany registrationForm)
    {
        UserType = UserType.Company;
        Username = registrationForm.Username;
        Email = registrationForm.Email;
        Bio = registrationForm.Bio;
        Headquarter = registrationForm.Headquarter;
        FiscalCode = registrationForm.FiscalCode;
        VatNumber = registrationForm.VatNumber;
    }

    public Company(Entity.Company entity)
    {
        UserType = UserType.Company;
        Id = entity.CompanyId;
        Username = entity.Username;
        Salt = entity.Salt;
        HashedPassword = entity.HashedPassword;
        Email = entity.Email;
        CreatedAt = entity.CreatedAt;
        Bio = entity.Bio;
        Headquarter = entity.Headquarter;
        FiscalCode = entity.FiscalCode;
        VatNumber = entity.VatNumber;
    }

    public DTO.Company ToDto()
    {
        return new DTO.Company
        {
            CompanyId = Id,
            Username = Username,
            Email = Email,
            Bio = Bio,
            Headquarter = Headquarter,
            FiscalCode = FiscalCode,
            VatNumber = VatNumber,
        };
    }

    public Entity.Company ToEntity()
    {
        return new Entity.Company
        {
            CompanyId = Id,
            CreatedAt = CreatedAt,
            Salt = Salt,
            HashedPassword = HashedPassword,
            Username = Username,
            Email = Email,
            Bio = Bio,
            Headquarter = Headquarter,
            FiscalCode = FiscalCode,
            VatNumber = VatNumber,
        };
    }

}
