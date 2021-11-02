using CommandLine;
using Humanizer;
using System.Diagnostics;
using System.IO.Compression;

Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(options => 
{
  var sw = new Stopwatch();
  sw.Start();

  var sourceFile = new FileInfo(options.Source);
  if (!sourceFile.Exists) 
  { 
    Console.WriteLine($"Source file '{sourceFile.FullName}' not found."); 
    Environment.Exit(1);
  }

  var targetFile = new FileInfo(sourceFile.FullName + ".gz");
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

  sw.Stop();
  Console.WriteLine($"{sourceFile.Name}[{sourceFile.Length.Bytes()}] compressed to {targetFile.Name}[{targetFile.Length.Bytes()}] in {sw.Elapsed.TotalMilliseconds.Milliseconds()}");
});





class CommandLineOptions
{
  [Option('s', "source", Required = true, HelpText = "Source file to be compressed.")]
  public string Source { get; set; }
}