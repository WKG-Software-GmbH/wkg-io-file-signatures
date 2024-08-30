
namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a 3GPP (.3GP) Release 6/7 MBMS Extended Presentations video
/// </summary>
public class ThreeGp3ge() : ThreeGp("e"u8)
{
    /// <inheritdoc/>
    public override bool IsMatch(Stream stream)
    {
        if (!base.IsMatch(stream))
        {
            return false;
        }
        int b = stream.ReadByte();
        return b is '6' or '7';
    }
}