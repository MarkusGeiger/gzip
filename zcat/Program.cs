using CommandLine;

Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(o =>
{
  
});

class CommandLineOptions
{
  [Option('s', "source", Required = true, HelpText = "File(s) to process.")]
  public IEnumerable<string> Source { get; set; }
}