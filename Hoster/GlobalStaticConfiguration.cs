using System;
using System.Windows;

namespace Hoster
{
    public class GlobalStaticConfiguration
    {
        // Average type speed --> 187 chars/min
        public static TimeSpan SearchDispatcherTimerTimeSpan => new TimeSpan(0, 0, 0, 0, 750);
        public static TimeSpan CredentialsUILockTime => new TimeSpan(0, 0, 120);
        public static double ProfileWidthCollapsed => 40;
        public static double ProfileDefaultWidthExpanded => 250;
        public static double ProfileMaxWidthExpanded => 350;

        private static string DBType;
        private static string DBHostName;
        private static string DBPort;
        private static string DBName;
        private static string DBUser;
        private static string DBPassword;
        private static string DBFile;
        private static bool _loaded;
        public static void LoadConfigs()
        {
            DBFile=  System.Configuration.ConfigurationSettings.AppSettings["DBFile"];
            DBType = System.Configuration.ConfigurationSettings.AppSettings["DBType"];
            DBHostName = System.Configuration.ConfigurationSettings.AppSettings["DBHostName"];
            DBPort = System.Configuration.ConfigurationSettings.AppSettings["DBPort"];
            DBName = System.Configuration.ConfigurationSettings.AppSettings["DBName"];
            DBUser = System.Configuration.ConfigurationSettings.AppSettings["DBUser"];
            DBPassword = System.Configuration.ConfigurationSettings.AppSettings["DBPassword"];
            _loaded = true;
        }

        public static string GetDBConnectionString()
        {
            if (!_loaded)
            {
                LoadConfigs();
            }

      
            string _connString="";
            switch (DBType.ToLower())
            {
                case "msaccess":
                    _connString=   "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + DBFile;
                    break;
                case "mssqlserver":
                    if(DBPassword.Trim()=="")
                    _connString= @"Data Source="+DBHostName+";Initial Catalog="+DBName+";User ID="+DBUser+";Password="+DBPassword+ ";Integrated Security=True";
                    else
                     _connString = @"Data Source=" + DBHostName + ";Initial Catalog=" + DBName + ";User ID=" + DBUser + ";Password=" + DBPassword + ";";
                    break;
                default:
                    break;
            }


            return _connString;
        }


        public static IDataAccess GetDataAccess()
        {
            string connString = GetDBConnectionString();

            switch (DBType.ToLower()) {
                case "msaccess":
                    return new MSAccessDataAccess(connString);
                    break;
                case "mssqlserver":
                    return new SQLServerDataAccess(connString);
                    break;
                default:
                    throw new Exception("Unknow database !");
            }

        }

    }
}