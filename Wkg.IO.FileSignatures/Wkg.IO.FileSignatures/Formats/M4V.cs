namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of an MPEG-4 video file
/// </summary>
public class M4V : Isobmff
{
    public M4V() : base("M4V "u8, "video/mp4", "m4v") => Pass();
}
