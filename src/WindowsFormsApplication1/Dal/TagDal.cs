using System;
using System.Data;
using PttLib.Helpers;
using PttLib.TourInfo;
using WordPressSharp.Models;

namespace WordpressScraper.Dal
{
    public class TagDal
    {
        private readonly Dal _dal;

        public TagDal(Dal dal)
        {
            _dal = dal;
        }

        public DataSet GetAllTags()
        {

            var sql =
                "SELECT wpt.name, wpt.slug, wpt.term_group, wpt.term_id, wptt.count, wptt.description, wptt.parent, wptt.taxonomy, wptt.term_taxonomy_id " +
                "FROM  wp_terms wpt " +
                "INNER JOIN wp_term_taxonomy wptt ON wptt.term_id = wpt.term_id " +
                "WHERE taxonomy='post_tag'";

            return _dal.GetData(sql);
        }

        public int InsertTag(Term term)
        {
            var converterFunctions = new ConverterFunctions();
            
            var sql = string.Format(
                "INSERT INTO wp_terms(name, slug, term_group) VALUES" +
                "('{0}','{1}',0);" +
                "SET @l=LAST_INSERT_ID();" +
                "INSERT INTO wp_term_taxonomy(term_id, taxonomy, description, parent, count) VALUES (@l,'post_tag','{2}',0,0);" +
                "SELECT @l;",
                term.Name.EscapeSql(),
                converterFunctions.SeoUrl(term.Name).EscapeSql(),
                term.Description.EscapeSql());

            var tagInsertDataSet = _dal.GetData(sql);

            if (tagInsertDataSet.Tables.Count == 0) { return -1; }
            if (tagInsertDataSet.Tables[0].Rows.Count == 0) { return -1; }
            if (tagInsertDataSet.Tables[0].Rows[0].ItemArray.Length == 0) { return -1; }

            var id = tagInsertDataSet.Tables[0].Rows[0][0].ToString();
            return int.Parse(id);
        }
    }
}