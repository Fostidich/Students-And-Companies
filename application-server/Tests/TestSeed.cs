using System;

public class TestSeed {

    private readonly IAuthenticationService authentication;
    private readonly string password;
    private readonly string salt;
    private readonly string hashedPassword;

    public TestSeed(IAuthenticationService authentication, string password) {
        this.authentication = authentication;
        this.password = password;
        salt = authentication.GenerateSalt();
        hashedPassword = authentication.HashPassword(salt, password);
    }

    public void SeedDatabase(AppDbContext context) {
        context.Company.AddRange(new Entity.Company {
            Username = "SeedCompany",
            Email = "Seed@Company.mail",
            Salt = salt,
            HashedPassword = hashedPassword,
            Headquarter = "SeedHeadquarter",
            FiscalCode = "SeedFiscalCode",
            VatNumber = "SeedVatNumber",
        });

        context.Student.AddRange(new Entity.Student {
			Username = "SeedStudent",
			Email = "Seed@Student.mail",
			Salt = salt,
			HashedPassword = hashedPassword,
			Name = "SeedName",
			Surname = "SeedSurname",
			University = "SeedUniversity",
			CourseOfStudy = "SeedCourseOfStudy",
			Gender = 'f',
			BirthDate = new DateTime(2001, 12, 2, 19, 55, 0),
	    });

        for (int i = 1; i < 10; i++) {

            context.Company.AddRange(new Entity.Company {
                Username = "SeedCompany" + i,
                Email = "Seed" + i + "@Company.mail",
                Salt = salt,
                HashedPassword = hashedPassword,
                Headquarter = "SeedHeadquarter",
                FiscalCode = "SeedFiscalCode",
                VatNumber = "SeedVatNumber",
            });

            context.Student.AddRange(new Entity.Student {
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

        }

    }

}
