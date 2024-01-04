using CommandLine;

namespace SqlSchema.ConsoleHost.Options
{
    [Verb("cmp", HelpText = "Compare schemas")]
    public class Compare : Base
    {
        public new void Run()
        {
            base.Run();
        }
    }
}
