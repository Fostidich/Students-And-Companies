using System;

public class Student : User {

    public string Bio { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string University { get; set; }
    public string CourseOfStudy { get; set; }
    public char Gender { get; set; }
    public DateTime BirthDate { get; set; }

    public Student(DTO.RegistrationFormStudent registrationForm) {
        UserType = UserType.Student;
        Username = registrationForm.Username;
        Email = registrationForm.Email;
        Bio = registrationForm.Bio;
        Name = registrationForm.Name;
        Surname = registrationForm.Surname;
        University = registrationForm.University;
        CourseOfStudy = registrationForm.CourseOfStudy;
        Gender = registrationForm.Gender;
        BirthDate = registrationForm.BirthDate;
    }

    public Student(Entity.Student entity) {
        UserType = UserType.Student;
        Id = entity.StudentId;
        Salt = entity.Salt;
        HashedPassword = entity.HashedPassword;
        Username = entity.Username;
        Email = entity.Email;
        Bio = entity.Bio;
        Name = entity.Name;
        Surname = entity.Surname;
        University = entity.University;
        CourseOfStudy = entity.CourseOfStudy;
        Gender = entity.Gender;
        BirthDate = entity.BirthDate;
    }

    public DTO.Student ToDto() {
        return new DTO.Student {
            StudentId = Id,
            Username = Username,
            Email = Email,
            Bio = Bio,
            Name = Name,
            Surname = Surname,
            University = University,
            CourseOfStudy = CourseOfStudy,
            Gender = Gender,
            BirthDate = BirthDate,
        };
    }

    public Entity.Student ToEntity() {
        return new Entity.Student {
            StudentId = Id,
            CreatedAt = CreatedAt,
            Salt = Salt,
            HashedPassword = HashedPassword,
            Username = Username,
            Email = Email,
            Bio = Bio,
            Name = Name,
            Surname = Surname,
            University = University,
            CourseOfStudy = CourseOfStudy,
            Gender = Gender,
            BirthDate = BirthDate
        };
    }

}
