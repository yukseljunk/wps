using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using CommandLine;
using PttLib;
using WpsLib.Dal;
using WpsLib.ProgramOptions;

namespace WpsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var options = new WpsConsoleCommandLineOptions();
                if (Parser.Default.ParseArguments(args, options))
                {

                    options.PostIdsInt = new List<int>();
                    if (options.PostIds != null)
                    {
                        foreach (var postId in options.PostIds)
                        {
                            int postIdConverted;
                            if (int.TryParse(postId, out postIdConverted))
                            {
                                options.PostIdsInt.Add(int.Parse(postId));
                            }
                        }
                    }

                    Publish(options);


                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }

        }

        private static void Publish(WpsConsoleCommandLineOptions options)
        {
            var programOptionsFactory = new ProgramOptionsFactory();
            var blogOptions = programOptionsFactory.Get(options.BlogConfigPath);
           // var postDal = new PostDal(new Dal(MySqlConnectionString));

        }
    }
}
