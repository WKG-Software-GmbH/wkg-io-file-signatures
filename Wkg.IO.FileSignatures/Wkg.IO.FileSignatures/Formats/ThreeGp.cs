namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a 3rd Generation Partnership Project 3GPP multimedia files (3GG, 3GP, 3G2)
/// </summary>
public abstract class ThreeGp(ReadOnlySpan<byte> signature) : Isobmff([.. "3g"u8, .. signature], "video/3gpp", "3gp");