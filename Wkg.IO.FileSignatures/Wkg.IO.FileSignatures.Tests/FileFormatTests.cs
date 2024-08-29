namespace Wkg.IO.FileSignatures.Tests;

[TestClass]
public class FileFormatTests
{
    [TestMethod]
    public void SignatureCannotBeEmpty()
    {
        // TODO: change to cannot be empty
        Assert.ThrowsException<ArgumentException>(() => new ConcreteFileFormat([], "example/bad", "bad"));
        Assert.ThrowsException<ArgumentException>(() => new ConcreteFileFormat(null!, "example/bad", "bad"));
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void MediaTypeCannotBeNullOrEmpty(string badMediaType)
    {
        Assert.ThrowsException<ArgumentNullException>(() => new ConcreteFileFormat([0x01], badMediaType, "bad"));
    }

    [TestMethod]
    public void EqualityIsBasedOnSignature()
    {
        ConcreteFileFormat first = new([0x01], "example/one", "1");
        ConcreteFileFormat second = new([0x01], "example/two", "2");

        Assert.AreEqual(first, second);
    }

    [TestMethod]
    public void GetHashCodeIsBasedOnSignature()
    {
        ConcreteFileFormat first = new([0x01], "example/one", "1");
        ConcreteFileFormat second = new([0x01], "example/two", "2");

        Assert.AreEqual(first.GetHashCode(), second.GetHashCode());
    }

    [TestMethod]
    public void MatchesHeaderContainingSignature()
    {
        ConcreteFileFormat format = new([0x6F, 0x3C], "example/sim", "");
        byte[] header = [0x6F, 0x3c, 0xFF, 0xFA];

        using MemoryStream ms = new(header);
        bool result = format.IsMatch(ms);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void MatchesSignatureAtOffsetPosition()
    {
        OffsetFileFormat format = new([0x03, 0x04], "example/test", "", 2);
        byte[] header = [0x01, 0x02, 0x03, 0x04];

        using MemoryStream ms = new(header);
        bool result = format.IsMatch(ms);

        Assert.IsTrue(result);
    }

    private class OffsetFileFormat(byte[] signature, string mediatType, string extension, int offset) : FileFormat(signature, mediatType, extension, offset);

    [DataTestMethod]
    [DataRow(new byte[] { 0x6F })]
    [DataRow(new byte[] { 0x3C, 0x6F })]
    public void DoesNotMatchDifferentHeader(byte[] header)
    {
        ConcreteFileFormat format = new([0x6F, 0x3C], "example/sim", "");

        using MemoryStream ms = new(header);
        bool result = format.IsMatch(ms);

        Assert.IsFalse(result);
    }

    private class ConcreteFileFormat(ReadOnlySpan<byte> signature, string mediaType, string extension) : FileFormat(signature, mediaType, extension);
}
