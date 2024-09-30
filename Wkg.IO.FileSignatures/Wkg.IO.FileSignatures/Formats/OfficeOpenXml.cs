using System.IO.Compression;

namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of an Office Open XML file.
/// </summary>
public abstract class OfficeOpenXml : Zip, IFileFormatReader
{
    /// <summary>
    /// Initializes a new instance of the OfficeOpenXmlFormat class which matches an archive containing a unique entry.
    /// </summary>
    /// <param name="identifiableEntry">The entry in the archive which is used to identify the format.</param>
    /// <param name="mediaType">The media type of the format.</param>
    /// <param name="extension">The appropriate extension for the format.</param>
    protected OfficeOpenXml(string identifiableEntry, string mediaType, string extension) : base(int.MaxValue, mediaType, extension)
    {
        if (string.IsNullOrEmpty(identifiableEntry))
        {
            throw new ArgumentNullException(nameof(identifiableEntry));
        }

        IdentifiableEntry = identifiableEntry;
    }

    /// <summary>
    /// Gets the entry in the file which can be used to identify the format.
    /// </summary>
    public string IdentifiableEntry { get; }

    /// <inheritdoc/>
    public bool IsMatch(IDisposable? file)
    {
        if (file is ZipArchive archive)
        {
            // Match archives which contain a non-standard version of the identifiable entry, e.g. document2.xml instead of document.xml.
            int index = Math.Max(0, IdentifiableEntry.LastIndexOf('.'));
            ReadOnlySpan<char> fileName = IdentifiableEntry.AsSpan()[..^index];
            ReadOnlySpan<char> extension = IdentifiableEntry.AsSpan()[index..];
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                ReadOnlySpan<char> entryName = entry.FullName.AsSpan();
                if (entryName.StartsWith(fileName, StringComparison.OrdinalIgnoreCase) &&
                    entryName.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    public IDisposable? Read(Stream stream)
    {
        try
        {
            return new ZipArchive(stream, ZipArchiveMode.Read, true);
        }
        catch (InvalidDataException)
        {
            return null;
        }
    }
}
