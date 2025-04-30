namespace Softools.Documentos;

public class Utils
{
    public static string GetUploadFolderPath()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "uploads");
        Directory.CreateDirectory(path); // ensure it exists
        return path;
    }

}