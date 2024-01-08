using CommandLine;

namespace SqlSchema.Options
{
    public class Base
    {
        [Option('v', Default = false, HelpText = "Verbose")]
        public bool Verbose { get; set; }

        [Option("debug", Default = false, HelpText = "Verbose")]
        public bool Debug { get; set; }

        public void Run()
        {
            if (Verbose)
            {
                var cmdLine = Parser.Default.FormatCommandLine(this);
                Console.Error.WriteLine(cmdLine);
            }
        }
    }
}
