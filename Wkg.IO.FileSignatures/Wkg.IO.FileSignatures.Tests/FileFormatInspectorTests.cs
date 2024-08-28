﻿namespace Wkg.IO.FileSignatures.Tests;

[TestClass]
public class FileFormatInspectorTests
{
    [TestMethod]
    public void StreamCannotBeNull()
    {
        var inspector = new FileFormatInspector();

        Assert.ThrowsException<ArgumentNullException>(() => inspector.DetermineFileFormat(null));
    }

    [TestMethod]
    public void EmptyStreamReturnsNull()
    {
        var inspector = new FileFormatInspector();
        FileFormat? result;

        using (var stream = new MemoryStream())
        {
            result = inspector.DetermineFileFormat(stream);
        }

        Assert.IsNull(result);
    }

    [TestMethod]
    public void StreamMustBeSeekable()
    {
        var nonSeekableStream = new NonSeekableStream();
        var inspector = new FileFormatInspector(Array.Empty<FileFormat>());

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

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    [TestMethod]
    public void UnrecognisedReturnsNull()
    {
        var inspector = new FileFormatInspector(Array.Empty<FileFormat>());
        FileFormat result;

        using (var stream = new MemoryStream(new byte[] { 0x0A }))
        {
            result = inspector.DetermineFileFormat(stream);
        }

        Assert.IsNull(result);
    }

    [TestMethod]
    public void SingleMatchIsReturned()
    {
        var expected = new TestFileFormat(new byte[] { 0x42, 0x4D });
        var inspector = new FileFormatInspector(new FileFormat[] { expected });
        FileFormat result;

        using (var stream = new MemoryStream(new byte[] { 0x42, 0x4D, 0x3A, 0x00 }))
        {
            result = inspector.DetermineFileFormat(stream);
        }

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void StreamIsReadUntilRequiredBufferIsReceived()
    {
        var expected = new TestFileFormat(new byte[] { 0x00, 0x01 });
        var incorrect = new TestFileFormat(new byte[] { 0x00, 0x02 });
        var inspector = new FileFormatInspector(new FileFormat[] { expected, incorrect });
        FileFormat result;

        using (var fragmentedStream = new FragmentedStream(new byte[] { 0x00, 0x01, 0x03 }))
        {
            result = inspector.DetermineFileFormat(fragmentedStream);
        }

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void StreamIsResetToOriginalPosition()
    {
        var shortSignature = new TestFileFormat(new byte[] { 0x00, 0x01 });
        var longSignaure = new TestFileFormat(new byte[] { 0x00, 0x01, 0x02 });
        var inspector = new FileFormatInspector(new FileFormat[] { shortSignature, longSignaure });
        var position = 0L;

        using (var stream = new MemoryStream(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04 }))
        {
            inspector.DetermineFileFormat(stream);
            position = stream.Position;
        }

        Assert.AreEqual(0, position);
    }

    [TestMethod]
    public void MultipleMatchesReturnsMostDerivedFormat()
    {
        var baseFormat = new BaseFormat();
        var inheritedFormat = new InheritedFormat();
        var inspector = new FileFormatInspector(new FileFormat[] { inheritedFormat, baseFormat });
        FileFormat result = null;

        using (var stream = new MemoryStream(new byte[] { 0x00 }))
        {
            result = inspector.DetermineFileFormat(stream);
        }

        Assert.AreEqual(inheritedFormat, result);
    }

    [TestMethod]
    public void MutipleMatchesReturnsFormatWithLongestHeader()
    {
        var shortHeader = new TestFileFormat(new byte[] { 0x02, 0x00 });
        var longHeader = new AnotherTestFileFormat(new byte[] { 0x02, 0x00, 0xFF });

        var inspector = new FileFormatInspector(new FileFormat[] { shortHeader, longHeader });
        FileFormat result = null;

        using (var stream = new MemoryStream(new byte[] { 0x02, 0x00, 0xFF, 0xFA }))
        {
            result = inspector.DetermineFileFormat(stream);
        }

        Assert.IsNotNull(result);
        Assert.AreEqual(longHeader, result);
    }

    private class FragmentedStream : MemoryStream
    {
        public FragmentedStream(byte[] buffer) : base(buffer)
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return base.Read(buffer, offset, 1);
        }
    }

    private class TestFileFormat : FileFormat
    {
        public TestFileFormat(byte[] signature) : base(signature, "example/test", "test")
        {
        }
    }

    private class BaseFormat : FileFormat
    {
        public BaseFormat() : this("example/base")
        {
        }

        protected BaseFormat(string mediaType) : base(new byte[] { 0x00 }, mediaType, "")
        {
        }

        public override bool IsMatch(Stream stream)
        {
            return true;
        }
    }

    private class InheritedFormat : BaseFormat
    {
        public InheritedFormat() : base("example/inherited")
        {
        }
    }

    private class AnotherTestFileFormat : FileFormat
    {
        public AnotherTestFileFormat(byte[] signature) : base(signature, "example/another", "test")
        {
        }
    }
}
