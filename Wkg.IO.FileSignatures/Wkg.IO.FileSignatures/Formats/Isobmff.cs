namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a ISO/IEC base media file format
/// </summary>
/// <see href="https://en.wikipedia.org/wiki/ISO/IEC_base_media_file_format"/>
public abstract class Isobmff : FileFormat
{
    private static ReadOnlySpan<byte> FileSignature => [0x66, 0x74, 0x79, 0x70];

    protected Isobmff(ReadOnlySpan<byte> signature, string mediaType, string extension)
        : base([..FileSignature, ..signature], mediaType, extension, offset: 4) => Pass();
}
