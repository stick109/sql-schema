using CommandLine;
using SqlSchema.ConsoleHost.Options;

Parser.Default.ParseArguments<Extract, Compare>(args)
    .WithParsed<Extract>(options => options.Run())
    .WithParsed<Compare>(options => options.Run());
