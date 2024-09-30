namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a Windows executable file
/// </summary>
public class Executable() : FileFormat("MZ"u8, "application/vnd.microsoft.portable-executable", "exe");