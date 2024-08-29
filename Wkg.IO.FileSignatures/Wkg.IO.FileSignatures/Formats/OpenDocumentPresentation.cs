namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a OffsetFileFormat file.
/// </summary>
public class OpenDocumentPresentation : FileFormat
{
    public OpenDocumentPresentation() : base("mimetypeapplication/vnd.oasis.opendocument.presentation"u8, "application/vnd.oasis.opendocument.presentation", "odp", 30) => Pass();
}
