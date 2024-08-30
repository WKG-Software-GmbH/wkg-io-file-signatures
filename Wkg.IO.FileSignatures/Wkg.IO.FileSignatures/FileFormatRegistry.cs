using System.Collections.Frozen;

namespace Wkg.IO.FileSignatures;

/// <summary>
/// Provides a registry of file formats.
/// </summary>
public static class FileFormatRegistry
{
    /// <summary>
    /// Gets a dictionary of file formats grouped by MIME type.
    /// </summary>
    public static FrozenDictionary<string, FileFormatGroup> MimeTypeRegistry { get; }

    static FileFormatRegistry()
    {
        IEnumerable<FileFormat> formats = FileFormatLocator.GetFormats();
        Dictionary<string, FileFormatGroup> mimeTypeRegistry = new
        ([
            .. formats.GroupBy(format => format.MediaType)
                .Select(group => new KeyValuePair<string, FileFormatGroup>(group.Key, new FileFormatGroup(group.Key, [.. group]))),
            .. formats.Select(format => (Index: format.MediaType.IndexOf('/'), Format: format))
                .Where(t => t.Index != -1)
                .GroupBy(t => t.Format.MediaType[.. t.Index])
                .Select(group => new KeyValuePair<string, FileFormatGroup>($"{group.Key}/*", new FileFormatGroup($"{group.Key}/*", [.. group.Select(t => t.Format)])))
        ]);
        MimeTypeRegistry = mimeTypeRegistry.ToFrozenDictionary();
    }
}