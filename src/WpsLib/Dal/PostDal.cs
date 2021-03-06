using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.SqlServer.Server;
using PttLib.Helpers;
using PttLib.TourInfo;
using WordPressSharp.Models;

namespace WpsLib.Dal
{
    public class PostDal
    {
        private readonly Dal _dal;

        public PostDal(Dal dal)
        {
            _dal = dal;
        }

        public void DeleteAll()
        {
            var sql = "DELETE FROM wp_postmeta";
            _dal.ExecuteNonQuery(sql);

            sql = "DELETE FROM wp_term_relationships";
            _dal.ExecuteNonQuery(sql);

            sql = "DELETE FROM wp_posts";
            _dal.ExecuteNonQuery(sql);


        }

        public Post GetPost(int id)
        {
            var sql = "SELECT P.*,U.display_name FROM wp_posts P INNER JOIN wp_users U ON P.post_author = U.ID where P.ID= " + id;
            var data = _dal.GetData(sql);
            if (data.Tables.Count == 0) { return null; }
            if (data.Tables[0].Rows.Count == 0) { return null; }
            return GetPostFromDataRow(data.Tables[0].Rows[0]);
        }

        public IList<Post> GetPosts(IList<int> ids)
        {
            var sql = "SELECT P.*,U.display_name FROM wp_posts P INNER JOIN wp_users U ON P.post_author = U.ID where P.ID in(" + string.Join(",", ids) + ")";
            var data = _dal.GetData(sql);
            if (data.Tables.Count == 0) { return null; }
            if (data.Tables[0].Rows.Count == 0) { return null; }
            var result = new List<Post>();
            foreach (DataRow row in data.Tables[0].Rows)
            {
                result.Add(
                    GetPostFromDataRow(row)
                );
            }
            return result;
        }

        public IList<Post> GetImagePostsForPosts(IList<int> ids)
        {
            var sql = "SELECT P.*, 1 as display_name FROM wp_posts P where P.post_type='attachment' and P.post_parent in(" + string.Join(",", ids) + ") order by post_date Asc";
            var data = _dal.GetData(sql);
            if (data.Tables.Count == 0) { return null; }
            if (data.Tables[0].Rows.Count == 0) { return null; }
            var result = new List<Post>();
            foreach (DataRow row in data.Tables[0].Rows)
            {
                result.Add(
                    GetPostFromDataRow(row)
                );
            }
            return result;
        }

        public IList<Post> GetPosts(PostOrder order = PostOrder.NewestFirst, int limit = 100, bool onlyDraft = true)
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
            sql += " LIMIT 0," + limit;

            var data = _dal.GetData(sql);
            if (data.Tables.Count == 0) { return null; }
            if (data.Tables[0].Rows.Count == 0) { return null; }
            var result = new List<Post>();
            foreach (DataRow row in data.Tables[0].Rows)
            {
                result.Add(
                    GetPostFromDataRow(row)
                );
            }
            return result;
        }

        private static Post GetPostFromDataRow(DataRow row)
        {
            return new Post()
            {
                Id = row["ID"].ToString(),
                Title = row["post_title"].ToString(),
                PublishDateTime = (DateTime)row["post_date"],
                Content = row["post_content"].ToString(),
                Status = row["post_status"].ToString(),
                Name = row["post_name"].ToString(),
                Url = row["guid"].ToString(),
                PostType = row["post_type"].ToString(),
                Author = row["display_name"].ToString(),
                ParentId = row["post_parent"] == null ? "" : row["post_parent"].ToString()
            };
        }


        public void PublishPost(Post post)
        {
            var catDal = new CategoryDal(_dal);
            var postCategories = catDal.GetCategories(post.Id);
            var defaultCategoryId = catDal.GetInsertDefaultCategoryId();
            var categorySet = postCategories.Contains(defaultCategoryId);
            var uncategorizedSet = postCategories.Contains(1);

            var converterFunctions = new ConverterFunctions();
            var postName = converterFunctions.SeoPostUrl(post.Title);
            var sql = string.Format(
                "Update wp_posts set post_status='publish',post_date=NOW(),post_date_gmt=NOW(),post_modified=NOW(),post_modified_gmt=NOW(),post_name='{1}' where ID={0};",
                post.Id,
                postName.EscapeSql()
                );

            if (!uncategorizedSet)
            {
                sql += string.Format("Insert into wp_term_relationships(object_id,term_taxonomy_id,term_order) values({0}, {1},0);", post.Id, 1);
            }
            if (!categorySet)
            {
                sql += string.Format("Insert into wp_term_relationships(object_id,term_taxonomy_id,term_order) values({0}, {1},0);", post.Id, defaultCategoryId);
            }
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

        public DataSet GetAllPostMeta(string metaKey = "foreignkey")
        {
            var sql = string.Format("Select post_id,meta_value from wp_postmeta where meta_key='{0}'", metaKey);
            return _dal.GetData(sql);

        }

        public void SetPostMetaData(int postId, string metaKey, string metaValue)
        {
            var existSql = string.Format("Select post_id,meta_value from wp_postmeta where meta_key='{0}' and post_id='{1}'", metaKey, postId);
            var data = _dal.GetData(existSql);
            var exists = data.Tables.Count != 0;
            if (data.Tables[0].Rows.Count == 0) { exists = false; }
            if (!exists)
            {
                var insertSql = string.Format("INSERT INTO wp_postmeta(post_id, meta_key,meta_value) VALUES('{0}','{1}','{2}')", postId, metaKey, metaValue);
                _dal.ExecuteNonQuery(insertSql);

            }
            var sql = string.Format("Update wp_postmeta SET meta_value='{0}' where meta_key='{1}' and post_id={2}", metaValue, metaKey, postId);
            _dal.ExecuteNonQuery(sql);

        }

        public DataSet GetAllTitles()
        {
            var sql = "Select post_title from wp_posts";
            return _dal.GetData(sql);
        }
    }
}