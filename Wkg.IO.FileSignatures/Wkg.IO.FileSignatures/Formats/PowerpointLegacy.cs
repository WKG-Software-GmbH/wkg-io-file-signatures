namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a legacy Powerpoint 97-2003 presentation.
/// </summary>
public class PowerPointLegacy() : CompoundFileBinary("PowerPoint Document", "application/vnd.ms-powerpoint", "ppt");