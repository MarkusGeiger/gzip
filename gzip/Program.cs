using CommandLine;
using Humanizer;
using System.Diagnostics;
using System.IO.Compression;

const string GzipExtension = ".gz";

Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(options => 
{
  var sw = new Stopwatch();

  foreach (var file in options.Source)
  {
    sw.Start();
    var sourceFile = new FileInfo(file);
    if (!sourceFile.Exists)
    {
      Console.WriteLine($"Source file '{sourceFile.FullName}' not found.");
      continue;
    }

    if (sourceFile.Extension == GzipExtension)
    {
      var targetFile = new FileInfo(sourceFile.FullName.Replace(sourceFile.Extension, String.Empty));
      DecompressFile(sourceFile, targetFile);
      sw.Stop();
      Console.WriteLine($"{sourceFile.Name}[{sourceFile.Length.Bytes()}] uncompressed to {targetFile.Name}[{targetFile.Length.Bytes()}] in {sw.Elapsed.TotalMilliseconds.Milliseconds()}");
    }
    else
    {
      var targetFile = new FileInfo(sourceFile.FullName + GzipExtension);
      CompressFile(sourceFile, targetFile);
      sw.Stop();
      Console.WriteLine($"{sourceFile.Name}[{sourceFile.Length.Bytes()}] compressed to {targetFile.Name}[{targetFile.Length.Bytes()}] in {sw.Elapsed.TotalMilliseconds.Milliseconds()}");
    }
  }
});

void CompressFile(FileInfo sourceFile, FileInfo targetFile)
{
  using (FileStream sourceStream = sourceFile.OpenRead())
  {
    using (FileStream targetStream = targetFile.OpenWrite())
    {
      using (GZipStream gZipStream = new GZipStream(targetStream, CompressionMode.Compress))
      {
        sourceStream.CopyTo(gZipStream);
      }
    }
  }
}

void DecompressFile(FileInfo sourceFile, FileInfo targetFile)
{
  using (FileStream sourceStream = sourceFile.OpenRead())
  {
    using (FileStream targetStream = targetFile.OpenWrite())
    {
      using (GZipStream gZipStream = new GZipStream(sourceStream, CompressionMode.Decompress))
      {
        gZipStream.CopyTo(targetStream);
      }
    }
  }
}

class CommandLineOptions
{
  [Option('s', "source", Required = true, HelpText = "Source file to be processed.")]
  public IEnumerable<string> Source { get; set; }
}