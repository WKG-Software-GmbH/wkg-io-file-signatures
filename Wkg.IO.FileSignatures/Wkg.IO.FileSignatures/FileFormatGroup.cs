using System.Collections.Immutable;

namespace Wkg.IO.FileSignatures;

/// <summary>
/// Represents a group of file formats with the same MIME type.
/// </summary>
/// <param name="MimeType">The MIME type of the group.</param>
/// <param name="Formats">The formats in the group.</param>
public readonly record struct FileFormatGroup(string MimeType, ImmutableArray<FileFormat> Formats);