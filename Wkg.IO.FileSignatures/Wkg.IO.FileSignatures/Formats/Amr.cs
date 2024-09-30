namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of an Adaptive Multi-Rate ACELP (Algebraic Code Excited Linear Prediction) Codec file
/// Commonly audio format with GSM cell phones. (See RFC 4867.)
/// </summary>
public class Amr() : FileFormat("#!AMR"u8, "audio/amr", "amr");