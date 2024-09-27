namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a Dicom file.
/// </summary>
public class Dicom() : FileFormat("DICM"u8, "application/dicom", "dcm", offset: 128);