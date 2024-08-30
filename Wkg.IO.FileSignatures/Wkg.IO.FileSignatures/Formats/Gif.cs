﻿namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of Graphics Interchange Format (GIF) image.
/// </summary>
public class Gif : Image
{
    public Gif() : base("GIF8"u8, "image/gif", "gif") => Pass();
}