namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a Google WebP image file.
/// </summary>
public class WebP() : Image(FileSignature, "image/webp", "webp", offset: 8)
{
    private static ReadOnlySpan<byte> FileSignature => [0x57, 0x45, 0x42, 0x50];
}
