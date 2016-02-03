using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace YoutubeUtilities
{

    class CommandLineOptions
    {
        [Option('f', "file", Required = true,
            HelpText = "Input file to be processed.")]
        public string InputFile { get; set; }

        [Option('r', "rtoken", Required = true,
            HelpText = "Youtube API Refresh token")]
        public string RefreshToken { get; set; }

        [Option('s', "secret", Required = true,
            HelpText = "Youtube API Client Secret")]
        public string ClientSecret { get; set; }

        [Option('i', "id", Required = true,
            HelpText = "Youtube API Client Id")]
        public string ClientId { get; set; }

        [Option('t', "title", DefaultValue = "My video title",
            HelpText = "Title of the video")]
        public string Title { get; set; }

        [Option('d', "desc", DefaultValue = "My video description",
            HelpText = "Description of the video")]
        public string Description { get; set; }

        [OptionList('a', "tags", HelpText = "Tags for the video", Separator = ',')]
        public IList<string> Tags { get; set; }

        [Option('c', "cat", DefaultValue = "22",
            HelpText = "Category of the video")]
        public string Category { get; set; }

        [Option('p', "public", DefaultValue = true,
            HelpText = "publicity of the video")]
        public bool Public { get; set; }

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