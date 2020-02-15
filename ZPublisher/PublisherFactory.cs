using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZPublisher
{
  public  class PublisherFactory
    {
        /// <summary>
        /// /Create the required db driver 
        /// 
        /// </summary>
        const int ORACLE_DB=1;
        const int MSSQLSERVER_DB = 2;
        const int MYSQL_DB = 3;
        public PublisherFactory()
        {

        }

        public static IPublisher CreatePublisher( int dbType,string dbHost,int dbPort,string dbName,string dbUesr,string dbPassword,string dbService)
        {
            IPublisher publisher=null;
            switch (dbType)
            {
                case ORACLE_DB:
                    publisher = new OraclePublisher(dbHost, dbPort, dbName, dbUesr, dbPassword, dbService);
                    break;
                case MSSQLSERVER_DB:
                    publisher = new MSSQLPublisher(CreateConnectionString(dbType,dbHost,dbPort,dbName, dbUesr, dbPassword,dbService));
                    break;
                case MYSQL_DB:
                    publisher = new MYSQLPublisher();
                    break;
                default:
                    publisher = new OraclePublisher(dbHost, dbPort, dbName, dbUesr, dbPassword, dbService);
                    break;
            }
            return publisher;
        }
        public static string CreateConnectionString(int dbType, string dbHost, int dbPort, string dbName, string dbUesr, string dbPassword, string dbService)
        {



            string _connString = "";
            switch (dbType)
            {
             
                case MSSQLSERVER_DB:
                    _connString = @"Data Source=" + dbHost + ";Initial Catalog=" + dbName + ";User ID=" + dbUesr + ";Password=" + dbPassword + ";";
                    break;
                default:
                    break;
            }


            return _connString;
        }

    }
}
