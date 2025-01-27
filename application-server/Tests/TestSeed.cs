using System;
using System.Threading;

public class TestSeed {

    private readonly IAuthenticationService authentication;
    private readonly string salt;
    private readonly string hashedPassword;

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

        for (int i = 1; i <= 210; i++) {

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

            context.StudentSkills.AddRange(new Entity.StudentSkills
            {
                StudentId = i,
                SkillId = i
            });
            
            
        }
        
        context.SaveChanges();

        for (int i = 1; i <= 205; i++) {

            for (int j = 1; j < 5; j++)
            {
                context.StudentSkills.AddRange(new Entity.StudentSkills
                {
                    StudentId = i,
                    SkillId = i + j
                });
            }

            context.Advertisement.AddRange(new Entity.Advertisement
            {
                CompanyId = i,
                Name = "SeedName",
                CreatedAt = new DateTime(2021, 12, 2, 19, 55, 0),
                Description = "SeedDescription",
                Duration = 12,
                Spots = 3,
                Available = i < 100 ? 2 : 3,
                Open = true,
                Questionnaire = "SeedQuestionnaire"
            });



            for (int j = 0; j < 5; j++)
            {
                context.AdvertisementSkills.AddRange(new Entity.AdvertisementSkills
                {
                    AdvertisementId = i,
                    SkillId = i + j
                });
            }


            context.Internship.AddRange(new Entity.Internship
            {
                CreatedAt = new DateTime(2021, 12, 2, 19, 55, 0),
                AdvertisementId = i,
                StudentId = i,
                CompanyId = i,
                StartDate = new DateTime(2021, 12, 2, 19, 55, 0),
                EndDate = new DateTime(2022, 12, 2, 19, 55, 0)
            });
            

        }
        
        context.SaveChanges();
        
        for (int i = 1; i <= 203; i++) {


            for (int j = 0; j < 3; j++) {
                    context.StudentNotifications.AddRange(new Entity.StudentNotifications {
                        StudentId = i,
                        AdvertisementId = i + j,
                        Type = 'r'
                    });
            }
            

            context.StudentFeedbacks.AddRange(new Entity.StudentFeedback {
                InternshipId = i,
                Rating = 10,
                Comment = "SeedComment"
            });


            context.CompanyFeedbacks.AddRange(new Entity.CompanyFeedback
            {
                InternshipId = i,
                Rating = 10,
                Comment = "SeedComment"
            });
        }
    }

    
    public class SeedHelper {

        private int companyId;
        private int studentId;

        public string Password { get; }

        protected internal SeedHelper() {
            companyId = 0;
            studentId = 0;
            Password = "SeedPassword";
        }

        public string GetNewCompanyUsername() {
            return "SeedCompany" + Interlocked.Increment(ref companyId);
        }

        public string GetNewStudentUsername() {
            return "SeedStudent" + Interlocked.Increment(ref studentId);
        }

        public string GetNewCompanyEmail() {
            return "Seed" + Interlocked.Increment(ref companyId) + "@Company.mail";
        }

        public string GetNewStudentEmail() {
            return "Seed" + Interlocked.Increment(ref studentId) + "@Student.mail";
        }

        public string GetCompanyUsername(int id) {
            return "SeedCompany" + id;
        }

        public string GetStudentUsername(int id) {
            return "SeedStudent" + id;
        }

        public int GetNewCompanyId() {
            return Interlocked.Increment(ref companyId);
        }

        public int GetNewStudentId() {
            return Interlocked.Increment(ref studentId);
        }

    }

}
