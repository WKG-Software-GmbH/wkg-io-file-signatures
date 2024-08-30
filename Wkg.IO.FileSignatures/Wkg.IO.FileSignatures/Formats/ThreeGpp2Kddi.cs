
namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a 3GPP2 EZmovie video file for KDDI 3G cellphones
/// </summary>
public class ThreeGpp2Kddi() : Isobmff("KDDI"u8, "video/3gpp2", "3g2");