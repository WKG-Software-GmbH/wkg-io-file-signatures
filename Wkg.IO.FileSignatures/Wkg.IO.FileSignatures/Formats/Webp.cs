namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a Google WebP image file.
/// </summary>
public class Webp : Image
{
    private static ReadOnlySpan<byte> FileSignature => [0x57, 0x45, 0x42, 0x50];

    public Webp() : base(FileSignature, "image/webp", "webp", 8) => Pass();
}
