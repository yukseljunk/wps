using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System;

namespace WordpressScraper.Dal
{
    public class Dal : IDisposable
    {
        private MySqlConnection _connection;

        private readonly string _connectionString;

        public Dal(string connectionString)
        {
            _connectionString = connectionString;
            
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_connection == null) return;
            try
            {
                _connection.Dispose();
                if (_connection.State == ConnectionState.Open || _connection.State == ConnectionState.Broken)
                {
                    _connection.Close();
                }
            }
            catch
            {
                // ignored
            }
        }

        public IList<string> TestConnection()
        {
            var result = new List<string>();
            try
            {
                CheckConnection();
                result.Add("Success");
            }
            catch (Exception exception)
            {
                result.Add(exception.ToString());

                if (exception.Message.Contains("mysql_native_password") && exception.Message.Contains("YES"))
                {
                    result.Add(
                        "You should specify your IP on Remote Database Access Hosts on mysql server, cpanel>Remote database access hosts>add an access host>your ip");
                }
            }
            finally
            {
                Dispose();
            }

            return result;
        }

        private void CheckConnection()
        {
            _connection = new MySqlConnection(_connectionString);
            _connection.Open();
        }

        public DataSet GetData(string sql)
        {
            var result = GetData(new List<string>() { sql });
            return result.Count > 0 ? result[0] : null;
        }

        public IList<DataSet> GetData(IList<string> sqls)
        {
            var result = new List<DataSet>();
            CheckConnection();
            try
            {
                foreach (var sql in sqls)
                {
                    var cmd = _connection.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandTimeout = 0;
                    var adapter = new MySqlDataAdapter(cmd);
                    var dataset = new DataSet();
                    adapter.Fill(dataset);
                    result.Add(dataset);
                }
            }
            finally
            {
                Dispose();
            }

            return result;
        }

        public IList<DataSet> GetDataAndNoQuery(IList<Tuple<string, bool>> sqls)
        {
            var result = new List<DataSet>();
            CheckConnection();
            try
            {
                foreach (var sql in sqls)
                {
                    if (sql.Item2)
                    {
                        var cmd = _connection.CreateCommand();
                        cmd.CommandText = sql.Item1;
                        var adapter = new MySqlDataAdapter(cmd);
                        var dataset = new DataSet();
                        adapter.Fill(dataset);
                        result.Add(dataset);
                    }
                    else
                    {
                        var cmd = _connection.CreateCommand();
                        cmd.CommandText = sql.Item1;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            finally
            {
                Dispose();
            }



            return result;
        }

        public void ExecuteNonQuery(IList<string> sqls)
        {
            CheckConnection();
            try
            {
                foreach (var sql in sqls)
                {
                    if (!_connection.Ping())
                    {
                        _connection.Open();
                    }
                    var cmd = _connection.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                   
                }
            }
            finally
            {
                Dispose();
            }

        }

        public void ExecuteNonQuery(string sql)
        {
            ExecuteNonQuery(new List<string>() { sql });
        }


    }
}