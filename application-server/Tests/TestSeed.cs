using System;
using System.Collections.Generic;
using System.Threading;

public class TestSeed {

    private readonly IAuthenticationService authentication;
    private readonly string salt;
    private readonly string hashedPassword;
    private readonly int numberOfSeeds = 500;

    public string Password { get; }
    public SeedHelper Seed { get; }

    public TestSeed(IAuthenticationService authentication) {
        this.authentication = authentication;
        Seed = new SeedHelper();
        Password = Seed.Password;
        salt = authentication.GenerateSalt();
        hashedPassword = authentication.HashPassword(salt, Password);
    }

    public void SeedDatabase(AppDbContext context) {

        for (int i = 1; i <= numberOfSeeds + 10; i++) {

            context.Company.AddRange(new Entity.Company
            {
                Username = "SeedCompany" + i,
                Email = "Seed" + i + "@Company.mail",
                Salt = salt,
                HashedPassword = hashedPassword,
                Headquarter = "SeedHeadquarter",
                FiscalCode = "SeedFiscalCode",
                VatNumber = "SeedVatNumber",
            });

            context.Student.AddRange(new Entity.Student
            {
                Username = "SeedStudent" + i,
                Email = "Seed" + i + "@Student.mail",
                Salt = salt,
                HashedPassword = hashedPassword,
                Name = "SeedName",
                Surname = "SeedSurname",
                University = "SeedUniversity",
                CourseOfStudy = "SeedCourseOfStudy",
                Gender = 'm',
                BirthDate = new DateTime(2001, 12, 2, 19, 55, 0),
            });

            context.Skill.AddRange(new Entity.Skill
            {
                Name = "Skill" + i
            });

        }

        context.SaveChanges();

        for (int i = 1; i <= numberOfSeeds + 5; i++) {

            for (int j = 0; j < 5; j++) {

                context.StudentSkills.AddRange(new Entity.StudentSkills
                {
                    StudentId = i,
                    SkillId = i + j
                });

            }

            context.Advertisement.AddRange(new Entity.Advertisement
            {
                CompanyId = i,
                Name = "SeedName" + i,
                Description = "SeedDescription",
                Duration = 12,
                Spots = 3,
                Available = i < 100 ? 2 : 3,
                Open = true,
                Questionnaire = "SeedQuestionnaire"
            });

            for (int j = 0; j < 5; j++) {

                context.AdvertisementSkills.AddRange(new Entity.AdvertisementSkills
                {
                    AdvertisementId = i,
                    SkillId = i + j
                });

            }

            context.Internship.AddRange(new Entity.Internship
            {
                AdvertisementId = i,
                StudentId = i,
                CompanyId = i,
                StartDate = new DateTime(2021, 12, 2, 19, 55, 0),
                EndDate = new DateTime(2022, 12, 2, 19, 55, 0)
            });

        }

        context.SaveChanges();

        for (int i = 1; i <= numberOfSeeds + 3 ; i++) {

            for (int j = 0; j < 3; j++) {

                context.StudentNotifications.AddRange(new Entity.StudentNotifications {
                    StudentId = i,
                    AdvertisementId = i + j,
                    Type = "RECOMMENDED"
                });

            }

            context.CompanyFeedback.AddRange(new Entity.CompanyFeedback {
                InternshipId = i,
                Rating = 10,
                Comment = "SeedComment"
            });

            context.StudentFeedback.AddRange(new Entity.StudentFeedback {
                InternshipId = i,
                Rating = 10,
                Comment = "SeedComment"
            });

        }

        context.SaveChanges();

    }

    public class SeedHelper {

        private int id;

        public string Password { get; }

        protected internal SeedHelper() {
            id = 0;
            Password = "SeedPassword";
        }

        public void BlackListCompany(int id) {
        }

        public void BlackListStudent(int id) {
        }

        public int GetNewCompanyId() {
            return Interlocked.Increment(ref id);
        }

        public int GetNewStudentId() {
            return Interlocked.Increment(ref id);
        }

        public string GetNewCompanyUsername() {
            return "SeedCompany" + GetNewCompanyId();
        }

        public string GetNewStudentUsername() {
            return "SeedStudent" + GetNewStudentId();
        }

        public string GetNewCompanyEmail() {
            return "Seed" + GetNewCompanyId() + "@Company.mail";
        }

        public string GetNewStudentEmail() {
            return "Seed" + GetNewStudentId() + "@Student.mail";
        }

        public string GetCompanyUsername(int id) {
            return "SeedCompany" + id;
        }

        public string GetStudentUsername(int id) {
            return "SeedStudent" + id;
        }

    }

}
