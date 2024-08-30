using System.Collections.Immutable;
using System.Diagnostics;

namespace Wkg.IO.FileSignatures;

/// <summary>
/// Provides a mechanism to determine the format of a file.
/// </summary>
/// <remarks>
/// Initialises a new FileFormatInspector instance which can determine the specified file formats.
/// </remarks>
/// <param name="formats">The formats which are recognised.</param>
public class FileFormatInspector(IEnumerable<FileFormat> formats) : IFileFormatInspector
{
    private readonly ImmutableArray<FileFormat> _formats = [.. formats.OrderBy(t => t.HeaderLength)];

    /// <summary>
    /// Initialises a new FileFormatInspector instance which can determine the default file formats.
    /// </summary>
    public FileFormatInspector() : this(FileFormatLocator.GetFormats()) => Pass();

    /// <summary>
    /// Determines the format of a file.
    /// </summary>
    /// <param name="stream">A stream containing the file content.</param>
    /// <returns>An instance of a matching file format, or null if the format could not be determined.</returns>
    public FileFormat? DetermineFileFormat(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (!stream.CanSeek)
        {
            throw new NotSupportedException("The passed stream object is not seekable.");
        }

        if (stream.Length == 0)
        {
            return null;
        }

        Span<bool> candidates = stackalloc bool[_formats.Length];
        int matches = FindMatchingFormats(stream, candidates);

        if (matches > 1)
        {
            RemoveBaseFormats(candidates, ref matches);
        }

        if (matches > 0)
        {
            for (int i = candidates.Length - 1; i >= 0; i--)
            {
                if (candidates[i])
                {
                    return _formats[i];
                }
            }
        }

        return null;
    }

    private int FindMatchingFormats(Stream stream, Span<bool> candidates)
    {
        Debug.Assert(candidates.Length == _formats.Length);
        int matches = 0;
        for (int i = 0; i < _formats.Length; i++)
        {
            if (_formats[i].IsMatch(stream))
            {
                candidates[i] = true;
                matches++;
            }
        }

        if (matches > 1)
        {
            for (int i = 0; i < _formats.Length; i++)
            {
                if (candidates[i] && _formats[i] is IFileFormatReader reader)
                {
                    using IDisposable? file = reader.Read(stream);
                    if (file is null || !reader.IsMatch(file))
                    {
                        candidates[i] = false;
                        matches--;
                    }
                }
            }
        }

        stream.Position = 0;
        return matches;
    }

    private void RemoveBaseFormats(Span<bool> candidates, ref int matches)
    {
        Debug.Assert(candidates.Length > 1);
        Debug.Assert(candidates.Length == _formats.Length);
        for (int i = 0; i < candidates.Length; i++)
        {
            if (!candidates[i])
            {
                continue;
            }
            for (int j = 0; j < candidates.Length; j++)
            {
                if (i != j && candidates[j] && _formats[j].GetType().IsAssignableFrom(_formats[i].GetType()))
                {
                    candidates[j] = false;
                    matches--;
                }
            }
        }
    }
}
