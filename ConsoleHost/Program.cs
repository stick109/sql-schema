using CommandLine;
using SqlSchema;
using SqlSchema.Options;

Parser.Default.ParseArguments<Extract, Compare>(args)
    .WithParsed<Extract>(Extractor.Run)
    .WithParsed<Compare>(Comparer.Run);
