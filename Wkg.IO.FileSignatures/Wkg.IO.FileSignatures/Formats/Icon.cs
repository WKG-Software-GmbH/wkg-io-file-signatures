namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of an icon file.
/// See <see href="https://www.iana.org/assignments/media-types/image/vnd.microsoft.icon"/>
/// </summary>
public class Icon() : Image([0x00, 0x00, 0x01, 0x00], "image/vnd.microsoft.icon", "ico");