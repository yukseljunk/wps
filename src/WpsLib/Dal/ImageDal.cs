using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using PttLib.Helpers;
using WordPressSharp.Models;

namespace WpsLib.Dal
{
    public class ImageDal
    {
        private readonly WpsLib.Dal.Dal _dal;

        public ImageDal(WpsLib.Dal.Dal dal)
        {
            _dal = dal;
        }

        public IList<int> Insert(IList<ImagePost> images, string ftpDir)
        {
            var sql = "";
            foreach (var imagePost in images)
            {
                sql += InsertSql(imagePost, ftpDir);
            }

            var postInsertDataSet = _dal.GetData(sql);
            if (postInsertDataSet.Tables.Count == 0) { return null; }
            var result = new List<int>();
            foreach (DataTable table in postInsertDataSet.Tables)
            {
                var imageId = 0;
                if (table.Rows.Count == 0) { result.Add(imageId); continue; }
                if (table.Rows[0].ItemArray.Length == 0) { result.Add(imageId); continue; }
                imageId = int.Parse(table.Rows[0][0].ToString());
                result.Add(imageId);

            }
            return result;
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
            var sql = InsertSql(image, ftpDir);

            var postInsertDataSet = _dal.GetData(sql);

            if (postInsertDataSet.Tables.Count == 0) { return -1; }
            if (postInsertDataSet.Tables[0].Rows.Count == 0) { return -1; }
            if (postInsertDataSet.Tables[0].Rows[0].ItemArray.Length == 0) { return -1; }

            var id = postInsertDataSet.Tables[0].Rows[0][0].ToString();
            return int.Parse(id);
        }


        public string InsertSql(ImagePost image, string ftpDir)
        {
            var customFields = new List<CustomField>();
            var customFieldSql = new StringBuilder();
            if (image.CustomFields != null)
            {
                customFields = image.CustomFields.ToList();
            }
            var postName = Path.GetFileNameWithoutExtension(image.Url);// + Path.GetExtension(image.Url);

            var extension = Path.GetExtension(image.Url);
            var attachFile = ftpDir + "/" + postName + extension;

            if (!string.IsNullOrEmpty(image.Alt))
            {
                customFields.Add(new CustomField() { Key = "_wp_attachment_image_alt", Value = image.Alt });
            }
            customFields.Add(new CustomField() { Key = "_wp_attached_file", Value = attachFile });


            foreach (var customField in customFields)
            {
                customFieldSql.Append(
                    string.Format(
                        "INSERT INTO wp_postmeta( post_id, meta_key, meta_value) VALUES (@l,'{0}','{1}');",
                        customField.Key.EscapeSql(), customField.Value.EscapeSql()));

            }

            return string.Format(
                "INSERT INTO wp_posts(post_author, post_date, post_date_gmt, post_content, post_title, post_excerpt, post_status, comment_status, " +
                "ping_status, post_password, post_name, to_ping, pinged, post_modified, post_modified_gmt, post_content_filtered, post_parent, guid, " +
                "menu_order, post_type, post_mime_type, comment_count) VALUES " +
                "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}');" +
                "SET @l=LAST_INSERT_ID();" +
                "{22}SELECT @l;",
                image.Author, image.PublishDateTime.ToString("yyyy-MM-dd HH':'mm':'ss"), image.PublishDateTime.ToString("yyyy-MM-dd HH':'mm':'ss"), image.Content.EscapeSql(), postName.EscapeSql(), image.Exerpt, "inherit",
                "open", "closed",
                "", image.Title.EscapeSql(), "", "", DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss"), DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss"), "", 0, image.Url.EscapeSql(), 0,
                "attachment", image.MimeType, 0, customFieldSql.ToString());


        }

        public void InsertAttachmentMetaData(int postId, ImagePost image, string ftpDir)
        {
            var postName = Path.GetFileNameWithoutExtension(image.Url);// + Path.GetExtension(image.Url);
            var extension = Path.GetExtension(image.Url);
            var attachFile = ftpDir + "/" + postName + extension;

            var thumbnailName = string.Format("{0}-{1}x{2}{3}", postName, image.ThumbnailWidth, image.ThumbnailHeight, extension);
            var orientation = image.Width >= image.Height ? 0 : 1;
            var attachmentMetaData = string.Format("a:5:{{" +
                                                   "s:5:\"width\";i:{0};" +
                                                   "s:6:\"height\";i:{1};" +
                                                   "s:4:\"file\";s:{8}:\"{2}\";" +
                                                   "s:5:\"sizes\";a:3:{{" +
                                                   "s:9:\"thumbnail\";a:4:{{" +
                                                   "s:4:\"file\";s:{10}:\"{9}\";" +
                                                   "s:5:\"width\";i:{3};" +
                                                   "s:6:\"height\";i:{4};" +
                                                   "s:9:\"mime-type\";s:{11}:\"{6}\";" +
                                                   "}}" +
                                                   "s:6:\"medium\";a:4:{{" +
                                                   "s:4:\"file\";s:{10}:\"{7}-164x300{5}\";" +
                                                   "s:5:\"width\";i:164;" +
                                                   "s:6:\"height\";i:300;" +
                                                   "s:9:\"mime-type\";s:{11}:\"{6}\";}}" +
                                                   "s:13:\"excerpt-thumb\";a:4:{{" +
                                                   "s:4:\"file\";s:{10}:\"{7}-191x350{5}\";" +
                                                   "s:5:\"width\";i:191;" +
                                                   "s:6:\"height\";i:350;" +
                                                   "s:9:\"mime-type\";s:{11}:\"{6}\";" +
                                                   "}}" +
                                                   "}}" +
                                                   "s:10:\"image_meta\";a:11:{{" +
                                                   "s:8:\"aperture\";i:0;" +
                                                   "s:6:\"credit\";s:0:\"\";" +
                                                   "s:6:\"camera\";s:0:\"\";" +
                                                   "s:7:\"caption\";s:0:\"\";" +
                                                   "s:17:\"created_timestamp\";i:0;" +
                                                   "s:9:\"copyright\";s:0:\"\";" +
                                                   "s:12:\"focal_length\";s:2:\"50\";" +
                                                   "s:3:\"iso\";s:3:\"800\";" +
                                                   "s:13:\"shutter_speed\";i:0;" +
                                                   "s:5:\"title\";s:0:\"\";" +
                                                   "s:11:\"orientation\";i:{12};" +
                                                   "}}" +
                                                   "}}",
                                                   image.Width,
                                                   image.Height,
                                                   attachFile,
                                                   image.ThumbnailWidth,
                                                   image.ThumbnailHeight,
                                                   extension,
                                                   image.MimeType,
                                                   postName,
                                                   attachFile.Length,
                                                   thumbnailName,
                                                   thumbnailName.Length,
                                                   image.MimeType.Length,
                                                   orientation);

            var sql= string.Format(
                "INSERT INTO wp_postmeta( post_id, meta_key, meta_value) VALUES ({0},'_wp_attachment_metadata','{1}');",
                postId, attachmentMetaData.EscapeSql());
            _dal.ExecuteNonQuery(sql);
        }
    }
}