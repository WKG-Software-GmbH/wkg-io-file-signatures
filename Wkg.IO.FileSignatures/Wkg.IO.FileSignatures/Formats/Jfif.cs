namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a JPEG File Interchange Fomat (JFIF) file.
/// </summary>
public class JpegJfif() : Jpeg([0xFF, 0xE0]);