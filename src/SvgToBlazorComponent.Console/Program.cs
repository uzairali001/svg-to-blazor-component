// See https://aka.ms/new-console-template for more information

using HtmlAgilityPack;

using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

string? sourceDir = args.ElementAtOrDefault(0);
string outputDir = args.ElementAtOrDefault(1) ?? (Path.Combine(Environment.CurrentDirectory, "output"));

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

return 0;


static async Task ProcessSvgFiles(string sourceDir, string outputDir)
{
    var files = Directory.EnumerateFiles(sourceDir, "*.svg", SearchOption.AllDirectories);
    Console.WriteLine($"Converting {files.Count()} Files");
    await Task.Delay(1000);
    Stopwatch stopwatch = Stopwatch.StartNew();

    foreach (var filePath in files)
    {
        string subDirectories = GetPascalCaseString(Path.GetDirectoryName(filePath.Replace(sourceDir + "\\", "")) ?? string.Empty);
        string outputDirectories = Path.Combine(outputDir, subDirectories);
        Directory.CreateDirectory(outputDirectories);

        string fileName = Path.GetFileNameWithoutExtension(filePath);
        string fileNamePascal = GetPascalCaseString(fileName);

        string outputPath = Path.Combine(outputDirectories, $"fa{fileNamePascal}.tsx");

        string svgContent = ReadSvgFile(filePath);

        WriteReactComponent(outputPath, svgContent);

        Console.WriteLine($"Converted '{filePath}' to Blazor component '{outputPath}'.");
    }
    stopwatch.Stop();

    Console.WriteLine($"Took {stopwatch.Elapsed.TotalSeconds}s");
    Console.WriteLine($"Converted {files.Count()} files to Blazor component. Output directory: '{outputDir}'.");
    
}

static string ReadSvgFile(string filePath)
{
    using StreamReader reader = new(filePath, Encoding.UTF8);

    HtmlDocument doc = new();
    doc.Load(reader);
    RemoveComments(doc.DocumentNode);

    using MemoryStream memoryStream = new ();
    doc.Save(memoryStream);
    memoryStream.Seek(0, SeekOrigin.Begin);

    using StreamReader sr = new(memoryStream);
    return sr.ReadToEnd();
}

static string GetPascalCaseString(string str)
{
    TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
    return string.Join("", str.Split("-").Select(textInfo.ToTitleCase)).Replace(" ", "");
}

static void RemoveComments(HtmlNode node)
{
    if (!node.HasChildNodes)
    {
        return;
    }

    for (int i = 0; i < node.ChildNodes.Count; i++)
    {
        if (node.ChildNodes[i].NodeType == HtmlNodeType.Comment)
        {
            node.ChildNodes.RemoveAt(i);
            --i;
        }
    }

    foreach (HtmlNode subNode in node.ChildNodes)
    {
        RemoveComments(subNode);
    }
}


static void WriteBlazorComponent(string outputPath, string svgContent)
{
    File.WriteAllText(outputPath, svgContent);
}

static void WriteReactComponent(string outputPath, string svgContent)
{
    string componentName = Path.GetFileNameWithoutExtension(outputPath);

    File.WriteAllText(outputPath, $$"""
        export type IconSize = "default" | "small" | "large";

        export interface {{ componentName }}Props {
            size: IconSize;
        }
        export default function {{componentName}}({}: {{componentName}}Props) {
            return (
                {{svgContent}}
            )
        }
        """);
}