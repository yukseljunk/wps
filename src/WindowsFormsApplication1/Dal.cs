using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using PttLib.TourInfo;
using System;

namespace WindowsFormsApplication1
{
    public class Dal
    {
        private readonly string _connectionString;

        public Dal(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IList<string> TestConnection()
        {
            var result = new List<string>();
            MySqlConnection connection = null;
            try
            {
                using (connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                }
                result.Add("Success");
            }
            catch (Exception exception)
            {
                result.Add(exception.ToString());

                if (exception.Message.Contains("mysql_native_password") && exception.Message.Contains("YES"))
                {
                    result.Add("You should specify your IP on Remote Database Access Hosts on mysql server, cpanel>Remote database access hosts>add an access host>your ip");
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return result;
        }

        public DataSet GetData(string sql)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = sql;
                    var adapter = new MySqlDataAdapter(cmd);
                    var dataset = new DataSet();
                    adapter.Fill(dataset);
                    return dataset;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }

            }
        }

        public IList<int> UserIds()
        {
            var result = new List<int>();
            var userDataSet = GetData("Select ID from wp_users");
            if (userDataSet.Tables.Count == 0) { return result; }
            if (userDataSet.Tables[0].Rows.Count == 0) { return result; }
            foreach (DataRow dataRow in userDataSet.Tables[0].Rows)
            {
                result.Add(int.Parse(dataRow[0].ToString()));
            }
            return result;
        } 

        public int InsertUser(string displayname, string userName = "", string email = "", string userUrl = "", string passEncoded = "$P$BYvykzVw6vXRlA4jyW85HZxrCoJoE40")
        {
            if (string.IsNullOrEmpty(userName))
            {
                var converterFunctions = new ConverterFunctions();
                userName = converterFunctions.SeoUrl(displayname);
            }
            if (string.IsNullOrEmpty(email))
            {
                email = userName + "@mysite.com";
            }
            if (string.IsNullOrEmpty(userUrl))
            {
                userUrl = "http:\\www." + userName.Replace("-", "") + ".com";
            }

            //duplicate check
            var userDataSet = GetData(string.Format("Select ID from wp_users Where user_login='{0}'",userName));
            if (userDataSet.Tables.Count > 0 && userDataSet.Tables[0].Rows.Count > 0 &&
                userDataSet.Tables[0].Rows[0].ItemArray.Length > 0)
            {
                return 0;
            }

            var userInsertDataSet = GetData(string.Format(
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