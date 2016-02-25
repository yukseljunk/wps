using System;
using System.Collections.Generic;
using CommandLine;
using WordPressSharp.Models;
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
                var publisher = new Publisher();
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

                    publisher.Publish(options);


                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }

        }



    }

    class Publisher
    {
        private ProgramOptions _options;

        public void Publish(WpsConsoleCommandLineOptions options)
        {
            try
            {
                var programOptionsFactory = new ProgramOptionsFactory();
                _options = programOptionsFactory.Get(options.BlogConfigPath);
                var postDal = new PostDal(new Dal(MySqlConnectionString));

                PostOrder postOrder = PostOrder.NewestFirst;
                switch (options.Strategy)
                {
                    case "newest":
                        postOrder = PostOrder.NewestFirst;
                        break;
                    case "oldest":
                        postOrder = PostOrder.OldestFirst;
                        break;
                    case "random":
                        postOrder = PostOrder.Random;
                        break;
                    default:
                        break;
                }

                IList<Post> posts = new List<Post>();// postDal.GetPosts(postOrder, options.NumberToPublish);
                if (options.Strategy == "selected")
                {
                    posts = postDal.GetPosts(options.PostIdsInt);
                }
                else
                {
                    posts = postDal.GetPosts(postOrder, options.NumberToPublish);
                }
                if (posts == null)
                {
                    Console.WriteLine("No posts found to publish!");
                    return;
                }

                foreach (var post in posts)
                {
                    try
                    {
                        Console.WriteLine(string.Format("Publishing '{0}'", post.Title));
                        postDal.PublishPost(post);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.ToString());
                    }
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
            Console.WriteLine("Publishing done.");
        }
        private string MySqlConnectionString
        {
            get
            {
                return string.Format("Server={0};Database={1};Uid={2};Pwd={3}; Allow User Variables=True", _options.DatabaseUrl, _options.DatabaseName, _options.DatabaseUser, _options.DatabasePassword);
            }
        }

    }
}
