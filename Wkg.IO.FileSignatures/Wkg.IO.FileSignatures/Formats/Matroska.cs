
using System.Collections.Immutable;

namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a Matroska file (mkv, webm).
/// </summary>
public abstract class Matroska(ReadOnlySpan<byte> docTypeSignature, string mediaType, string extension) : FileFormat(MatroskaSignature, mediaType, extension)
{
    private readonly ImmutableArray<byte> _docTypeSignature = [.. docTypeSignature];

    private static ReadOnlySpan<byte> MatroskaSignature => [0x1A, 0x45, 0xDF, 0xA3];

    /// <inheritdoc />
    public override bool IsMatch(Stream stream)
    {
        if (!base.IsMatch(stream))
        {
            return false;
        }

        stream.Position = 0x1F;
        for (int i = 0; i < _docTypeSignature.Length; i++)
        {
            if (stream.ReadByte() != _docTypeSignature[i])
            {
                return false;
            }
        }
        return true;
    }
}