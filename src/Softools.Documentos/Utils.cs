namespace Softools.Documentos;

public class Utils
{
    public static string GetUploadFolderPath()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "uploads");
        Directory.CreateDirectory(path); // ensure it exists
        return path;
    }
    
    public static string GetGeneratedFolderPath()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "generated");
        Directory.CreateDirectory(path); // ensure it exists
        return path;
    }

}