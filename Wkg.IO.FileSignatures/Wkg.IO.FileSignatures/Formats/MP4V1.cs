namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a MPEG-4 v1 file
/// </summary>
public class MP4V1 : Isobmff
{
    public MP4V1() : base("isom"u8, "video/mp4", "mp4") => Pass();
}
