namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a Portable Document Format (PDF) file.
/// </summary>
public class Pdf : FileFormat
{
    private const uint MAX_FILE_HEADER_SIZE = 1024;

    public Pdf() : this([0x25, 0x50, 0x44, 0x46]) => Pass();

    protected Pdf(ReadOnlySpan<byte> signature) : base(signature, "application/pdf", "pdf", 0) => Pass();

    public override bool IsMatch(Stream stream)
    {
        if (stream == null || stream.Length < HeaderLength)
        {
            return false;
        }

        stream.Position = 0;
        int signatureValidationIndex = 0;
        int fileByte;

        while (stream.Position < MAX_FILE_HEADER_SIZE && (fileByte = stream.ReadByte()) != -1)
        {
            if (CompareFileByteToSignatureAt((byte)fileByte, signatureValidationIndex))
            {
                signatureValidationIndex++;
            }
            else
            {
                signatureValidationIndex = 0;
            }

            if (signatureValidationIndex == Signature.Length)
            {
                return true;
            }
        }

        return false;
    }

    protected virtual bool CompareFileByteToSignatureAt(byte fileByte, int signatureIndex)
    {
        return fileByte == Signature[signatureIndex];
    }
}
