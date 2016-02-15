using System;
using System.IO;
using System.Linq;

namespace YoutubeUtilities
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var options = new CommandLineOptions();
                if (CommandLine.Parser.Default.ParseArguments(args, options))
                {
                    string[] tags = new[] {""};
                    if (options.Tags != null)
                    {
                        tags = options.Tags.ToArray();
                    }
                    var ytUtilities = new YouTubeUtilities(options.RefreshToken, options.ClientSecret, options.ClientId);
                    using (var fileStream = new FileStream(options.InputFile, FileMode.Open))
                    {
                        var videoId = ytUtilities.UploadVideo(fileStream, options.Title, options.Description, tags,
                            options.Category, options.Public);
                        if (videoId == "FAILED")
                        {
                            Console.WriteLine("Video upload failed!");
                        }
                        else
                        {
                            Console.WriteLine("Video uploaded with Id: " + videoId);

                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }
    }
}
