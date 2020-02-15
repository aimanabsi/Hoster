using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZPublisher
{
  
 public   class Main
    {
        public static string DBServer = "";
        public static string Port = "";
        public static string Service = "";
        public static string DBUser = "";
        public static string DBPassword = "";
        public static void LoadConfigurationData()
        {
            DBServer = System.Configuration.ConfigurationSettings.AppSettings["DBServer"];
            Port = System.Configuration.ConfigurationSettings.AppSettings["Port"];
            Service = System.Configuration.ConfigurationSettings.AppSettings["Service"];
            DBUser = System.Configuration.ConfigurationSettings.AppSettings["DBUser"];
            DBPassword = System.Configuration.ConfigurationSettings.AppSettings["Password"];
        }
    }
}
