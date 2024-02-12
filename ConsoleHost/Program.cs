using System.Reflection;
using CommandLine;
using SqlSchema;
using SqlSchema.Options;

[assembly: AssemblyVersion("1.1.*")]
[assembly: AssemblyTitle("Sql Schema Comparer")]
[assembly: AssemblyCopyright("Copyright (c) 2024 Konstantin Surkov")]

Parser.Default.ParseArguments<Extract, Compare>(args)
    .WithParsed<Extract>(Extractor.Run)
    .WithParsed<Compare>(Comparer.Run);
