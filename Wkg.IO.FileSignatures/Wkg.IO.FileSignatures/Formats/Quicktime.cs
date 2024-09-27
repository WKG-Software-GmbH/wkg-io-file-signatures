namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a QuickTime movie file
/// </summary>
public class Quicktime() : Isobmff("qt  "u8, "video/quicktime", "mov");