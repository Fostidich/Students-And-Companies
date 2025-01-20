using System.IO;
using Microsoft.AspNetCore.Http;

public class ProfileService : IProfileService {

    private readonly IProfileQueries queries;
    private readonly IAuthenticationService authentication;
    private readonly IAuthenticationQueries authenticationQueries;
    private readonly IFileService files;

    public ProfileService(IProfileQueries queries,
            IAuthenticationService authentication,
            IAuthenticationQueries authenticationQueries,
            IFileService files) {
        this.queries = queries;
        this.authentication = authentication;
        this.authenticationQueries = authenticationQueries;
        this.files = files;
    }

    public Company GetCompany(int id) {
        // Search user in the DB
        Entity.Company user = queries.FindCompanyFromId(id);

        // Return user or null
        if (user == null) return null;
        return new Company(user);
    }

    public Student GetStudent(int id) {
        // Search user in the DB
        Entity.Student user = queries.FindStudentFromId(id);

        // Return user or null
        if (user == null) return null;
        return new Student(user);
    }

    public bool UpdateProfileCompany(int userId, DTO.ProfileUpdateCompany updateForm) {
        bool errors = false;

        // Change password
        if (!string.IsNullOrWhiteSpace(updateForm.Password)) {
            // Retrieve salt and hashed password
            var salt = authentication.GenerateSalt();
            var hash = authentication.HashPassword(salt, updateForm.Password);

            // Update salt and password
            if (!queries.UpdateSaltAndPassword(UserType.Company, userId, salt, hash))
                errors = true;;
        }

        // Update username
        if (!string.IsNullOrWhiteSpace(updateForm.Username)) {
            if (!queries.UpdateUsername(UserType.Company, userId, updateForm.Username))
                errors = true;
        }

        // Update email
        if (!string.IsNullOrWhiteSpace(updateForm.Email)) {
            if (!queries.UpdateEmail(UserType.Company, userId, updateForm.Email))
                errors = true;
        }

        // Update bio
        if (!string.IsNullOrWhiteSpace(updateForm.Bio)) {
            if (!queries.UpdateBio(UserType.Company, userId, updateForm.Bio))
                errors = true;
        }

        // Update headquarter
        if (!string.IsNullOrWhiteSpace(updateForm.Headquarter)) {
            if (!queries.UpdateHeadquarter(userId, updateForm.Headquarter))
                errors = true;
        }

        // Update fiscal code
        if (!string.IsNullOrWhiteSpace(updateForm.FiscalCode)) {
            if (!queries.UpdateFiscalCode(userId, updateForm.FiscalCode))
                errors = true;
        }

        // Update VAT number
        if (!string.IsNullOrWhiteSpace(updateForm.VatNumber)) {
            if (!queries.UpdateVatNumber(userId, updateForm.VatNumber))
                errors = true;
        }

        return !errors;
    }

    public bool UpdateProfileStudent(int userId, DTO.ProfileUpdateStudent updateForm) {
        bool errors = false;

        // Change password
        if (!string.IsNullOrWhiteSpace(updateForm.Password)) {
            // Retrieve salt and hashed password
            var salt = authentication.GenerateSalt();
            var hash = authentication.HashPassword(salt, updateForm.Password);

            // Update salt and password
            if (!queries.UpdateSaltAndPassword(UserType.Student, userId, salt, hash))
                errors = true;;
        }

        // Update username
        if (!string.IsNullOrWhiteSpace(updateForm.Username)) {
            if (!queries.UpdateUsername(UserType.Student, userId, updateForm.Username))
                errors = true;
        }

        // Update email
        if (!string.IsNullOrWhiteSpace(updateForm.Email)) {
            if (!queries.UpdateEmail(UserType.Student, userId, updateForm.Email))
                errors = true;
        }

        // Update bio
        if (!string.IsNullOrWhiteSpace(updateForm.Bio)) {
            if (!queries.UpdateBio(UserType.Student, userId, updateForm.Bio))
                errors = true;
        }

        // Update name
        if (!string.IsNullOrWhiteSpace(updateForm.Name)) {
            if (!queries.UpdateName(userId, updateForm.Name))
                errors = true;
        }

        // Update surname
        if (!string.IsNullOrWhiteSpace(updateForm.Surname)) {
            if (!queries.UpdateSurname(userId, updateForm.Surname))
                errors = true;
        }

        // Update university
        if (!string.IsNullOrWhiteSpace(updateForm.University)) {
            if (!queries.UpdateUniversity(userId, updateForm.University))
                errors = true;
        }

        // Update course of study
        if (!string.IsNullOrWhiteSpace(updateForm.CourseOfStudy)) {
            if (!queries.UpdateCourseOfStudy(userId, updateForm.CourseOfStudy))
                errors = true;
        }

        // Update gender
        if (updateForm.Gender != null) {
            if (!queries.UpdateGender(userId, updateForm.Gender.Value))
                errors = true;
        }

        // Update birth date
        if (updateForm.BirthDate != null) {
            if (!queries.UpdateBirthDate(userId, updateForm.BirthDate.Value))
                errors = true;
        }

        return !errors;
    }

    public bool IsCompanyUpdateFormValid(DTO.ProfileUpdateCompany updateForm) {
        var username = updateForm.Username;
        var email = updateForm.Email;

        // Check username uniqueness
        if (!string.IsNullOrWhiteSpace(username)) {
            if (authenticationQueries.FindCompanyFromUsername(username) != null)
                return false;
            if (authenticationQueries.FindStudentFromUsername(username) != null)
                return false;
        }

        // Check email uniqueness
        if (!string.IsNullOrWhiteSpace(email)) {
            if (authenticationQueries.FindCompanyFromEmail(email.ToLowerInvariant()) != null)
                return false;
            if (authenticationQueries.FindStudentFromEmail(email.ToLowerInvariant()) != null)
                return false;
        }

        // Checks passed
        return true;
    }

    public bool IsStudentUpdateFormValid(DTO.ProfileUpdateStudent updateForm) {
        var username = updateForm.Username;
        var email = updateForm.Email;

        // Check username uniqueness
        if (!string.IsNullOrWhiteSpace(username)) {
            if (authenticationQueries.FindStudentFromUsername(username) != null)
                return false;
            if (authenticationQueries.FindCompanyFromUsername(username) != null)
                return false;
        }

        // Check email uniqueness
        if (!string.IsNullOrWhiteSpace(email)) {
            if (authenticationQueries.FindStudentFromEmail(email.ToLowerInvariant()) != null)
                return false;
            if (authenticationQueries.FindCompanyFromEmail(email.ToLowerInvariant()) != null)
                return false;
        }

        // Checks passed
        return true;
    }


    public bool CheckCvValidity(IFormFile file) {
        // Check actual file presence
        if (file == null || file.Length == 0)
            return false;

        // Check if the file is a PDF
        if (file.ContentType != "application/pdf")
            return false;

        // Check file size
        if (file.Length > 5 * 1024 * 1024)
            return false;

        return true;
    }

    public bool StoreCvFile(int userId, IFormFile file) {
        // Convert file form to byte array
        byte[] fileBytes;
        using var memoryStream = new MemoryStream();
        file.CopyTo(memoryStream);
        fileBytes = memoryStream.ToArray();

        // Compute file path
        string filePath = files.GetCvFilePath(userId.ToString());

        // Store file in file system
        return files.SaveFile(filePath, fileBytes);
   }

    public IFormFile RetrieveCvFile(int userId) {
        // Compute file path
        string filePath = files.GetCvFilePath(userId.ToString());

        // Retrieve file from file system
        byte[] fileBytes;
        try {
            fileBytes = files.RetrieveFile(filePath);
        } catch {
            fileBytes = null;
        }

        // Early not found return
        if (fileBytes == null)
            return null;

        // Convert file to form file
        var stream = new MemoryStream(fileBytes);
        return new FormFile(stream, 0, fileBytes.Length, null, Path.GetFileName(filePath)) {
            Headers = new HeaderDictionary(),
            ContentType = "application/pdf"
        };
    }

    public bool DeleteCv(int userId) {
        // Compute file path
        string filePath = files.GetCvFilePath(userId.ToString());

        // Delete file
        return files.DeleteFile(filePath);
    }

    public bool DeleteUser(UserType userType, int userId) {
        return queries.DeleteUser(userType, userId);
    }

    public bool AddSkill(int studentId, string name) {
        // Find skill id
        int skillId = queries.FindSkill(name);

        // Add skill if not present
        if (skillId == 0) {
            // Add new skill
            if (!queries.AddSkill(name))
                return false;

            // Find new skill id
            skillId = queries.FindSkill(name);

            // Error occurred
            if (skillId == 0)
                return false;
        }

        // Add skill id to students skills
        return queries.AddSkillToStudent(studentId, skillId);
    }

}
