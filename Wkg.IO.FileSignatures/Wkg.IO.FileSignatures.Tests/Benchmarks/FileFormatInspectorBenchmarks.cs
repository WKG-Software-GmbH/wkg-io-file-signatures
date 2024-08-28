﻿using BenchmarkDotNet.Attributes;

namespace Wkg.IO.FileSignatures.Tests.Benchmarks;

[MemoryDiagnoser]
public class FileFormatInspectorBenchmarks
{
    private readonly FileFormatInspector inspector;
    private static readonly string samplesPath;

    static FileFormatInspectorBenchmarks()
    {
        var buildDirectoryPath = Path.GetDirectoryName(typeof(FunctionalTests).Assembly.Location);
        samplesPath = Path.Combine(buildDirectoryPath, "Samples");
    }

    public FileFormatInspectorBenchmarks()
    {
        inspector = new FileFormatInspector();
    }

    [ParamsSource(nameof(SampleFiles))]
    public string FileName { get; set; }

    public static IEnumerable<string> SampleFiles()
    {
        var samplesDirectory = new DirectoryInfo(samplesPath);

        return samplesDirectory
            .GetFiles()
            //.Where(x => x.Extension == ".xls")
            .Select(x => x.Name)
            .ToList();
    }

    [Benchmark]
    public FileFormat DetermineFileFormat()
    {
        // We must open the stream as part of the benchmark because otherwise
        // Windows anti-malware will get rather upset with us.
        using var stream = File.OpenRead(Path.Combine(samplesPath, FileName));
        return inspector.DetermineFileFormat(stream);
    }
}
