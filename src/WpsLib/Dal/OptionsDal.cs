using System.Data;
using PttLib.Helpers;

namespace WpsLib.Dal
{
    public class OptionsDal
    {
        private readonly Dal _dal;

        public OptionsDal(Dal dal)
        {
            _dal = dal;
        }
        public DataSet GetAllOptions()
        {
            var sql = "SELECT option_id, option_name, option_value, autoload FROM  wp_options";
            return _dal.GetData(sql);
        }

        public string GetValue(string key)
        {
            var optionDataSet = _dal.GetData(string.Format("Select option_id, option_name, option_value from wp_options Where option_name='{0}'", key));
            if (optionDataSet.Tables.Count > 0 && optionDataSet.Tables[0].Rows.Count > 0 &&
                optionDataSet.Tables[0].Rows[0].ItemArray.Length > 0)
            {
                return optionDataSet.Tables[0].Rows[0][2].ToString();

            }
            return null;
        }

        public void SetValue(string key, string value)
        {
            //duplicate check
            var optionDataSet = _dal.GetData(string.Format("Select option_id from wp_options Where option_name='{0}'", key));
            if (optionDataSet.Tables.Count > 0 && optionDataSet.Tables[0].Rows.Count > 0 &&
                optionDataSet.Tables[0].Rows[0].ItemArray.Length > 0)
            {
                //option is present, so update
                _dal.ExecuteNonQuery(UpdateSql(key, value));
                return;

            }

            //insert the option
            var insertedOption = InsertOption(key, value);

        }

        public int InsertOption(string key, string value)
        {
            var optionInsertDataSet = _dal.GetData(InsertSql(key, value));

            if (optionInsertDataSet.Tables.Count == 0) { return -1; }
            if (optionInsertDataSet.Tables[0].Rows.Count == 0) { return -1; }
            if (optionInsertDataSet.Tables[0].Rows[0].ItemArray.Length == 0) { return -1; }

            var id = optionInsertDataSet.Tables[0].Rows[0][0].ToString();
            return int.Parse(id);
        }

        public string InsertSql(string key, string value)
        {
            return string.Format(
                "INSERT INTO wp_options(option_name, option_value, autoload) VALUES" +
                "('{0}','{1}',1);" +
                "SET @l=LAST_INSERT_ID();" +
                "SELECT @l;",
                key.EscapeSql(),
                value.EscapeSql());
        }

        public string UpdateSql(string key, string value)
        {
            return string.Format(
                "UPDATE wp_options SET option_value='{1}' WHERE option_name='{0}'",
                key.EscapeSql(),
                value.EscapeSql());
        }
    }
}