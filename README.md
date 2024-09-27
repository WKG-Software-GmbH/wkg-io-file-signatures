# WKG I/O File Signatures

[![NuGet](https://img.shields.io/badge/NuGet-555555?style=for-the-badge&logo=nuget)![NuGet version (Wkg.IO.FileSignatures)](https://img.shields.io/nuget/v/Wkg.IO.FileSignatures.svg?style=for-the-badge&label=Wkg.IO.FileSignatures)![NuGet downloads (Wkg.IO.FileSignatures)](https://img.shields.io/nuget/dt/Wkg.IO.FileSignatures?style=for-the-badge)](https://www.nuget.org/packages/Wkg.IO.FileSignatures/)

---

`Wkg.IO.FileSignatures` is an updated version of [Neil Harvey's FileSignatures library](https://github.com/neilharvey/FileSignatures) for file fingerprinting and signature detection based on known file header formats. A core goal of this library is to provide a simple and easy-to-use API for detecting file types based on their headers, as commonly required in file upload and processing scenarios.

As part of our commitment to open-source software, we are making this updated version of the library [available to the public](https://github.com/WKG-Software-GmbH/wkg-io-file-signatures/) under the GNU General Public License v3.0. We hope that it will be useful to other developers and that the community will contribute to its further development.

## Disclaimer

This project is a derivative of [Neil Harvey's FileSignatures library](https://github.com/neilharvey/FileSignatures), which was licensed under the MIT License. The modified version of this project is licensed under the GNU General Public License v3.0 (GPLv3).

- The original portions of the code are subject to the MIT License.
- All new contributions and modifications to the original code are licensed under the GPLv3, ensuring that improvements to this project are shared with the community.

Modifications to the original code include modernization of the codebase, performance improvements, and support for additional file signatures.

## Installation

Install the `Wkg.IO.FileSignatures` NuGet package by adding the following package reference to your project file:

```xml
<ItemGroup>
    <PackageReference Include="Wkg.IO.FileSignatures" Version="X.X.X" />
</ItemGroup>
```

> :warning: **Warning**
> Replace `X.X.X` with the latest stable version available on the [nuget feed](https://www.nuget.org/packages/Wkg.IO.FileSignatures), where **the major version must match the major version of your targeted .NET runtime**.

## Usage

The following sample provides a basic dependency injection setup to verify that uploaded files are valid video files:

```csharp
/// <summary>
/// Represents a service that validates files based on their header signature against a collection of supported file formats.
/// </summary>
public interface IFileValidationService
{
    /// <summary>
    /// Checks whether the file stream is valid in accordance with the implementing service.
    /// </summary>
    /// <param name="file">The file stream to validate</param>
    /// <returns><see langword="true"/> if the file is valid, otherwise <see langword="false"/></returns>
    bool IsValid(Stream file);

    /// <summary>
    /// The file formats supported by the implementing service.
    /// </summary>
    ImmutableArray<FileFormat> SupportedFileFormats { get; }
}

// the interface for your specific file validation service
public interface IVideoFileValidationService : IFileValidationService;

// the implementation of your specific file validation service
internal class VideoFileValidationService : IVideoFileValidationService
{
    private readonly FileFormatInspector _fileInspector;

    public ImmutableArray<FileFormat> SupportedFileFormats { get; }

    public FileValidationService()
    {
        FileFormat[] allowedFileFormats = [.. FileFormatRegistry.MimeTypeRegistry["video/*"].Formats];
        SupportedFileFormats = [.. allowedFileFormats];
        _fileInspector = new FileFormatInspector(allowedFileFormats);
    }

    public bool IsValid(Stream file)
    {
        FileFormat? format = _fileInspector.DetermineFileFormat(file);
        return format is not null;
    }
}

// register your service in the DI container as usual
services.AddSingleton<IVideoFileValidationService, VideoFileValidationService>();
```

> :information_source: **Note**
> `FileFormatInspector` requires the inspected stream to be seekable. To prevent having to load the entire stream into memory, you can use buffering techniques to ensure that the stream is seekable, e.g., as implemented by [`CachingStream` in the WKG Base library](https://github.com/WKG-Software-GmbH/wkg-base/blob/main/docs/documentation.md#cachingstream-class).

You can now inject `IVideoFileValidationService` into your controllers or services to validate uploaded files:

```csharp
public class UploadController(IVideoFileValidationService fileValidationService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> UploadFileAsync(IFormFile file)
    {
        await using Stream fileStream = file.OpenReadStream();
        // CachingStream from WKG Base library, otherwise use MemoryStream to buffer the stream into memory
        await using CachingStream bufferStream = new CachingStream(fileStream, leaveOpen: true);
        if (!fileValidationService.IsValid(bufferStream))
        {
            return BadRequest("Invalid file format");
        }
        // and if everything is fine write the buffer to disk
        await using Stream destination = File.OpenWrite("output");
        buffer.Seek(0, SeekOrigin.Begin);
        await buffer.CopyToAsync(destination);
        return Ok();
    }
}
```

You can specify the file formats supported by your service by selecting the appropriate file formats from the `FileFormatRegistry.MimeTypeRegistry` or by creating your own `FileFormat` instances directly.

```csharp
// allow video and image files by MIME type from the registry
FileFormat[] allowedFileFormats = 
[
    .. FileFormatRegistry.MimeTypeRegistry["video/*"].Formats, 
    .. FileFormatRegistry.MimeTypeRegistry["image/*"].Formats
];
// allow only certain video formats
[
    .. FileFormatRegistry.MimeTypeRegistry["video/mp4"].Formats,
    .. FileFormatRegistry.MimeTypeRegistry["video/3gpp"].Formats,
    .. FileFormatRegistry.MimeTypeRegistry["video/3gpp2"].Formats,
    new Mkv(),
    new WebM()
];
// or implement your own file formats:
public class Bash : FileFormat(signature: "#!/bin/bash"u8, "text/x-shellscript", "sh");
FileFormat[] allowedFileFormats = [new Bash()];
```