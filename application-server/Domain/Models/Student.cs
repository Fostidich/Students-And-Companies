public class Student : User {

    public Student(DTO.RegistrationFormStudent registrationForm) {

    }

    public Student(Entity.Student entity) {

    }

    public DTO.Student ToDto() {
        return new DTO.Student { };
    }

    public Entity.Student ToEntity() {
        return new Entity.Student { };
    }

}
