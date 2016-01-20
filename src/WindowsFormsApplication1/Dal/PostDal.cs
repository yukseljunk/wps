using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using PttLib.Helpers;
using PttLib.TourInfo;
using WordPressSharp.Models;

namespace WordpressScraper.Dal
{
    public class PostDal
    {
        private readonly Dal _dal;

        public PostDal(Dal dal)
        {
            _dal = dal;
        }

        public IList<Post> GetPosts(PostOrder order = PostOrder.NewestFirst, int limit=100, bool onlyDraft = true)
        {
            var sql = "SELECT P.*,U.display_name FROM wp_posts P INNER JOIN wp_users U ON P.post_author = U.ID ";
            if (onlyDraft)
            {
                sql += " where P.post_status='draft' ";
            }
            switch (order)
            {
                case PostOrder.Random:
                    sql += " order by RAND()";
                    break;
                case PostOrder.NewestFirst:
                    sql += " order by post_date DESC";
                    break;
                case PostOrder.OldestFirst:
                    sql += " order by post_date ASC";
                    break;

            }
            sql += " LIMIT 0,"+limit;

            var data = _dal.GetData(sql);
            if (data.Tables.Count == 0) { return null; }
            if (data.Tables[0].Rows.Count == 0) { return null; }
            var result = new List<Post>();
            foreach (DataRow row in data.Tables[0].Rows)
            {
                result.Add(
                    new Post()
                    {
                        Id = row["ID"].ToString(),
                        Title = row["post_title"].ToString(),
                        PublishDateTime = DateTime.Parse(row["post_date"].ToString(), new CultureInfo("en-US")),
                        Content = row["post_content"].ToString(),
                        Status = row["post_status"].ToString(),
                        Name = row["post_name"].ToString(),
                        Url = row["guid"].ToString(),
                        PostType = row["post_type"].ToString(),
                        Author = row["display_name"].ToString()
                    }
                );
            }
            return result;
        }

        public void PublishPost(Post post)
        {
            var sql = string.Format("Update wp_posts set post_status='publish',post_date=NOW() where ID={0}", post.Id);
            _dal.ExecuteNonQuery(sql);
        }

        public int InsertPost(Post post)
        {
            var customFieldSql = new StringBuilder();
            foreach (var customField in post.CustomFields)
            {
                customFieldSql.Append(
                    string.Format("INSERT INTO wp_postmeta( post_id, meta_key, meta_value) VALUES (@l,'{0}','{1}');",
                        customField.Key.EscapeSql(), customField.Value.EscapeSql()));

            }


            var tagsSql = new StringBuilder();
            if (post.Terms != null)
            {
                foreach (var term in post.Terms)
                {
                    tagsSql.Append(
                        string.Format(
                            "INSERT INTO wp_term_relationships(object_id, term_taxonomy_id, term_order) VALUES (@l,{0},0);",
                            term.Id));

                }
            }

            var imagesSql = new StringBuilder();
            foreach (var imageId in post.ImageIds)
            {
                imagesSql.Append(
                    string.Format("Update wp_posts set post_parent=@l where Id={0};", imageId));

            }

            var converterFunctions = new ConverterFunctions();
            var postName = converterFunctions.SeoUrl(post.Title);
            postName = "";

            var sql = string.Format(
                "INSERT INTO wp_posts(post_author, post_date, post_date_gmt, post_content, post_title, post_excerpt, post_status, comment_status, " +
                "ping_status, post_password, post_name, to_ping, pinged, post_modified, post_modified_gmt, post_content_filtered, post_parent, guid, " +
                "menu_order, post_type, post_mime_type, comment_count) VALUES " +
                "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}');" +
                "SET @l=LAST_INSERT_ID();" +
                "Update wp_posts set guid=concat('{22}?p=',@l) where Id=@l;" +
                "{23}{24}{25}SELECT @l;",
                post.Author, post.PublishDateTime.ToString("yyyy-MM-dd HH':'mm':'ss"), post.PublishDateTime.ToString("yyyy-MM-dd HH':'mm':'ss"), post.Content.EscapeSql(), post.Title.EscapeSql(), "", post.Status,
                post.CommentStatus, "open",
                "", postName.EscapeSql(), "", "", DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss"), DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss"), "", 0, "", 0,
                post.PostType, "", 0, post.BlogUrl.EscapeSql(), customFieldSql.ToString(), tagsSql.ToString(), imagesSql.ToString());

            var postInsertDataSet = _dal.GetData(sql);

            if (postInsertDataSet.Tables.Count == 0) { return -1; }
            if (postInsertDataSet.Tables[0].Rows.Count == 0) { return -1; }
            if (postInsertDataSet.Tables[0].Rows[0].ItemArray.Length == 0) { return -1; }

            var id = postInsertDataSet.Tables[0].Rows[0][0].ToString();
            return int.Parse(id);
        }

        public DataSet GetAllPostMeta()
        {
            var sql = "Select post_id,meta_value from wp_postmeta where meta_key='foreignkey'";
            return _dal.GetData(sql);

        }

        public DataSet GetAllTitles()
        {
            var sql = "Select post_title from wp_posts";
            return _dal.GetData(sql);
        }
    }
}