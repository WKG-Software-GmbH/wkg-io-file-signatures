
namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a 3GPP2 Media (.3G2) compliant with 3GPP2 C.S0050-0/A/B video file
/// </summary>
public class ThreeG2() : Isobmff("3g2"u8, "video/3gpp2", "3g2")
{
    /// <inheritdoc/>
    public override bool IsMatch(Stream stream)
    {
        if (!base.IsMatch(stream))
        {
            return false;
        }
        int b = stream.ReadByte();
        return b is >= 'a' and <= 'c';
    }
}