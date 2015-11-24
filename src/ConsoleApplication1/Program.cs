using System;
using System.Collections.Generic;
using WordPressSharp;
using WordPressSharp.Models;

namespace ConsoleApplication1
{
    class Program
    {
        private const string DefaultUrl = "https://www.etsy.com/c/books-movies-and-music/music/instrument-straps";
        static void Main(string[] args)
        {

            Console.WriteLine("Enter url:, i.e. {0}", DefaultUrl);
            var url = Console.ReadLine();
            if (string.IsNullOrEmpty(url))
            {
                url = DefaultUrl;
            }

            var etsyResults = GetEtsyItems(url);
            foreach (var etsyResult in etsyResults)
            {
                var item = GetEtsyItem(etsyResult.Item1, etsyResult.Item2);
                Console.WriteLine(item);
               
            }
            //AddToGonBlog();
            Console.Read();

        }


        private static string GetEtsy()
        {
            var etsy = new Etsy();
            return etsy.Get("https://www.etsy.com/c/books-movies-and-music/music/instrument-straps");
        }
        private static IEnumerable<Tuple<string, string>> GetEtsyItems(string url)
        {
            var etsy = new Etsy();
            return etsy.GetItems(url);
        }
        private static Item GetEtsyItem(string title, string url)
        {
            var etsy = new Etsy();
            return etsy.GetItem(title, url);
        }

        private static void AddToGonBlog()
        {
            using (var client = new WordPressClient())
            {
                var post = new Post
                {
                    PostType = "post",
                    Title = "My Awesome Post",
                    Content = "<p>This is the content</p>",
                    PublishDateTime = DateTime.Now.AddDays(1),
                    Status = "Draft"

                };

                var t = new Term
                {
                    Name = "tag1" + DateTime.Now.ToString(),
                    Description = "term description",
                    Slug = "term_test",
                    Taxonomy = "post_tag"
                };

                var termId = client.NewTerm(t);
                t.Id = termId;


                var t2 = new Term
                {
                    Name = "cat1" + DateTime.Now.ToString(),
                    Description = "cat description",
                    Slug = "term_test",
                    Taxonomy = "category"
                };

                var termId2 = client.NewTerm(t2);
                t2.Id = termId2;


                post.Terms = new Term[] { t, t2 };

                var id = Convert.ToInt32(client.NewPost(post));
                Console.WriteLine(id);



            }

        }


    }
}
