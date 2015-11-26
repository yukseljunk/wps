using System.Data;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication1
{
    public class Dal
    {
        private readonly string _connectionString;

        public Dal(string connectionString)
        {
            _connectionString = connectionString;
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

    }
}