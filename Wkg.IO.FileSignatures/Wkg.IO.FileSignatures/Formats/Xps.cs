namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of an XML Paper Specification (XPS) document.
/// </summary>
public class Xps() : OfficeOpenXml("FixedDocSeq.fdseq", "application/vnd.ms-xpsdocument", "xps");