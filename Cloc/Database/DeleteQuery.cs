using Cloc.Classes;
using MySql.Data.MySqlClient;
using System;
using static Cloc.Classes.Security;

namespace Cloc.Database
{
    internal class DeleteQuery
    {
        internal static bool DeleteWorkerQuery(string UCN)
        {
            bool flag = false;
            UCN = EncryptString(UCN);
            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; delete from People where ucn = @UCN;";
                    var cmd = new MySqlCommand(query, dbConn.Connection);
                    cmd.Parameters.AddWithValue("@UCN", UCN);

                    if (cmd.ExecuteNonQuery() > 0)
                    { flag = true; }
                }
                catch (Exception ex)
                {
                    ErrorLog.AddErrorLog(ex.ToString());
                    return false;
                }
                finally
                {
                    dbConn.Close();
                }
            }

            return flag;
        }
    }
}
