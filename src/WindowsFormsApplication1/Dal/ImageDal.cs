using System;
using System.Collections.Generic;
using System.Text;
using PttLib.Helpers;
using PttLib.TourInfo;
using WordPressSharp.Models;
using System.IO;
using System.Linq;

namespace WordpressScraper.Dal
{
    public class ImageDal
    {
        private readonly Dal _dal;

        public ImageDal(Dal dal)
        {
            _dal = dal;
        }

        /// <summary>
        /// image.Content> description of the image
        /// image.Exerpt> caption of the image
        /// image.Status> always inherit
        /// image.MimeType>default image/jpeg
        /// image.Name> image.url.filename
        /// image.Title=image.Name
        /// </summary>
        /// <param name="image"></param>
        /// <param name="ftpDir"></param>
        /// <returns></returns>
        public int Insert(ImagePost image, string ftpDir)
        {
            var customFields = new List<CustomField>();
            var customFieldSql = new StringBuilder();
            if (image.CustomFields != null)
            {
                customFields = image.CustomFields.ToList();
            }
            var postName = Path.GetFileNameWithoutExtension(image.Url) + Path.GetExtension(image.Url);

            if (!string.IsNullOrEmpty(image.Alt))
            {
                customFields.Add(new CustomField() { Key = "_wp_attachment_image_alt", Value = image.Alt });
                customFields.Add(new CustomField() { Key = "_wp_attached_file", Value = ftpDir + "/" + postName });

                //TODO: figure out this data...
                customFields.Add(new CustomField() { Key = "_wp_attachment_metadata", Value = "a:5:{s:5:\"width\";i:640;s:6:\"height\";i:640;s:4:\"file\";s:63:\"2015/12/shopping-cart-cover-for-girls--accessories-for-ba-1.jpg\";s:5:\"sizes\";a:1:{s:9:\"thumbnail\";a:4:{s:4:\"file\";s:63:\"shopping-cart-cover-for-girls--accessories-for-ba-1-150x150.jpg\";s:5:\"width\";i:150;s:6:\"height\";i:150;}}}" });

            }

            foreach (var customField in customFields)
            {
                customFieldSql.Append(
                    string.Format(
                        "INSERT INTO wp_postmeta( post_id, meta_key, meta_value) VALUES (@l,'{0}','{1}');",
                        customField.Key.EscapeSql(), customField.Value.EscapeSql()));

            }



            var sql = string.Format(
                "INSERT INTO wp_posts(post_author, post_date, post_date_gmt, post_content, post_title, post_excerpt, post_status, comment_status, " +
                "ping_status, post_password, post_name, to_ping, pinged, post_modified, post_modified_gmt, post_content_filtered, post_parent, guid, " +
                "menu_order, post_type, post_mime_type, comment_count) VALUES " +
                "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}');" +
                "SET @l=LAST_INSERT_ID();" +
                "{22}SELECT @l;",
                image.Author, image.PublishDateTime.ToString("yyyy-MM-dd HH':'mm':'ss"), image.PublishDateTime.ToString("yyyy-MM-dd HH':'mm':'ss"), image.Content.EscapeSql(), postName.EscapeSql(), image.Exerpt, "inherit",
                "open", "closed",
                "", postName.EscapeSql(), "", "", DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss"), DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss"), "", 0, image.Url, 0,
                "attachment", image.MimeType, 0, customFieldSql.ToString());

            var postInsertDataSet = _dal.GetData(sql);

            if (postInsertDataSet.Tables.Count == 0) { return -1; }
            if (postInsertDataSet.Tables[0].Rows.Count == 0) { return -1; }
            if (postInsertDataSet.Tables[0].Rows[0].ItemArray.Length == 0) { return -1; }

            var id = postInsertDataSet.Tables[0].Rows[0][0].ToString();
            return int.Parse(id);
        }

    }
}