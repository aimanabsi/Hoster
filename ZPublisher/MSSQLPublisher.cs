using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ZPublisher
{
    public class MSSQLPublisher : IPublisher
    {
        private string _connectionString;

        public MSSQLPublisher(string connString)
        {
            this._connectionString = connString;
        }
        public bool Publish(string tableName, Dictionary<string, string> dataTuples)
        {

           
            bool inserted = false;
            int i = 0;
            string colsStatement = " ";
            string valuesStatement = "";
            int colCount = 1;
            string deleteStatement = "DELETE FROM " + tableName + " WHERE ";
            foreach (KeyValuePair<string, string> pair in dataTuples)
            {

                if (colCount == 1)
                {
                    colsStatement += " " + pair.Key + " ";
                    deleteStatement += " " + pair.Key + "='" + pair.Value + "'";
                    valuesStatement += " '" + pair.Value + "' ";
                }
                else if (colCount == 2)
                {

                    colsStatement += " , " + pair.Key + " ";
                    valuesStatement += " , '" + pair.Value + "' ";
                    deleteStatement += " AND " + pair.Key + "='" + pair.Value + "'";
                }

                else
                {
                    colsStatement += " , " + pair.Key + " ";
                    valuesStatement += " , '" + pair.Value + "' ";
                }


                colCount++;
            }
          
            using (var transaction=new TransactionScope())
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand delCmd = new SqlCommand();
                        delCmd.CommandText = deleteStatement;
                        delCmd.Connection = conn;
                        delCmd.ExecuteNonQuery();

                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = "INSERT into  " + tableName + "(" + colsStatement + ") VALUES(" + valuesStatement + ") ";
                        cmd.Connection = conn;
                        i = cmd.ExecuteNonQuery();

                        transaction.Complete();
                        inserted = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        LogWriter.WriteErrorLog("Error happened in Publish : " + ex.Message);
                    }
                }

            }

           
           

            return inserted;
           
        }
    }
}
