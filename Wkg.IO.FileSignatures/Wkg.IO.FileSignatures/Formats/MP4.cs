namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a MPEG-4 video
/// </summary>
public class MP4 : Isobmff
{
    public MP4() : base("mp42"u8, "video/mp4", "mp4") => Pass();
}
