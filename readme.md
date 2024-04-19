# SvgToBlazorComponent
### Convert SVG files to Blazor components with ease

This project provides a command-line tool `(SvgToBlazorComponent.exe)` that automates the conversion of SVG files into corresponding Blazor component Razor files (.razor).

Installation
There are two primary methods for installing `SvgToBlazorComponent`:

### 1. Pre-built executable (recommended):

- Download the latest release from the GitHub Releases section.
- Place the downloaded `SvgToBlazorComponent.exe` file in a directory accessible from your command prompt or terminal.

### 2. From source (for developers):

- Clone this repository using Git: `git clone https://github.com/uzairali001/svg-to-blazor-component.git`
- Navigate to the project directory: `cd svg-to-blazor-component\src`
- Build the project using a suitable C# compiler (e.g., `dotnet build`).
- The `SvgToBlazorComponent.exe` file will be located in the `bin/Debug` or `bin/Release` folder (depending on your build configuration).

### Usage
Run the `SvgToBlazorComponent.exe` tool with the following syntax:

```
SvgToBlazorComponent.exe <source directory> <destination directory>
```
- `<source directory>`: The path to the directory containing your SVG files. The tool will search this directory and its subdirectories recursively for any files with the .svg extension.
- `<destination directory>`: The path to the directory where you want to create the generated Blazor component Razor files.

### Help Option

To get a quick overview of the tool's usage, you can either pass `"help"` as the first argument or run the tool without any arguments:

```
SvgToBlazorComponent.exe help  # OR
SvgToBlazorComponent.exe
```
This will display the following usage message:

```
Usage: SvgToBlazorComponent.exe <source directory> <destination directory>
```

### Example
```
SvgToBlazorComponent.exe C:\MyProject\images C:\MyProject\BlazorComponents
```
This command will search the `C:\MyProject\images` directory (including subdirectories) for all `.svg` files and generate corresponding Blazor component Razor files in the `C:\MyProject\BlazorComponents` directory.

Generated Files
For each SVG file found, the tool will create a Razor file with the same name (excluding the `.svg` extension) in the specified destination directory. The Razor file will contain the SVG code encapsulated within an HTML `<svg>` element, making it ready to be used as a reusable Blazor component.