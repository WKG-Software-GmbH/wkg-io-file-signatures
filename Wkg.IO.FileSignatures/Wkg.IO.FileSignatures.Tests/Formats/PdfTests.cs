using Wkg.IO.FileSignatures.Formats;

namespace Wkg.IO.FileSignatures.Tests.Formats;

[TestClass]
public class PdfTests
{
    private const int MAX_FILE_HEADER_SIZE = 1024;

    private static ReadOnlySpan<byte> PdfHeader => "%PDF"u8;

    public static IEnumerable<object[]> MatchHeaderAtAnyPlaceInFileHeaderData =>
    [
        [Enumerable.Range(0, 42).Select(i => (byte)i).ToArray()],
        [Array.Empty<byte>()],
        [Enumerable.Range(0, MAX_FILE_HEADER_SIZE - PdfHeader.Length).Select(i => (byte)i).ToArray()],
    ];

    [DataTestMethod]
    [DynamicData(nameof(MatchHeaderAtAnyPlaceInFileHeaderData))]
    public void MatchPdfHeaderAtAnyPlaceInFileHeader(byte[] data)
    {
        Pdf format = new();
        byte[] fileHeader = [..data, ..PdfHeader];

        using MemoryStream ms = new(fileHeader);
        bool result = format.IsMatch(ms);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void MatchAdobePdfHeaderInFileHeader()
    {
        AdobePdf format = new();
        byte[] fileHeader =
        [
            ..Enumerable
                .Range(0, 42)
                .Select(i => (byte)i),
            .."%!PS-Adobe-1.3 PDF-1.5"u8
        ];

        using MemoryStream ms = new(fileHeader);
        bool result = format.IsMatch(ms);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void DoesNotMatchPdfHeaderOutsideFileHeader()
    {
        Pdf format = new();
        byte[] fileHeader =
        [
            .. Enumerable
                .Range(0, MAX_FILE_HEADER_SIZE)
                .Select(i => (byte)i),
            .. PdfHeader
        ];

        using MemoryStream ms = new(fileHeader);
        bool result = format.IsMatch(ms);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void DoesNotMatchContentSmallerThanHeader()
    {
        Pdf format = new();
        byte[] header = [0x25, 0x50, 0x44];

        using MemoryStream ms = new(header);
        bool result = format.IsMatch(ms);

        Assert.IsFalse(result);
    }

    [DataTestMethod]
    [DataRow(new byte[] { 0x25, 0x51, 0x50, 0x44, 0x46 })]
    [DataRow(new byte[] { 0x25, 0x50, 0x44, 0x64 })]
    public void DoesNotMatchDifferentHeader(byte[] header)
    {
        Pdf format = new();

        using MemoryStream ms = new(header);
        bool result = format.IsMatch(ms);

        Assert.IsFalse(result);
    }
}