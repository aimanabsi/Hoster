using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZPublisher
{
  public  interface IPublisher
    {
       
        /// <summary>
        /// publish the data to the LIS DB
        /// </summary>
        /// <param name="tableName">name of the table that data will be inserted into </param>
        /// <param name="dataTuples">the data with the coloumns names cmbined in a tuple</param>
        /// <returns></returns>
         bool Publish(string tableName, Dictionary<string, string> dataTuples);

    }
}
