using System.Collections.Immutable;

namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of an MPEG-4 (.MP4) Nero video
/// </summary>
public class MP4Nero() : MP4("ND"u8)
{
    // Nero video files have a lot of sub-formats, and we don't want to create specific handlers for each one.
    private static ImmutableArray<byte> Signatures { get; } =
    [
        .. "SC"u8,
        .. "SH"u8,
        .. "SM"u8,
        .. "SP"u8,
        .. "SS"u8,
        .. "XC"u8,
        .. "XH"u8,
        .. "XM"u8,
        .. "XP"u8,
        .. "XS"u8,
    ];

    /// <inheritdoc/>
    public override bool IsMatch(Stream stream)
    {
        if (!base.IsMatch(stream))
        {
            return false;
        }
        // continue reading the stream to check the sub-type
        Span<byte> bytes = stackalloc byte[2];
        int bytesRead = stream.Read(bytes);
        if (bytesRead != bytes.Length)
        {
            return false;
        }
        int index = Signatures.AsSpan().IndexOf(bytes);
        return (index & 1) == 0;
    }
}