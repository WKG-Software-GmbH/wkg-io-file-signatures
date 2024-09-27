using System.IO.Compression;
using Wkg.IO.FileSignatures.Formats;

namespace Wkg.IO.FileSignatures.Tests.Formats;

[TestClass]
public class OfficeOpenXmlTests
{
    [TestMethod]
    public void InvalidZipArchiveDoesNotThrow()
    {
        FileFormatInspector inspector = new();

        using MemoryStream stream = new([0x50, 0x4B, 0x03, 0x04]);
        FileFormat? format = inspector.DetermineFileFormat(stream);

        Assert.IsNotNull(format);
        Assert.IsInstanceOfType<Zip>(format);
    }

    [TestMethod]
    public void IdentifierWithoutExtensionDoesNotThrow()
    {
        TestOfficeOpenXml format = new("test", "example/test", "test");

        using MemoryStream stream = new();
        using (ZipArchive createArchive = new(stream, ZipArchiveMode.Create, true))
        {
            createArchive.CreateEntry("test");
        }

        using ZipArchive archive = new(stream, ZipArchiveMode.Read);

        bool result = format.IsMatch(archive);

        Assert.IsTrue(result);
    }

    private class TestOfficeOpenXml(string identifiableEntry, string mediaType, string extension) 
        : OfficeOpenXml(identifiableEntry, mediaType, extension);
}
