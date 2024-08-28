namespace Wkg.IO.FileSignatures.Tests;

[TestClass]
public class FileFormatTests
{
    [TestMethod]
    public void SignatureCannotBeNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new ConcreteFileFormat(null, "example/bad", "bad"));
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void MediaTypeCannotBeNullOrEmpty(string badMediaType)
    {
        Assert.ThrowsException<ArgumentNullException>(() => new ConcreteFileFormat(new byte[] { 0x01 }, badMediaType, "bad"));
    }

    [TestMethod]
    public void EqualityIsBasedOnSignature()
    {
        var first = new ConcreteFileFormat(new byte[] { 0x01 }, "example/one", "1");
        var second = new ConcreteFileFormat(new byte[] { 0x01 }, "example/two", "2");

        Assert.AreEqual(first, second);
    }

    [TestMethod]
    public void GetHashCodeIsBasedOnSignature()
    {
        var first = new ConcreteFileFormat(new byte[] { 0x01 }, "example/one", "1");
        var second = new ConcreteFileFormat(new byte[] { 0x01 }, "example/two", "2");

        Assert.AreEqual(first.GetHashCode(), second.GetHashCode());
    }

    [TestMethod]
    public void MatchesHeaderContainingSignature()
    {
        var format = new ConcreteFileFormat(new byte[] { 0x6F, 0x3C }, "example/sim", "");
        var header = new byte[] { 0x6F, 0x3c, 0xFF, 0xFA };

        using var ms = new MemoryStream(header);
        var result = format.IsMatch(ms);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void MatchesSignatureAtOffsetPosition()
    {
        var format = new OffsetFileFormat(new byte[] { 0x03, 0x04 }, "example/test", "", 2);
        var header = new byte[] { 0x01, 0x02, 0x03, 0x04 };

        using var ms = new MemoryStream(header);
        var result = format.IsMatch(ms);

        Assert.IsTrue(result);
    }

    private class OffsetFileFormat : FileFormat
    {
        public OffsetFileFormat(byte[] signature, string mediatType, string extension, int offset)
            : base(signature, mediatType, extension, offset)
        {
        }
    }

    [DataTestMethod]
    [DataRow(new byte[] { 0x6F })]
    [DataRow(new byte[] { 0x3C, 0x6F })]
    public void DoesNotMatchDifferentHeader(byte[] header)
    {
        var format = new ConcreteFileFormat(new byte[] { 0x6F, 0x3C }, "example/sim", "");

        using var ms = new MemoryStream(header);
        var result = format.IsMatch(ms);

        Assert.IsFalse(result);
    }

    private class ConcreteFileFormat : FileFormat
    {
        public ConcreteFileFormat(byte[] signature, string mediaType, string extension) : base(signature, mediaType, extension)
        {
        }
    }
}
