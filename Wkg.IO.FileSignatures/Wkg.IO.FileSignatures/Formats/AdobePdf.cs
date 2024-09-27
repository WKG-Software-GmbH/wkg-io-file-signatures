namespace Wkg.IO.FileSignatures.Formats;

public class AdobePdf() : Pdf(
[
    0x25, 0x21, 0x50, 0x53, 0x2D, 0x41, 0x64, 0x6F, 0x62, 0x65, 0x2D, VERSION_NUMBER_PLACEHOLDER, 0x2E,
    VERSION_NUMBER_PLACEHOLDER, 0x20, 0x50, 0x44,
    0x46, 0x2D, VERSION_NUMBER_PLACEHOLDER, 0x2E, VERSION_NUMBER_PLACEHOLDER
])
{
    private const byte VERSION_NUMBER_PLACEHOLDER = 0x00;

    /// <inheritdoc/>
    protected override bool CompareFileByteToSignatureAt(byte fileByte, int signatureIndex)
    {
        return base.CompareFileByteToSignatureAt(fileByte, signatureIndex) || IsVersionNumber(fileByte, Signature[signatureIndex]);
    }

    private static bool IsVersionNumber(byte fileByte, byte signatureByte)
    {
        return signatureByte == VERSION_NUMBER_PLACEHOLDER && IsNumber(fileByte);
    }

    private static bool IsNumber(byte b) => b is >= 0x30 and <= 0x39;
}