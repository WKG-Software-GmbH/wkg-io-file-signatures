namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a MPEG-4 file
/// </summary>
/// <param name="signature">The concrete signature of the format.</param>
public abstract class MP4(ReadOnlySpan<byte> signature) : Isobmff(signature, "video/mp4", "mp4");