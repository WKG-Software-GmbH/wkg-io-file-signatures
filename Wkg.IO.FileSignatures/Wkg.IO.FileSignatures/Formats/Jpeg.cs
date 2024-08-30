namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a Joint Photographics Experts Group (JPEG) image.
/// </summary>
public class Jpeg : Image
{
    private const string MEDIA_TYPE = "image/jpeg";
    private const string EXTENSION = "jpg";

    private static ReadOnlySpan<byte> Soi => [0xFF, 0xD8];

    /// <summary>
    /// Initialises a new Jpeg format.
    /// </summary>
    public Jpeg() : base(Soi, MEDIA_TYPE, EXTENSION) => Pass();

    /// <summary>
    /// Initialises a new Jpeg format with the specified application marker.
    /// </summary>
    /// <param name="marker">The 2-byte application marker used by the JPEG format.</param>
    protected Jpeg(byte[] marker) : base([Soi[0], Soi[1], marker[0], marker[1]], MEDIA_TYPE, EXTENSION) => Pass();
}
