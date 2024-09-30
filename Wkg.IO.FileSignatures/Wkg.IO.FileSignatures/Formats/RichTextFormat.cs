namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a Rich Text Format (RTF) file.
/// </summary>
public class RichTextFormat() : FileFormat("{\\rtf1"u8, "application/rtf", "rtf");