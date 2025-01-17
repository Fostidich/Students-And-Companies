using System.IO;
using System.Threading.Tasks;
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

    public User GetUser(int id) {
        // Search user in the DB
        Entity.User user = queries.FindFromUserId(id);

        // Return user or null
        if (user == null) return null;
        return new User(user);
    }

    public bool UpdateProfile(int userId, DTO.ProfileUpdate updateForm) {
        bool errors = false;

        // Change password
        if (!string.IsNullOrWhiteSpace(updateForm.Password)) {
            // Retrieve salt and hashed password
            var salt = authentication.GenerateSalt();
            var hash = authentication.HashPassword(salt, updateForm.Password);

            // Update salt and password
            if (!queries.UpdateSaltAndPassword(userId, salt, hash))
                errors = true;;
        }

        // Update username
        if (!string.IsNullOrWhiteSpace(updateForm.Username)) {
            if (!queries.UpdateUsername(userId, updateForm.Username))
                errors = true;
        }

        // Update email
        if (!string.IsNullOrWhiteSpace(updateForm.Email)) {
            if (!queries.UpdateEmail(userId, updateForm.Email))
                errors = true;
        }

        return !errors;
    }

    public bool IsUpdateFormValid(DTO.ProfileUpdate updateForm) {
        var username = updateForm.Username;
        var email = updateForm.Email;

        // Check username uniqueness
        if (!string.IsNullOrWhiteSpace(username)) {
            if (authenticationQueries.FindFromUsername(username) != null)
                return false;
        }

        // Check email uniqueness
        if (!string.IsNullOrWhiteSpace(email)) {
            if (authenticationQueries.FindFromEmail(email.ToLowerInvariant()) != null)
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
        bool outcome = files.SaveFile(filePath, fileBytes);

        // Add filepath to students table in DB
        if (outcome) {
            outcome &= queries.SetCvFilePath(userId, filePath);
        }

        return outcome;
    }

    public IFormFile RetrieveCvFile(int userId) {
        // Compute file path
        string filePath = queries.GetCvFilePath(userId);
        if (string.IsNullOrWhiteSpace(filePath))
            return null;

        // Retrieve file from file system
        byte[] fileBytes;
        try {
            fileBytes = files.RetrieveFile(filePath);
        } catch {
            fileBytes = null;
        }

        // Convert file to form file
        var stream = new MemoryStream(fileBytes);
        return new FormFile(stream, 0, fileBytes.Length, null, Path.GetFileName(filePath)) {
            Headers = new HeaderDictionary(),
            ContentType = "application/pdf"
        };
    }

    public bool DeleteCv(int userId) {
        // Compute file path
        string filePath = queries.GetCvFilePath(userId);
        if (string.IsNullOrWhiteSpace(filePath))
            return false;

        // Delete file
        bool outcome = files.DeleteFile(filePath);

        // Delete file path from students DB row
        if (outcome)
            outcome &= queries.RemoveCvFilePath(userId);

        return outcome;
    }

}
