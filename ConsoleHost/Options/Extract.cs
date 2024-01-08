using CommandLine;

namespace SqlSchema.ConsoleHost.Options;

[Verb("extract", true, HelpText = "Extract schema from database")]
public class Extract: Base
{
    [Option('s', Required = true, HelpText = "Server address")]
    public string Server { get; set; } = null!;

    [Option('d', Required = true, HelpText = "Database name")]
    public string Database { get; set; } = null!;

    public new void Run()
    {
        base.Run();
        SchemaExtractor.Run(Server, Database, Verbose, Debug);
    }
}