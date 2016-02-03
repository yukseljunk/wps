using System;
using System.IO;
using System.Linq;

namespace YoutubeUtilities
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new CommandLineOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                string[] tags = new[] { "" };
                if (options.Tags != null)
                {
                    tags = options.Tags.ToArray();
                }
                var ytUtilities = new YouTubeUtilities(options.RefreshToken, options.ClientSecret, options.ClientId);
                using (var fileStream = new FileStream(options.InputFile, FileMode.Open))
                {
                    Console.WriteLine(ytUtilities.UploadVideo(fileStream, options.Title, options.Description, tags, options.Category, options.Public));
                }
            }
        }
    }
}
