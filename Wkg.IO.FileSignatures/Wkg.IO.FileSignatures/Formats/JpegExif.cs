namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a JPEG image containing EXIF data.
/// </summary>
public class JpegExif() : Jpeg([0xFF, 0xE1]);