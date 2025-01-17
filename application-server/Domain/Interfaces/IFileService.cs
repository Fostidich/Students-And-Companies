public interface IFileService {

    string GetCvFilePath(string fileName);
    bool SaveFile(string filePath, byte[] fileData);
    byte[] RetrieveFile(string filePath);
    bool DeleteFile(string filePath);

}
