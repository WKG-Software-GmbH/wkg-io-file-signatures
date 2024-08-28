using System.IO.Compression;
using Wkg.IO.FileSignatures.Formats;

namespace Wkg.IO.FileSignatures.Tests.Formats;

[TestClass]
public class OfficeOpenXmlTests
{
    [TestMethod]
    public void InvalidZipArchiveDoesNotThrow()
    {
        var inspector = new FileFormatInspector();

        using var stream = new MemoryStream(new byte[] { 0x50, 0x4B, 0x03, 0x04 });
        var format = inspector.DetermineFileFormat(stream);

        Assert.IsNotNull(format);
        Assert.IsInstanceOfType<Zip>(format);
    }

    [TestMethod]
    public void IdentifierWithoutExtensionDoesNotThrow()
    {
        var format = new TestOfficeOpenXml("test", "example/test", "test");

        using var stream = new MemoryStream();
        using (var createArchive = new ZipArchive(stream, ZipArchiveMode.Create, true))
        {
            createArchive.CreateEntry("test");
        }

        using var archive = new ZipArchive(stream, ZipArchiveMode.Read);

        var result = format.IsMatch(archive);

        Assert.IsTrue(result);
    }

    private class TestOfficeOpenXml : OfficeOpenXml
    {
        public TestOfficeOpenXml(string identifiableEntry, string mediaType, string extension) : base(identifiableEntry, mediaType, extension)
        {
        }
    }
}
