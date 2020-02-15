using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZPublisher
{
  public  class OraclePublisher:IPublisher
    {
        private string _connectionString;
        private string _tableName;
        private string _sampleIDCol;
        private string _testDescCol;
        private string _testResultCol;
        private string _unitCol;
        private string _normalRangeCol;

        public OraclePublisher(string dbHost, int dbPort, string dbName, string dbUser, string dbPassword, string dbService)
        {
            Main.LoadConfigurationData();
            _connectionString = " Data Source = (DESCRIPTION = (ADDRESS_LIST = "
           + "(ADDRESS=(PROTOCOL=TCP)(HOST=" + dbHost + ")(PORT="+dbPort+")))"
           + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME="+dbService+")));"
           + "User Id=" + dbUser + "; Password="+dbPassword+";";
        }


        public bool Publish(string tableName, Dictionary<string ,string> dataTuples)
        {
            bool _success = false;
            using(OracleConnection conn=new OracleConnection())
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO "+ tableName + "("+_sampleIDCol+","+_testDescCol+","+_testResultCol+","+_unitCol +","+_normalRangeCol+") values()";
                int rspns = cmd.ExecuteNonQuery();
                if (rspns == 1)
                    _success = true;
            }
            return _success;

        }

    }
}
