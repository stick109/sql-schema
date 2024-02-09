using CommandLine;

namespace SqlSchema.Options;

[Verb("compare", HelpText = "Compare schemas.")]
public class Compare : Base
{
    [Option('s', Required = true, HelpText = "Source schema.")]
    public string Source { get; set; } = null!;

    [Option('t', Required = true, HelpText = "Target schema.")]
    public string Target { get; set; } = null!;

    [Option('d', Default = false, HelpText = "Show all properties of missing tables, including columns and indexes.")]
    public bool DetailedOutput { get; set; }
}