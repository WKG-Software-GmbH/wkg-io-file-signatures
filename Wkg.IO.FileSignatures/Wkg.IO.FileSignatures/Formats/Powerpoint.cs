namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of a Powerpoint presentation.
/// </summary>
public class PowerPoint() : OfficeOpenXml("ppt/presentation.xml", "application/vnd.openxmlformats-officedocument.presentationml.presentation", "pptx");