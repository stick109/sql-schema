using CommandLine;

namespace SqlSchema.Options;

[Verb("compare", HelpText = "Compare schemas")]
public class Compare : Base
{
    [Option('s', Required = true, HelpText = "Source schema")]
    public string Source { get; set; } = null!;

    [Option('t', Required = true, HelpText = "Target schema")]
    public string Target { get; set; } = null!;
}