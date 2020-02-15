using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoster
{
   public class MSAccessDataAccess:IDataAccess
    {
        private string _connectionString;
        public MSAccessDataAccess(string connectionString)
        {
          
            _connectionString = connectionString;
        }
        public DataTable GetDevices()
        {
            DataTable _dt = new DataTable(); ;
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.CommandText = "SELECT * FROM devices";
                    cmd.Connection = conn;
                    OleDbDataAdapter adptr = new OleDbDataAdapter(cmd);
                    adptr.Fill(_dt);
                }
                catch (Exception ex)
                {
                   LogWriter.WriteErrorLog("Error happened in GetDevices : " + ex.Message);
                }
            }
            return _dt;
        }

        public DataTable GetProtocols()
        {
            DataTable _dt = new DataTable(); ;
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.CommandText = "SELECT * FROM protocols";
                    cmd.Connection = conn;
                    OleDbDataAdapter adptr = new OleDbDataAdapter(cmd);
                    adptr.Fill(_dt);
                }
                catch (Exception ex)
                {
                    LogWriter.WriteErrorLog("Error happened in GetProtocols : " + ex.Message);
                }
            }
            return _dt;
        }

        public DataTable GetConnectionTypes()
        {
            DataTable _dt = new DataTable(); ;
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.CommandText = "SELECT * FROM connection_types";
                    cmd.Connection = conn;
                    OleDbDataAdapter adptr = new OleDbDataAdapter(cmd);
                    adptr.Fill(_dt);
                }
                catch (Exception ex)
                {
                    LogWriter.WriteErrorLog("Error happened in GetConnectionTypes : " + ex.Message);
                }
            }
            return _dt;
        }


        public DataTable GetDBTypes()
        {
            DataTable _dt = new DataTable(); ;
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.CommandText = "SELECT * FROM lis_db_types";
                    cmd.Connection = conn;
                    OleDbDataAdapter adptr = new OleDbDataAdapter(cmd);
                    adptr.Fill(_dt);
                }
                catch (Exception ex)
                {
                    LogWriter.WriteErrorLog("Error happened in GetDBTypes : " + ex.Message);
                }
            }
            return _dt;
        }
        public DataTable GetListeners()
        {
            DataTable _dt = new DataTable(); ;
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.CommandText = "SELECT * FROM listeners";
                    cmd.Connection = conn;
                    OleDbDataAdapter adptr = new OleDbDataAdapter(cmd);
                    adptr.Fill(_dt);
                }
                catch (Exception ex)
                {
                    LogWriter.WriteErrorLog("Error happened in GetListeners : " + ex.Message);
                }
            }
            return _dt;
        }

        public DataTable GetListenerByID(int listenerID)
        {
            DataTable _dt = new DataTable(); 
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.CommandText = "SELECT * FROM listeners WHERE id="+listenerID;
                    cmd.Connection = conn;
                    OleDbDataAdapter adptr = new OleDbDataAdapter(cmd);
                    adptr.Fill(_dt);
                }
                catch (Exception ex)
                {
                    LogWriter.WriteErrorLog("Error happened in GetListenerByID : " + ex.Message);
                }
            }
            return _dt;
        }

        public int Update(string tableName,string primaryCol,Dictionary<string,string>data)
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
               
                if (colCount == 1)
                    setStatement += " " + pair.Key + " = " + pair.Value + " ";
                else
                    setStatement += " , " + pair.Key + " = '" + pair.Value + "' ";

                colCount++;
            }
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.CommandText = "UPDATE "+tableName+" "+setStatement+" "+ primaryCondition ;
                    cmd.Connection = conn;
                    i=cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    LogWriter.WriteErrorLog("Error happened in Update : " + ex.Message);
                }
            }
            return i;
        }

        public DataTable Query(string query)
        {
            throw new NotImplementedException();
        }

        public int ExecuteQuery(string query)
        {
            throw new NotImplementedException();
        }

        public int Insert(string tableName, Dictionary<string, string> data)
        {
            throw new NotImplementedException();
        }

        public int Delete(string tableName, Dictionary<string, string> conditions)
        {
            throw new NotImplementedException();
        }

        public DataTable GetStoredResults(int listnerID, int limit = 500)
        {
            throw new NotImplementedException();
        }

        public bool HasTestsCodes(int deviceID)
        {
            throw new NotImplementedException();
        }

        public string GetTestCode(int deviceID, string deviceTestCode)
        {
            throw new NotImplementedException();
        }

        public DataTable GetDeviceTestsCodes(int deviceID)
        {
            throw new NotImplementedException();
        }
    }
}
