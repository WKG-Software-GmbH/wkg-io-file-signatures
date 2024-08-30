namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a Google WebP image file.
/// </summary>
public class WebP : Image
{
    private static ReadOnlySpan<byte> FileSignature => [0x57, 0x45, 0x42, 0x50];

    public WebP() : base(FileSignature, "image/webp", "webp", 8) => Pass();
}
