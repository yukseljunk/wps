using System;
using System.Collections.Generic;
using System.Data;
using PttLib.TourInfo;

namespace WordpressScraper.Dal
{
    public class UserDal
    {
        private readonly Dal _dal;

        public UserDal(Dal dal)
        {
            _dal = dal;
        }

        public IList<int> UserIds()
        {
            var result = new List<int>();
            var userDataSet = _dal.GetData("Select ID from wp_users");
            if (userDataSet.Tables.Count == 0) { return result; }
            if (userDataSet.Tables[0].Rows.Count == 0) { return result; }
            foreach (DataRow dataRow in userDataSet.Tables[0].Rows)
            {
                result.Add(int.Parse(dataRow[0].ToString()));
            }
            return result;
        }

        public int InsertUser(string displayname, string blogName="mysite.com", string userName = "", string email = "", string userUrl = "", string passEncoded = "$P$BYvykzVw6vXRlA4jyW85HZxrCoJoE40")
        {
            if (string.IsNullOrEmpty(userName))
            {
                var converterFunctions = new ConverterFunctions();
                userName = converterFunctions.SeoUrl(displayname);
            }
            if (string.IsNullOrEmpty(email))
            {
                email = userName + "@" + blogName;
            }
            if (string.IsNullOrEmpty(userUrl))
            {
                userUrl = "http:\\www." + userName.Replace("-", "") + ".com";
            }

            //duplicate check
            var userDataSet = _dal.GetData(string.Format("Select ID from wp_users Where user_login='{0}'", userName));
            if (userDataSet.Tables.Count > 0 && userDataSet.Tables[0].Rows.Count > 0 &&
                userDataSet.Tables[0].Rows[0].ItemArray.Length > 0)
            {
                return 0;
            }

            var userInsertDataSet = _dal.GetData(string.Format(
                "INSERT INTO wp_users(user_login, user_pass, user_nicename, user_email, user_url, user_registered, user_activation_key, user_status, display_name)" +
                " VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}');SELECT LAST_INSERT_ID();",
                userName,
                passEncoded,
                userName,
                email,
                userUrl,
                DateTime.Now.ToLongDateString(),
                passEncoded,
                0,
                displayname));
            if (userInsertDataSet.Tables.Count == 0) { return -1; }
            if (userInsertDataSet.Tables[0].Rows.Count == 0) { return -1; }
            if (userInsertDataSet.Tables[0].Rows[0].ItemArray.Length == 0) { return -1; }

            return int.Parse(userInsertDataSet.Tables[0].Rows[0][0].ToString());



        }

    }
}