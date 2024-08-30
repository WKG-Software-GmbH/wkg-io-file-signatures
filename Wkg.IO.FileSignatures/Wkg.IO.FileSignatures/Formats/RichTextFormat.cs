namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a Rich Text Format (RTF) file.
/// </summary>
public class RichTextFormat : FileFormat
{
    public RichTextFormat() : base("{\\rtf1"u8, "application/rtf", "rtf") => Pass();
}
