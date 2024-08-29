namespace Wkg.IO.FileSignatures.Tests;

[TestClass]
public class FunctionalTests
{
    [DataTestMethod]
    [DataRow("test.bmp", "image/bmp")]
    [DataRow("test.doc", "application/msword")]
    [DataRow("test.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
    [DataRow("test.exe", "application/vnd.microsoft.portable-executable")]
    [DataRow("test.gif", "image/gif")]
    [DataRow("test.jfif", "image/jpeg")]
    [DataRow("test.exif", "image/jpeg")]
    [DataRow("saved.msg", "application/vnd.ms-outlook")]
    [DataRow("dragndrop.msg", "application/vnd.ms-outlook")]
    [DataRow("nonstandard.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
    [DataRow("test.pdf", "application/pdf")]
    [DataRow("test_header_somewhere_in_1024_first_bytes.pdf", "application/pdf")]
    [DataRow("test_header_adobe.pdf", "application/pdf")]
    [DataRow("test.rtf", "application/rtf")]
    [DataRow("test.png", "image/png")]
    [DataRow("test.ppt", "application/vnd.ms-powerpoint")]
    [DataRow("test2.ppt", "application/vnd.ms-powerpoint")]
    [DataRow("test.pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation")]
    [DataRow("test.spiff", "image/jpeg")]
    [DataRow("test.tif", "image/tiff")]
    [DataRow("test.xls", "application/vnd.ms-excel")]
    [DataRow("test.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    [DataRow("test.xps", "application/vnd.ms-xpsdocument")]
    [DataRow("test.zip", "application/zip")]
    [DataRow("test.dcm", "application/dicom")]
    [DataRow("test.odt", "application/vnd.oasis.opendocument.text")]
    [DataRow("test.ods", "application/vnd.oasis.opendocument.spreadsheet")]
    [DataRow("test.odp", "application/vnd.oasis.opendocument.presentation")]
    [DataRow("test.vsd", "application/vnd.visio")]
    [DataRow("test.vsdx", "application/vnd.visio")]
    [DataRow("test.webp", "image/webp")]
    [DataRow("test.mp4", "video/mp4")]
    [DataRow("test-v1.mp4", "video/mp4")]
    [DataRow("test.m4v", "video/mp4")]
    [DataRow("test.m4a", "audio/mp4")]
    [DataRow("test.mov", "video/quicktime")]
    [DataRow("test.3gp", "video/3gpp")]
    [DataRow("test.vcf", "text/vcard")]
    [DataRow("test.mp3", "audio/mpeg")]
    [DataRow("test.ogg", "audio/ogg")]
    [DataRow("test.amr", "audio/amr")]
    [DataRow("test.ico", "image/vnd.microsoft.icon")]
    public void SamplesAreRecognised(string sample, string expected)
    {
        FileFormat? result = InspectSample(sample);

        Assert.IsNotNull(result);
        Assert.AreEqual(expected, result?.MediaType);
    }

    private static FileFormat? InspectSample(string fileName)
    {
        FileFormatInspector inspector = new();
        string? buildDirectoryPath = Path.GetDirectoryName(typeof(FunctionalTests).Assembly.Location);
        FileInfo sample = new(Path.Combine(buildDirectoryPath ?? string.Empty, "Samples", fileName));
        FileFormat? result;

        using (FileStream stream = sample.OpenRead())
        {
            result = inspector.DetermineFileFormat(stream);
        }

        return result;
    }
}
