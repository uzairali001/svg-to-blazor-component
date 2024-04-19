// See https://aka.ms/new-console-template for more information

using System.Globalization;
using System.Text;

string? sourceDir = args.ElementAtOrDefault(0);
string outputDir = args.ElementAtOrDefault(1) ?? "output";

if (args.Length == 0 || args.ElementAt(0) == "help")
{
    Console.WriteLine("Usage: SvgToBlazorComponent <source directory> <output directory>");
    return 0;
}

if (string.IsNullOrEmpty(sourceDir))
{
    Console.Error.WriteLine("Source directory not set");
    return -1;
}

if (!Directory.Exists(sourceDir))
{
    Console.WriteLine($"Error: Source directory '{sourceDir}' does not exist.");
    return -1;
}

if (!Directory.Exists(outputDir))
{
    Directory.CreateDirectory(outputDir);
    Console.WriteLine($"Created output directory: '{outputDir}'.");
}

await ProcessSvgFiles(sourceDir, outputDir);


static async Task ProcessSvgFiles(string sourceDir, string outputDir)
{
    var files = Directory.EnumerateFiles(sourceDir, "*.svg", SearchOption.AllDirectories);
    Console.WriteLine($"Converting {files.Count()} Files");
    await Task.Delay(1000);

    foreach (var filePath in files)
    {
        string subDirectories = GetPascalCaseString(Path.GetDirectoryName(filePath.Replace(sourceDir + "\\", "")) ?? string.Empty);
        string outputDirectories = Path.Combine(outputDir, subDirectories);
        Directory.CreateDirectory(outputDirectories);

        string fileName = Path.GetFileNameWithoutExtension(filePath);
        string fileNamePascal = GetPascalCaseString(fileName);

        string outputPath = Path.Combine(outputDirectories, $"{fileNamePascal}.razor");

        string svgContent = ReadSvgFile(filePath);
        WriteBlazorComponent(outputPath, svgContent);

        Console.WriteLine($"Converted '{filePath}' to Blazor component '{outputPath}'.");
    }
}

static string ReadSvgFile(string filePath)
{
    using StreamReader reader = new(filePath, Encoding.UTF8);
    return reader.ReadToEnd();
}

static string GetPascalCaseString(string str)
{
    TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
    return string.Join("", str.Split("-").Select(textInfo.ToTitleCase)).Replace(" ", "");
}


static void WriteBlazorComponent(string outputPath, string svgContent)
{
    File.WriteAllText(outputPath, svgContent);
}


return 0;