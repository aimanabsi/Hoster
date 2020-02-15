using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoster
{
   public interface IDataAccess
    {
        DataTable Query(string query);
        int ExecuteQuery(string query);
        DataTable GetDevices();
        DataTable GetProtocols();
        DataTable GetConnectionTypes();
        DataTable GetDBTypes();

        DataTable GetListeners();
        DataTable GetListenerByID(int listenerID);
        int Update(string tableName, string primaryCol, Dictionary<string, string> data);
        int Insert(string tableName, Dictionary<string, string> data);
        int Delete(string tableName,Dictionary<string,string>conditions);
        DataTable GetStoredResults(int listnerID,int limit=500);
        bool HasTestsCodes(int deviceID);
        string GetTestCode(int deviceID,string deviceTestCode);
        DataTable GetDeviceTestsCodes(int deviceID);
    }
}
