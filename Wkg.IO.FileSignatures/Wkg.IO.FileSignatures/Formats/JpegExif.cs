namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a JPEG image containing EXIF data.
/// </summary>
public class JpegExif : Jpeg
{
    public JpegExif() : base([0xFF, 0xE1]) => Pass();
}
