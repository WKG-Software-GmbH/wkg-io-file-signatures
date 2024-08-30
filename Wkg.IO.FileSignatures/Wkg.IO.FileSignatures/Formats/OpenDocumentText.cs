﻿namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a OpenDocumentText file.
/// </summary>
public class OpenDocumentText : FileFormat
{
    public OpenDocumentText() : base("mimetypeapplication/vnd.oasis.opendocument.text"u8, "application/vnd.oasis.opendocument.text", "odt", 30) => Pass();
}
