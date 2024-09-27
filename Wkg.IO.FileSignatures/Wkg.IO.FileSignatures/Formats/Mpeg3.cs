namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a MPEG-1 Audio Layer 3 (MP3) audio file
/// </summary>
public class Mpeg3() : FileFormat("ID3"u8, "audio/mpeg", "mp3");