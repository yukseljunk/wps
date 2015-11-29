using System;
using System.Collections.Generic;
using System.Data;
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
        public int InsertPost(Post post)
        {
            var converterFunctions = new ConverterFunctions();
            var postName = converterFunctions.SeoUrl(post.Title);
            postName = "";

            var sql = string.Format(
                "INSERT INTO wp_posts(post_author, post_date, post_date_gmt, post_content, post_title, post_excerpt, post_status, comment_status, " +
                "ping_status, post_password, post_name, to_ping, pinged, post_modified, post_modified_gmt, post_content_filtered, post_parent, guid, " +
                "menu_order, post_type, post_mime_type, comment_count) VALUES " +
                "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}');"+
                "SET @l=LAST_INSERT_ID();"+
                "Update wp_posts set guid=concat('{22}?p=',@l) where Id=@l;"+
                "SELECT @l;",
                post.Author, post.PublishDateTime.AddDays(-1).ToString("yyyy-MM-dd HH':'mm':'ss"), post.PublishDateTime.AddDays(-1).ToString("yyyy-MM-dd HH':'mm':'ss"), post.Content, post.Title, "", post.Status,
                post.CommentStatus, "open",
                "", postName, "", "", DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss"), DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss"), "", 0, "", 0,
                post.PostType, post.MimeType, 0,post.BlogUrl);

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

    }
}