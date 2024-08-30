namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a 3rd Generation Partnership Project 3GPP multimedia files (3GG, 3GP, 3G2)
/// </summary>
public class ThreeGPP : Isobmff
{
    public ThreeGPP() : base("3gp"u8, "video/3gpp", "3gp") => Pass();
}
