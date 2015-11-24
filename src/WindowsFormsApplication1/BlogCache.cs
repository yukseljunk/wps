using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WordPressSharp;
using WordPressSharp.Models;

namespace WindowsFormsApplication1
{
    public class BlogCache
    {
        private Dictionary<string, HashSet<string>> _idsPresent = new Dictionary<string, HashSet<string>>();
        private Dictionary<string, HashSet<Term>> _tagsPresent = new Dictionary<string, HashSet<Term>>();

        private readonly WordPressSiteConfig _siteConfig;

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