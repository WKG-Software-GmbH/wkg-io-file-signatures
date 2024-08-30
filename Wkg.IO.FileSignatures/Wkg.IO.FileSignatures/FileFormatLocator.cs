using System.Reflection;

namespace Wkg.IO.FileSignatures;

public static class FileFormatLocator
{
    /// <summary>
    /// Returns all the default file formats.
    /// </summary>
    public static IEnumerable<FileFormat> GetFormats()
    {
        return GetFormats(typeof(FileFormatLocator).Assembly);
    }

    /// <summary>
    /// Returns all the concrete <see cref="FileFormat"/> types found in the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly which contains the file format definitions.</param>
    public static IEnumerable<FileFormat> GetFormats(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        return assembly.GetTypes()
             .Where(t => typeof(FileFormat).IsAssignableFrom(t))
             .Where(t => !t.GetTypeInfo().IsAbstract)
             .Where(t => t.GetConstructors().Any(c => c.GetParameters().Length == 0))
             .Select(Activator.CreateInstance)
             .OfType<FileFormat>();
    }

    /// <summary>
    /// Returns all the concrete <see cref="FileFormat"/> types found in the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly which contains the file format definitions.</param>
    /// <param name="includeDefaults">Whether to include the default format definitions with the results from the external assembly.</param>
    public static IEnumerable<FileFormat> GetFormats(Assembly assembly, bool includeDefaults)
    {
        IEnumerable<FileFormat> formatsInAssembly = GetFormats(assembly);

        if (!includeDefaults)
        {
            return formatsInAssembly;
        }
        else
        {
            IEnumerable<FileFormat> formatsThisAssembly = GetFormats();
            return formatsInAssembly.Union(formatsThisAssembly);
        }
    }

    public static IEnumerable<TFileFormat> SubTypesOf<TFileFormat>(Assembly? assembly = null) where TFileFormat : FileFormat
    {
        assembly ??= typeof(FileFormatLocator).Assembly;
        return assembly.GetTypes()
             .Where(t => t.IsAssignableTo(typeof(TFileFormat)) && !t.IsAbstract && t.GetConstructors().Any(c => c.GetParameters().Length is 0))
             .Select(Activator.CreateInstance)
             .Cast<TFileFormat>();
    }
}
