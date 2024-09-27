namespace Wkg.IO.FileSignatures.Formats;

/// <summary>
/// Specifies the format of an Excel 97-2003 workbook.
/// </summary>
public class ExcelLegacy() : CompoundFileBinary("Workbook", "application/vnd.ms-excel", "xls");