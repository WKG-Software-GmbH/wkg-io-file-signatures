namespace Wkg.IO.FileSignatures.Formats;

public class VisioLegacy : CompoundFileBinary
{
    public VisioLegacy() : base("VisioDocument", "application/vnd.visio", "vsd") => Pass();
}
