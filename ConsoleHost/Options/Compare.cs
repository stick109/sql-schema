using CommandLine;

namespace SqlSchema.ConsoleHost.Options;

[Verb("cmp", HelpText = "Compare schemas")]
public class Compare : Base
{
    [Option('s', Required = true, HelpText = "Source schema")]
    public string Source { get; set; } = null!;

    [Option('t', Required = true, HelpText = "Target schema")]
    public string Target { get; set; } = null!;

    public new void Run()
    {
        base.Run();
        SchemaComparer.Run(Source, Target);
    }
}