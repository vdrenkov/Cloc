using Cloc.Classes;
using MySql.Data.MySqlClient;
using System;
using static Cloc.Classes.Security;

namespace Cloc.Database
{
    internal class InsertQuery
    {
        internal static bool AddWorkerQuery(Person person, User user)
        {
            bool flag = false;
            string testUCN = person.UCN;
            person.UCN = EncryptString(person.UCN);
            user.UserUCN = person.UCN;
            user.AccessCode = HashString(user.AccessCode);

            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    User testUser = SelectQuery.SelectUserQuery(testUCN);

                    if (testUser.UserUCN != null)
                    {
                        return false;
                    }

                    string query = "use ClocDB; insert into People (ucn,name,surname,email,phoneNumber,country,city,address,position) values" +
                       $"(@UCN,@Name,@Surname,@Email,@PhoneNumber,@Country,@City,@Address,@Position);";
                    var cmd = new MySqlCommand(query, dbConn.Connection);
                    cmd.Parameters.AddWithValue("@UCN", person.UCN);
                    cmd.Parameters.AddWithValue("@Name", person.Name);
                    cmd.Parameters.AddWithValue("@Surname", person.Surname);
                    cmd.Parameters.AddWithValue("@Email", person.Email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", person.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Country", person.Country);
                    cmd.Parameters.AddWithValue("@City", person.City);
                    cmd.Parameters.AddWithValue("@Address", person.Address);
                    cmd.Parameters.AddWithValue("@Position", person.Position.ToString());

                    string secondQuery = "use ClocDB; insert into Users (userUcn,accessCode,checkIn,checkOut,isCheckedIn,hourPayment,totalHours,percent) values" +
                        $"(@UserUCN,@AccessCode,'{user.CheckIn:yyyy-MM-dd HH:mm:ss}','{user.CheckOut:yyyy-MM-dd HH:mm:ss}',@IsCheckedIn,@HourPayment,@TotalHours,@Percent);";
                    var command = new MySqlCommand(secondQuery, dbConn.Connection);
                    command.Parameters.AddWithValue("@UserUCN", user.UserUCN);
                    command.Parameters.AddWithValue("@AccessCode", user.AccessCode);
                    command.Parameters.AddWithValue("@IsCheckedIn", user.IsCheckedIn);
                    command.Parameters.AddWithValue("@HourPayment", Math.Round(user.HourPayment, 2));
                    command.Parameters.AddWithValue("@TotalHours", Math.Round(user.TotalHours, 2));
                    command.Parameters.AddWithValue("@Percent", Math.Round(user.Percent, 2));

                    if (cmd.ExecuteNonQuery() > 0 && command.ExecuteNonQuery() > 0)
                    { flag = true; }
                    if (person.UCN != null)
                    {
                        person.UCN = DecryptString(person.UCN);
                        user.UserUCN = person.UCN;
                    }
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

        internal static bool AddLogQuery(string ucn, string action)
        {
            bool flag = false;
            ucn = EncryptString(ucn);

            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = "use ClocDB; insert into Logs (userUcn,action) values (@UCN, @action);";
                    var cmd = new MySqlCommand(query, dbConn.Connection);
                    cmd.Parameters.AddWithValue("@UCN", ucn);
                    cmd.Parameters.AddWithValue("@action", action);

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

        internal static bool AddCheckQuery(User user)
        {
            bool flag = false;
            user.UserUCN = EncryptString(user.UserUCN);

            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = "use ClocDB; insert into Checks (userUcn,checkIn,checkOut) values" +
                       $"(@UCN,'{user.CheckIn:yyyy-MM-dd HH:mm:ss}','{user.CheckOut:yyyy-MM-dd HH:mm:ss}');";
                    var cmd = new MySqlCommand(query, dbConn.Connection);
                    cmd.Parameters.AddWithValue("@UCN", user.UserUCN);

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

        internal static bool AddReportQuery(string ucn, string names, double sum)
        {
            bool flag = false;
            ucn = EncryptString(ucn);

            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = "use ClocDB; insert into Reports (userUcn,names,sum) values (@UCN,@names,@sum);";
                    var cmd = new MySqlCommand(query, dbConn.Connection);
                    cmd.Parameters.AddWithValue("@UCN", ucn);
                    cmd.Parameters.AddWithValue("@names", names);
                    cmd.Parameters.AddWithValue("@sum", sum);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        flag = true;
                    }
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
