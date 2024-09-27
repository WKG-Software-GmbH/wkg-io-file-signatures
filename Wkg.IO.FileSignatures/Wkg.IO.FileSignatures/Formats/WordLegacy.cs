namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a Word 97-2003 document.
/// </summary>
public class WordLegacy() : CompoundFileBinary("WordDocument", "application/msword", "doc");