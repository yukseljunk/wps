using PttLib.Helpers;
using PttLib.TourInfo;
using WordPressSharp.Models;

namespace WordpressScraper.Dal
{
    public class CategoryDal
    {
        private const string DEFAULT_CATEGORY = "DEFAULT_CATEGORY";
        private readonly Dal _dal;

        public CategoryDal(Dal dal)
        {
            _dal = dal;
        }

        public int GetInsertDefaultCategoryId()
        {
            var defaultCategoryId = GetDefaultCategoryId();
            if (defaultCategoryId > 0) return defaultCategoryId;

            return InsertCategory(new Term(){Name = "Default Category", Description = DEFAULT_CATEGORY});
        }

        public int GetDefaultCategoryId()
        {
            var sql = "SELECT term_id FROM wp_term_taxonomy where taxonomy='category' and description='" + DEFAULT_CATEGORY + "'";
            var data = _dal.GetData(sql);
            if (data.Tables.Count == 0) { return 0; }
            if (data.Tables[0].Rows.Count == 0) { return 0; }
            return int.Parse(data.Tables[0].Rows[0][0].ToString());
        }

        public int InsertCategory(Term term)
        {
            var tagInsertDataSet = _dal.GetData(InsertSql(term));

            if (tagInsertDataSet.Tables.Count == 0) { return -1; }
            if (tagInsertDataSet.Tables[0].Rows.Count == 0) { return -1; }
            if (tagInsertDataSet.Tables[0].Rows[0].ItemArray.Length == 0) { return -1; }

            var id = tagInsertDataSet.Tables[0].Rows[0][0].ToString();
            return int.Parse(id);
        }

        public string InsertSql(Term term)
        {
            var converterFunctions = new ConverterFunctions();

            return string.Format(
                "INSERT INTO wp_terms(name, slug, term_group) VALUES" +
                "('{0}','{1}',0);" +
                "SET @l=LAST_INSERT_ID();" +
                "INSERT INTO wp_term_taxonomy(term_id, taxonomy, description, parent, count) VALUES (@l,'category','{2}',0,0);" +
                "SELECT @l;",
                term.Name.EscapeSql(),
                converterFunctions.SeoUrl(term.Name).EscapeSql(),
                term.Description.EscapeSql());
        }
    }
}