using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        private readonly Dal _dal;

        public BlogCache(WordPressSiteConfig siteConfig, Dal dal)
        {
            _siteConfig = siteConfig;
            _dal = dal;
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
            var allTags = _dal.GetData("SELECT wpt.name, wpt.slug, wpt.term_group, wpt.term_id, wptt.count, wptt.description, wptt.parent, wptt.taxonomy, wptt.term_taxonomy_id " +
                    "FROM  wp_terms wpt " +
                    "INNER JOIN wp_term_taxonomy wptt ON wptt.term_id = wpt.term_id " +
                    "WHERE taxonomy='post_tag'");
            if (allTags.Tables.Count == 0) return result;
            if (allTags.Tables[0].Rows.Count == 0) return result;
            foreach (DataRow row in allTags.Tables[0].Rows)
            {
                result.Add(new Term()
                               {
                                   Description = row["description"].ToString(),
                                   Name = row["name"].ToString(),
                                   Id = row["term_id"].ToString(),
                                   Count = int.Parse(row["count"].ToString()),
                                   Parent = row["parent"].ToString(),
                                   Slug = row["slug"].ToString(),
                                   Taxonomy = row["taxonomy"].ToString(),
                                   TermGroup = row["term_group"].ToString(),
                                   TermTaxonomyId = row["term_taxonomy_id"].ToString()
                               });
            }
            return result;
        }

        private HashSet<string> GetPostIds()
        {
            var result = new HashSet<string>();

            var allMeta = _dal.GetData("Select post_id,meta_value from wp_postmeta where meta_key='foreignkey'");
            if (allMeta.Tables.Count == 0) return result;
            if (allMeta.Tables[0].Rows.Count == 0) return result;
            foreach (DataRow row in allMeta.Tables[0].Rows)
            {
                result.Add(row["meta_value"].ToString());
            }

            return result;
        }

    }
}