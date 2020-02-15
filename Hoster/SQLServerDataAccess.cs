using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hoster
{
    class SQLServerDataAccess : IDataAccess
    {
        string _connectionString;
        public SQLServerDataAccess(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public int Delete(string tableName, Dictionary<string, string> conditions)
        {
            throw new NotImplementedException();
        }

        public int ExecuteQuery(string query)
        {
            throw new NotImplementedException();
        }

        public DataTable GetConnectionTypes()
        {

            DataTable _dt = new DataTable(); ;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "SELECT * FROM connection_types";
                    cmd.Connection = conn;
                    SqlDataAdapter adptr = new SqlDataAdapter(cmd);
                    adptr.Fill(_dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error happened in GetListeners : " + ex.Message);
                }
            }
            return _dt;
        }

        public DataTable GetDBTypes()
        {

            DataTable _dt = new DataTable(); ;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "SELECT * FROM lis_db_types";
                    cmd.Connection = conn;
                    SqlDataAdapter adptr = new SqlDataAdapter(cmd);
                    adptr.Fill(_dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error happened in GetListeners : " + ex.Message);
                }
            }
            return _dt;
        }

        public DataTable GetDevices()
        {

            DataTable _dt = new DataTable(); ;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "SELECT * FROM devices";
                    cmd.Connection = conn;
                    SqlDataAdapter adptr = new SqlDataAdapter(cmd);
                    adptr.Fill(_dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error happened in GetDevices : " + ex.Message);
                }
            }
            return _dt;
        }

        public DataTable GetListenerByID(int listenerID)
        {

            DataTable _dt = new DataTable(); ;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "SELECT * FROM listeners where id="+listenerID;
                    cmd.Connection = conn;
                    SqlDataAdapter adptr = new SqlDataAdapter(cmd);
                    adptr.Fill(_dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error happened in GetListeners : " + ex.Message);
                }
            }
            return _dt;
        }

        public DataTable GetListeners()
        {
            DataTable _dt = new DataTable(); ;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "SELECT * FROM listeners";
                    cmd.Connection = conn;
                    SqlDataAdapter adptr = new SqlDataAdapter(cmd);
                    adptr.Fill(_dt);
                }
                catch (Exception ex)
                {
                   MessageBox.Show("Error happened in GetListeners : " + ex.Message);
                }
            }
            return _dt;
        }

        public DataTable GetProtocols()
        {

            DataTable _dt = new DataTable(); ;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "SELECT * FROM protocols";
                    cmd.Connection = conn;
                    SqlDataAdapter adptr = new SqlDataAdapter(cmd);
                    adptr.Fill(_dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error happened in GetProtocols : " + ex.Message);
                }
            }
            return _dt;
        }

        public DataTable GetStoredResults(int listenerID, int limit = 500)
        {
            return Query("SELECT TOP " + limit + " * FROM " + DBTablesNames.RESULTS_TABLE + " where listener_id="+ listenerID);
        }

        public int Insert(string tableName, Dictionary<string, string> data)
        {
            int i = 0;
            string colsStatement = " ";
            string valuesStatement="";
            int colCount = 1;
            foreach (KeyValuePair<string, string> pair in data)
            {

                if (colCount == 1)
                {
                    colsStatement += " " + pair.Key + " ";
                    valuesStatement += " '" + pair.Value + "' ";
                }

                else {
                    colsStatement += " , " + pair.Key + " ";
                    valuesStatement += " , '" + pair.Value + "' ";
                }
                    

                colCount++;
            }
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "INSERT into  " + tableName + "("+colsStatement+") VALUES(" + valuesStatement + ") ";
                    LogWriter.WriteErrorLog("Insert Statement : " + cmd.CommandText);
                    cmd.Connection = conn;
                    i = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    LogWriter.WriteErrorLog("Error happened in Insert : " + ex.Message);
                }
            }
            return i;
        }

        public DataTable Query(string query)
        {
            DataTable _dt = new DataTable(); ;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = conn;
                    SqlDataAdapter adptr = new SqlDataAdapter(cmd);
                    adptr.Fill(_dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error happened in Query : " + ex.Message);
                }
            }
            return _dt;
        }

        public int Update(string tableName, string primaryCol, Dictionary<string, string> data)
        {
            int i = 0;
            string setStatement = " SET ";
            string primaryCondition = "";
            int colCount = 1;
            foreach (KeyValuePair<string, string> pair in data)
            {
                if (pair.Key == primaryCol)
                {
                    primaryCondition = " WHERE " + primaryCol + " = " + pair.Value;
                    continue;
                }

                if (colCount == 1) {
                        setStatement += " " + pair.Key + " = " + pair.Value + " ";
                }
                
                else
                    setStatement += " , " + pair.Key + " = '" + pair.Value + "' ";

                colCount++;
            }
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "UPDATE " + tableName + " " + setStatement + " " + primaryCondition;
                    cmd.Connection = conn;
                    i = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    LogWriter.WriteErrorLog("Error happened in Update : " + ex.Message);
                }
            }
            return i;
        }

        public bool HasTestsCodes(int deviceID)
        {
            return false;

        }

        public string GetTestCode(int deviceID, string deviceTestCode)
        {
            string testCode = "";
            DataTable dt = Query("SELECT TOP 1 test_code FROM device_tests_codes where device_id="+deviceID+" AND device_test_code='"+deviceTestCode+"'");
            foreach(DataRow dr in dt.Rows)
            {
                testCode = dr["test_code"].ToString();
            }

            return testCode;
        }

        public DataTable GetDeviceTestsCodes(int deviceID)
        {
            return Query("SELECT * FROM device_tests_codes WHERE device_id=" + deviceID);
        }
    }
}
