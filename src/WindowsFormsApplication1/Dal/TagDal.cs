using System.Data;

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
    }
}