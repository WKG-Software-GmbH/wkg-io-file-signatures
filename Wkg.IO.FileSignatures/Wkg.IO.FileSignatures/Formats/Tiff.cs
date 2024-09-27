namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a Tagged Image File Format (TIFF) image.
/// </summary>
public class Tiff() : Image(signature: [0x2A, 0x00], "image/tiff", "tif", offset: 2);