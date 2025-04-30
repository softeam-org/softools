using System.Diagnostics;
using DocumentFormat.OpenXml.Packaging;

namespace Softools.Documentos.Services;

public class TemplateService
{
    public string GerarDocumento(string templatePath, Dictionary<string, string> campos)
    {
        if (!File.Exists(templatePath))
            throw new FileNotFoundException("Template não encontrado", templatePath);

        try
        {
            using var test = WordprocessingDocument.Open(templatePath, false);
        }
        catch (Exception ex)
        {
            throw new InvalidDataException("Template está corrompido ou não é um .docx válido", ex);
        }

        var id = Guid.CreateVersion7().ToString();
        var output = Path.Combine(Utils.GetGeneratedFolderPath(), $"{id}.docx");
        PreencherTemplate(templatePath, output, campos);

        var psi = new ProcessStartInfo
        {
            FileName = "libreoffice",
            Arguments = $"--headless --convert-to pdf \"{output}\" --outdir \"{Utils.GetGeneratedFolderPath()}\"",
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi);
        var error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
            throw new Exception($"Erro ao gerar PDF: {error}");

        var pdfPath = Path.ChangeExtension(output, ".pdf");
        return pdfPath;
    }

    static void PreencherTemplate(string templatePath, string outputPath, Dictionary<string, string> variaveis)
    {
        File.Copy(templatePath, outputPath, true);

        using var wordDoc = WordprocessingDocument.Open(outputPath, true);
        var docPart = wordDoc.MainDocumentPart!;
        var doc = docPart.Document!;

        var xml = doc.InnerXml;

        foreach (var par in variaveis)
            xml = xml.Replace($"{{{{{par.Key}}}}}", par.Value);

        doc.InnerXml = xml;
        doc.Save();
    }
}
