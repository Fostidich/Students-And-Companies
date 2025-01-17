using System;
using System.IO;

public class FileService : IFileService {

    public string GetCvFilePath(string fileName) {
        // Combine path using the appropriate separator
        string directory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".students_and_companies", "files", "cv");

        // Ensure the directory exists
        Directory.CreateDirectory(directory);

        // Complete filename
        string fullFileName = "cv_" + fileName + ".pdf";

        // Return the full file path
        return Path.Combine(directory, fullFileName);
    }

    public bool SaveFile(string filePath, byte[] fileData) {
        // Try to save the file to the disk
        try {
            File.WriteAllBytes(filePath, fileData);
            return true;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public byte[] RetrieveFile(string filePath) {
        // Check if the file exists before trying to read
        if (File.Exists(filePath)) {
            // Read the file as a byte array
            byte[] fileData = File.ReadAllBytes(filePath);
            return fileData;
        }

        throw new FileNotFoundException("The file was not found", filePath);
    }

    public bool DeleteFile(string filePath) {
        // File does not exist
        if (!File.Exists(filePath)) {
            return false;
        }

        // Try to delete file
        try {
            File.Delete(filePath);
            return true;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

}
