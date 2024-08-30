﻿namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a Bitmap image file.
/// </summary>
public class Bmp : Image
{
    public Bmp() : base("BM"u8, "image/bmp", "bmp") => Pass();
}