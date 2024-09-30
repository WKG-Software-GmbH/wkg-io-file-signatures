namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a Contact vCard file
/// </summary>
// 0x0D, 0x0A (\r\n) combinations are omitted because the text writer can use any line ending style
public class Vcard() : FileFormat("BEGIN:VCARD"u8, "text/vcard", "vcf");