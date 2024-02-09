using CommandLine;

namespace SqlSchema.Options
{
    public class Base
    {
        [Option('v', Default = false, HelpText = "Verbose.")]
        public bool Verbose { get; set; }

        [Option("debug", Default = false, HelpText = "Even more verbose.")]
        public bool Debug { get; set; }

        public void Print()
        {
            if (Verbose)
            {
                var cmdLine = Parser.Default.FormatCommandLine(this);
                Console.Error.WriteLine(cmdLine);
            }
        }
    }
}
