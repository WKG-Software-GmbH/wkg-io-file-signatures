
namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a 3GPP Media (.3GP) Release 1-7 video
/// </summary>
public class ThreeGp3gp() : ThreeGp("p"u8)
{
    /// <inheritdoc/>
    public override bool IsMatch(Stream stream)
    {
        if (!base.IsMatch(stream))
        {
            return false;
        }
        int b = stream.ReadByte();
        return b is >= '1' and <= '7';
    }
}