using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace WpsConsole
{
    class WpsConsoleCommandLineOptions
    {
        [Option('b', "blogConfigPath", Required = true,
            HelpText = "Config File Path to Blog to publish")]
        public string BlogConfigPath { get; set; }

        [Option('n', "number", HelpText = "Number of items to publish when strategy is not 'selected items' ")]
        public int NumberToPublish { get; set; }

        [Option('s', "strategy", HelpText = "Strategy to publish: newest,oldest,random or selected")]
        public string Strategy { get; set; }

        [OptionList('p', "posts", HelpText = "Post Ids to publish when strategy is 'selected items'", Separator = ',')]
        public IList<string> PostIds { get; set; }

        public IList<int> PostIdsInt { get; set; }
       
        public override string ToString()
        {
            return string.Format("BlogConfig: {0}, # of items: {1}, strategy: {2}", BlogConfigPath, NumberToPublish, Strategy);
        }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
                (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}