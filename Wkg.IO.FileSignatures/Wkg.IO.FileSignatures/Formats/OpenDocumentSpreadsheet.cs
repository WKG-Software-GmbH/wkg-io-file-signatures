namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a OffsetFileFormat file.
/// </summary>
public class OpenDocumentSpreadsheet : FileFormat
{
    public OpenDocumentSpreadsheet() : base("mimetypeapplication/vnd.oasis.opendocument.spreadsheet"u8, "application/vnd.oasis.opendocument.spreadsheet", "ods", 30) => Pass();
}
