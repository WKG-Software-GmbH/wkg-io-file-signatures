namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a ISO/IEC base media file format
/// See <see href="https://en.wikipedia.org/wiki/ISO_base_media_file_format"/> and <see href="https://ftyps.com/"/>.
/// </summary>
public abstract class Isobmff(ReadOnlySpan<byte> signature, string mediaType, string extension) : FileFormat([..FileSignature, ..signature], mediaType, extension, offset: 4)
{
    private static ReadOnlySpan<byte> FileSignature => "ftyp"u8;
}
