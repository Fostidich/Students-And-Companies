public class Company : User {

    public Company(DTO.RegistrationFormCompany registrationForm) {

    }

    public Company(Entity.Company entity) {

    }

    public DTO.Company ToDto() {
        return new DTO.Company { };
    }


    public Entity.Company ToEntity() {
        return new Entity.Company { };
    }

}
