namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a Apple Lossless Audio Codec file
/// </summary>
public class M4A() : Isobmff("M4A "u8, "audio/mp4", "m4a");