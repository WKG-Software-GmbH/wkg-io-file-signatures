namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a Dicom file.
/// </summary>
public class Dicom : FileFormat
{
    public Dicom() : base("DICM"u8, "application/dicom", "dcm", 128) => Pass();
}
