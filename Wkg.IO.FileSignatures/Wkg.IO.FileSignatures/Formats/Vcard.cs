namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a Contact vCard file
/// </summary>
public class Vcard : FileFormat
{
    // 0x0D, 0x0A (\r\n) combinations are omitted because the text writer can use any line ending style
    public Vcard() : base("BEGIN:VCARD"u8, "text/vcard", "vcf") => Pass();
}
