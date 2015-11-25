using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WordPressSharp;
using WordPressSharp.Models;
using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication1
{
    public class BlogCache
    {
        private Dictionary<string, HashSet<string>> _idsPresent = new Dictionary<string, HashSet<string>>();
        private Dictionary<string, HashSet<Term>> _tagsPresent = new Dictionary<string, HashSet<Term>>();

        private readonly WordPressSiteConfig _siteConfig;


        static string mysqlConnStr = "Server=198.46.81.196;Database=nalgor5_wpgonbl;Uid=nalgor5_wpgonbl;Pwd=S]P-a588C3";
        static void MySql()
        {
            Console.WriteLine("Trying to connect to mysql....");
            Console.WriteLine("You should specify your IP on Remote Database Access Hosts on mysql server, cpanel>Remote database access hosts>add an access host>your ip");
            using (var connection = new MySqlConnection(mysqlConnStr))
            {
                connection.Open();
                try
                {
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = "Select * from wp_postmeta where meta_key='foreignkey'";
                    var adapter = new MySqlDataAdapter(cmd);
                    var dataset = new DataSet();
                    adapter.Fill(dataset);

                    var count = dataset.Tables[0].Rows.Count;
                    foreach (DataRow row in dataset.Tables[0].Rows)
                    {
                        Console.WriteLine(row["meta_value"]);
                    }
                    Console.WriteLine(count);

                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }

            }

            Console.ReadLine();
        }

        public BlogCache(WordPressSiteConfig siteConfig)
        {
            _siteConfig = siteConfig;
        }

        public void Start(string blogUrl)
        {
            IdsPresent(blogUrl);
            TagsPresent(blogUrl);
        }

        public void InsertId(string blogUrl, string id)
        {
            _idsPresent[blogUrl].Add(id);
        }

        public void InsertTag(string blogUrl, Term tag)
        {
            _tagsPresent[blogUrl].Add(tag);
        }

        public HashSet<string> IdsPresent(string blogUrl)
        {
            if (!_idsPresent.ContainsKey(blogUrl))
            {
                _idsPresent.Add(blogUrl, null);
            }
            if (_idsPresent[blogUrl] == null)
            {
                _idsPresent[blogUrl] = GetPostIds();
            }
            return _idsPresent[blogUrl];
        }

        public HashSet<Term> TagsPresent(string blogUrl)
        {
            if (!_tagsPresent.ContainsKey(blogUrl))
            {
                _tagsPresent.Add(blogUrl, null);
            }
            if (_tagsPresent[blogUrl] == null)
            {
                _tagsPresent[blogUrl] = GetTags();
            }
            return _tagsPresent[blogUrl];
        }

        private HashSet<Term> GetTags()
        {
            var result = new HashSet<Term>();
            using (var client = new WordPressClient(_siteConfig))
            {
                var tags = client.GetTerms("post_tag", null);
                foreach (var tag in tags)
                {
                    result.Add(tag);
                }

            }
            return result;
        }

        private HashSet<string> GetPostIds()
        {
            var blockSize = 10;
            var result = new HashSet<string>();
            using (var client = new WordPressClient(_siteConfig))
            {
                for (var i = 0; i < 1000; i++)
                {
                    var posts =
                        client.GetPosts(new PostFilter() { Number = blockSize, Offset = blockSize * (i - 1) });
                  
                    foreach (var post in posts)
                    {
                        var foreignKeyCustomField =
                            post.CustomFields.FirstOrDefault(cf => cf.Key == "foreignkey");
                        if (foreignKeyCustomField != null)
                        {
                            result.Add(foreignKeyCustomField.Value);
                        }

                    }
                    if (!posts.Any())
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }
            }
            return result;
        }

    }
}