namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a Apple Lossless Audio Codec file
/// </summary>
public class M4A : Isobmff
{
    public M4A() : base("M4A "u8, "audio/mp4", "m4a") => Pass();
}
