using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System;

namespace WordpressScraper.Dal
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
            var result = GetData(new List<string>() { sql });
            return result.Count > 0 ? result[0] : null;
        }

        public IList<DataSet> GetData(IList<string> sqls)
        {
            var result = new List<DataSet>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    foreach (var sql in sqls)
                    {
                        var cmd = connection.CreateCommand();
                        cmd.CommandText = sql;
                        var adapter = new MySqlDataAdapter(cmd);
                        var dataset = new DataSet();
                        adapter.Fill(dataset);
                        result.Add(dataset);
                    }
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }

            }
            return result;
        }

        public IList<DataSet> GetDataAndNoQuery(IList<Tuple<string, bool>> sqls)
        {

            var result = new List<DataSet>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    foreach (var sql in sqls)
                    {
                        if (sql.Item2)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.CommandText = sql.Item1;
                            var adapter = new MySqlDataAdapter(cmd);
                            var dataset = new DataSet();
                            adapter.Fill(dataset);
                            result.Add(dataset);
                        }
                        else
                        {
                            var cmd = connection.CreateCommand();
                            cmd.CommandText = sql.Item1;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }

            }
            return result;
        }

        public void ExecuteNonQuery(IList<string> sqls)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    foreach (var sql in sqls)
                    {
                        var cmd = connection.CreateCommand();
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
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

        public void ExecuteNonQuery(string sql)
        {
            ExecuteNonQuery(new List<string>() { sql });
        }




    }
}