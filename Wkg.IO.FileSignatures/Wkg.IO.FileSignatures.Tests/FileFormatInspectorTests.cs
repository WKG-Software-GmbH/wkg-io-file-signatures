using System.ComponentModel;

namespace Wkg.IO.FileSignatures.Tests;

[TestClass]
public class FileFormatInspectorTests
{
    [TestMethod]
    public void StreamCannotBeNull()
    {
        FileFormatInspector inspector = new();

        Assert.ThrowsException<ArgumentNullException>(() => inspector.DetermineFileFormat(null!));
    }

    [TestMethod]
    public void EmptyStreamReturnsNull()
    {
        FileFormatInspector inspector = new();
        FileFormat? result;

        using (MemoryStream stream = new())
        {
            result = inspector.DetermineFileFormat(stream);
        }

        Assert.IsNull(result);
    }

    [TestMethod]
    public void StreamMustBeSeekable()
    {
        NonSeekableStream nonSeekableStream = new();
        FileFormatInspector inspector = new([]);

        Assert.ThrowsException<NotSupportedException>(() => inspector.DetermineFileFormat(nonSeekableStream));
    }

    private class NonSeekableStream : Stream
    {
        public override bool CanSeek => false;

        #region Not relevant for tests
        public override bool CanRead => throw new NotImplementedException();

        public override bool CanWrite => throw new NotImplementedException();

        public override long Length => throw new NotImplementedException();

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Flush() => throw new NotImplementedException();

        public override int Read(byte[] buffer, int offset, int count) => throw new NotImplementedException();

        public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

        public override void SetLength(long value) => throw new NotImplementedException();

        public override void Write(byte[] buffer, int offset, int count) => throw new NotImplementedException();

        #endregion
    }

    [TestMethod]
    public void UnrecognisedReturnsNull()
    {
        FileFormatInspector inspector = new([]);
        FileFormat? result;

        using (MemoryStream stream = new([0x0A]))
        {
            result = inspector.DetermineFileFormat(stream);
        }

        Assert.IsNull(result);
    }

    [TestMethod]
    public void SingleMatchIsReturned()
    {
        TestFileFormat expected = new("BM"u8);
        FileFormatInspector inspector = new([expected]);
        FileFormat? result;

        using (MemoryStream stream = new([0x42, 0x4D, 0x3A, 0x00]))
        {
            result = inspector.DetermineFileFormat(stream);
        }

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void StreamIsReadUntilRequiredBufferIsReceived()
    {
        TestFileFormat expected = new([0x00, 0x01]);
        TestFileFormat incorrect = new([0x00, 0x02]);
        FileFormatInspector inspector = new([expected, incorrect]);
        FileFormat? result;

        using (FragmentedStream fragmentedStream = new([0x00, 0x01, 0x03]))
        {
            result = inspector.DetermineFileFormat(fragmentedStream);
        }

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void StreamIsResetToOriginalPosition()
    {
        TestFileFormat shortSignature = new([0x00, 0x01]);
        TestFileFormat longSignaure = new([0x00, 0x01, 0x02]);
        FileFormatInspector inspector = new([shortSignature, longSignaure]);
        long position = 0L;

        using (MemoryStream stream = new([0x00, 0x01, 0x02, 0x03, 0x04]))
        {
            inspector.DetermineFileFormat(stream);
            position = stream.Position;
        }

        Assert.AreEqual(0, position);
    }

    [TestMethod]
    public void MultipleMatchesReturnsMostDerivedFormat()
    {
        BaseFormat baseFormat = new();
        InheritedFormat inheritedFormat = new();
        FileFormatInspector inspector = new([inheritedFormat, baseFormat]);
        FileFormat? result;

        using (MemoryStream stream = new([0x00]))
        {
            result = inspector.DetermineFileFormat(stream);
        }

        Assert.AreEqual(inheritedFormat, result);
    }

    [TestMethod]
    public void MutipleMatchesReturnsFormatWithLongestHeader()
    {
        TestFileFormat shortHeader = new([0x02, 0x00]);
        AnotherTestFileFormat longHeader = new([0x02, 0x00, 0xFF]);

        FileFormatInspector inspector = new([shortHeader, longHeader]);
        FileFormat? result;

        using (MemoryStream stream = new([0x02, 0x00, 0xFF, 0xFA]))
        {
            result = inspector.DetermineFileFormat(stream);
        }

        Assert.IsNotNull(result);
        Assert.AreEqual(longHeader, result);
    }

    private class FragmentedStream : MemoryStream
    {
        public FragmentedStream(byte[] buffer) : base(buffer) => Pass();

        public override int Read(byte[] buffer, int offset, int count)
        {
            return base.Read(buffer, offset, 1);
        }
    }

    private class TestFileFormat(ReadOnlySpan<byte> signature) : FileFormat(signature, "example/test", "test");

    private class BaseFormat : FileFormat
    {
        public BaseFormat() : this("example/base") => Pass();

        protected BaseFormat(string mediaType) : base([0x00], mediaType, "") => Pass();

        public override bool IsMatch(Stream stream) => true;
    }

    private class InheritedFormat() : BaseFormat("example/inherited");

    private class AnotherTestFileFormat(byte[] signature) : FileFormat(signature, "example/another", "test");
}
