namespace Wkg.IO.FileSignatures.Formats;

public class Visio : OfficeOpenXml
{
    public Visio() : base("visio/document.xml", "application/vnd.visio", "vsdx") => Pass();
}
