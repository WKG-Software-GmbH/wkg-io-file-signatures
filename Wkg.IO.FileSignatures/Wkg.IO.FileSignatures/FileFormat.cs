using System.Collections.Immutable;

namespace Wkg.IO.FileSignatures;

/// <summary>
/// Specifies the format of a file.
/// </summary>
public abstract class FileFormat : IEquatable<FileFormat>
{
    /// <summary>
    /// Gets a byte signature which can be used to identify the file format.
    /// </summary>
    public ImmutableArray<byte> Signature { get; }

    /// <summary>
    /// Gets the number of bytes required to determine the format.
    /// A value of <see cref="int.MaxValue"/> indicates that the entire file is required to determine the format.
    /// </summary>
    public int HeaderLength { get; }

    /// <summary>
    /// Gets the appropriate file extension for the format.
    /// </summary>
    public string Extension { get; }

    /// <summary>
    /// Gets the media type identifier for the format.
    /// </summary>
    public string MediaType { get; }

    /// <summary>
    /// Gets the offset in the file at which the signature is located.
    /// </summary>
    public int Offset { get; }

    /// <summary>
    /// Initializes a new instance of the FileFormat class which has the specified signature and media type.
    /// </summary>
    /// <param name="signature">The header signature of the format.</param>
    /// <param name="headerLength">The number of bytes required to determine the format.</param>
    /// <param name="mediaType">The media type of the format.</param>
    /// <param name="extension">The appropriate file extension for the format.</param>
    /// <param name="offset">The offset at which the signature is located.</param>
    protected FileFormat(ReadOnlySpan<byte> signature, int headerLength, string mediaType, string extension, int offset)
    {
        if (signature.IsEmpty)
        {
            throw new ArgumentException("Signature cannot be empty.", nameof(signature));
        }
        if (string.IsNullOrEmpty(mediaType))
        {
            throw new ArgumentNullException(nameof(mediaType));
        }

        Signature = [.. signature];
        HeaderLength = headerLength;
        Offset = offset;
        Extension = extension;
        MediaType = mediaType;
    }

    /// <summary>
    /// Initializes a new instance of the FileFormat class which has the specified signature and media type.
    /// </summary>
    /// <param name="signature">The header signature of the format.</param>
    /// <param name="mediaType">The media type of the format.</param>
    /// <param name="extension">The appropriate file extension for the format.</param>
    protected FileFormat(ReadOnlySpan<byte> signature, string mediaType, string extension)
        : this(signature, signature.Length, mediaType, extension, 0) => Pass();

    /// <summary>
    /// Initializes a new instance of the FileFormat class which has the specified signature and media type.
    /// </summary>
    /// <param name="signature">The header signature of the format.</param>
    /// <param name="mediaType">The media type of the format.</param>
    /// <param name="extension">The appropriate file extension for the format.</param>
    /// <param name="offset">The offset at which the signature is located.</param>
    protected FileFormat(ReadOnlySpan<byte> signature, string mediaType, string extension, int offset)
        : this(signature, signature.Length + offset, mediaType, extension, offset) => Pass();

    /// <summary>
    /// Initializes a new instance of the FileFormat class which has the specified signature and media type.
    /// </summary>
    /// <param name="signature">The header signature of the format.</param>
    /// <param name="headerLength">The number of bytes required to determine the format.</param>
    /// <param name="mediaType">The media type of the format.</param>
    /// <param name="extension">The appropriate file extension for the format.</param>
    protected FileFormat(ReadOnlySpan<byte> signature, int headerLength, string mediaType, string extension)
        : this(signature, headerLength, mediaType, extension, 0) => Pass();

    /// <summary>
    /// Returns a value indicating whether the format matches a file header.
    /// </summary>
    /// <param name="stream">The stream to check.</param>
    public virtual bool IsMatch(Stream stream)
    {
        if (stream == null || stream.Length < HeaderLength && HeaderLength < int.MaxValue || Offset > stream.Length)
        {
            return false;
        }

        stream.Position = Offset;

        for (int i = 0; i < Signature.Length; i++)
        {
            int b = stream.ReadByte();
            if (b != Signature[i])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Determines whether the object is equal to this FileFormat.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    public override bool Equals(object? obj) => Equals(obj as FileFormat);

    /// <summary>
    /// Determines whether the format is equal to this FileFormat.
    /// </summary>
    /// <param name="fileFormat">The format to compare.</param>
    public bool Equals(FileFormat? fileFormat)
    {
        if (fileFormat == null)
        {
            return false;
        }

        if (ReferenceEquals(this, fileFormat))
        {
            return true;
        }

        if (GetType() != fileFormat.GetType())
        {
            return false;
        }

        return fileFormat.Signature.SequenceEqual(Signature);
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            foreach (byte element in Signature)
            {
                hash = (hash * 31) + element.GetHashCode();
            }

            return hash;
        }
    }

    /// <summary>
    /// Returns a string that represents this format.
    /// </summary>
    public override string ToString() => MediaType;
}
