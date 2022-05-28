using Cloc.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Windows;
using static Cloc.Classes.Security;

namespace Cloc.Database
{
    internal class UpdateQuery
    {
        internal static bool ChangeAccessCodeQuery(string UCN, string accessCode)
        {
            bool flag = false;
            UCN = EncryptString(UCN);
            accessCode = HashString(accessCode);
            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; update Users set accessCode = @accessCode where userUcn =@UCN;";
                    var cmd = new MySqlCommand(query, dbConn.Connection);
                    cmd.Parameters.AddWithValue("@accessCode", accessCode);
                    cmd.Parameters.AddWithValue("@UCN", UCN);

                    if (cmd.ExecuteNonQuery() > 0)
                    { flag = true; }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Промяната на кода за достъп не беше успешна.");
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

        internal static bool CheckInQuery(User user)
        {
            bool flag = false;
            user.UserUCN = EncryptString(user.UserUCN);
            user.AccessCode = HashString(user.AccessCode);
            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; update Users set checkIn = '{user.CheckIn:yyyy-MM-dd HH:mm:ss}' where userUcn = @user.UserUCN;";
                    var cmd = new MySqlCommand(query, dbConn.Connection);
                    cmd.Parameters.AddWithValue("@user.UserUCN", user.UserUCN);

                    string secondQuery = $"use ClocDB; update Users set isCheckedIn = true where userUcn = @user.UserUCN;";
                    var command = new MySqlCommand(secondQuery, dbConn.Connection);
                    command.Parameters.AddWithValue("@user.UserUCN", user.UserUCN);

                    if (cmd.ExecuteNonQuery() > 0 && command.ExecuteNonQuery() > 0)
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
                    user.UserUCN = DecryptString(user.UserUCN);
                }
            }
            return flag;
        }

        internal static bool CheckOutQuery(User user)
        {
            bool flag = false;
            user.UserUCN = EncryptString(user.UserUCN);
            user.AccessCode = HashString(user.AccessCode);
            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {

                try
                {
                    string query = $"use ClocDB; update Users set checkOut = '{user.CheckOut:yyyy-MM-dd HH:mm:ss}' where userUcn = @user.UserUCN;";
                    var cmd = new MySqlCommand(query, dbConn.Connection);
                    cmd.Parameters.AddWithValue("@user.UserUCN", user.UserUCN);

                    string secondQuery = $"use ClocDB; update Users set isCheckedIn = false where userUcn = @user.UserUCN;";
                    var command = new MySqlCommand(secondQuery, dbConn.Connection);
                    command.Parameters.AddWithValue("@user.UserUCN", user.UserUCN);

                    if (cmd.ExecuteNonQuery() > 0 && command.ExecuteNonQuery() > 0)
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
                    user.UserUCN = DecryptString(user.UserUCN);
                }
            }
            return flag;
        }

        internal static bool ChangeTotalHoursQuery(User user)
        {
            bool flag = false;
            user.UserUCN = EncryptString(user.UserUCN);
            user.AccessCode = HashString(user.AccessCode);
            double totalHours = Math.Round((user.CheckOut - user.CheckIn).TotalHours + user.TotalHours, 2);
            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; update Users set totalHours = @totalHours where userUcn = @user.UserUCN;";
                    var cmd = new MySqlCommand(query, dbConn.Connection);
                    cmd.Parameters.AddWithValue("@totalHours", totalHours);
                    cmd.Parameters.AddWithValue("@user.UserUCN", user.UserUCN);

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
                    user.UserUCN = DecryptString(user.UserUCN);
                }
            }
            return flag;
        }

        internal static bool NullTotalHoursQuery(User user)
        {
            bool flag = false;
            user.UserUCN = EncryptString(user.UserUCN);
            user.AccessCode = HashString(user.AccessCode);
            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; update Users set totalHours = 0 where userUcn = @user.UserUCN;";
                    var cmd = new MySqlCommand(query, dbConn.Connection);
                    cmd.Parameters.AddWithValue("@user.UserUCN", user.UserUCN);

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
                    user.UserUCN = DecryptString(user.UserUCN);
                }
            }
            return flag;
        }

        internal static bool ChangeHourPaymentQuery(string userUCN, double hourPayment)
        {
            bool flag = false;
            userUCN = EncryptString(userUCN);
            hourPayment = Math.Round(hourPayment, 2);
            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; update Users set hourPayment = @hourPayment where userUcn = @userUCN;";
                    var cmd = new MySqlCommand(query, dbConn.Connection);
                    cmd.Parameters.AddWithValue("@hourPayment", hourPayment);
                    cmd.Parameters.AddWithValue("@userUCN", userUCN);

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

        internal static bool ChangePercentQuery(string userUCN, double percent)
        {
            bool flag = false;
            userUCN = EncryptString(userUCN);
            percent = Math.Round(percent, 2);
            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; update Users set percent = @percent where userUcn =@userUCN;";
                    var cmd = new MySqlCommand(query, dbConn.Connection);
                    cmd.Parameters.AddWithValue("@percent", percent);
                    cmd.Parameters.AddWithValue("@userUCN", userUCN);

                    if (cmd.ExecuteNonQuery() > 0)
                    { flag = true; }
                }
                catch (Exception ex)
                {
                    ErrorLog.AddErrorLog(ex.ToString());
                    return false;
                }
                finally { dbConn.Close(); }
            }
            return flag;
        }

        internal static bool ChangePersonQuery(string UCN, string fieldParam, string changeParam)
        {
            bool flag = false;
            UCN = EncryptString(UCN);
            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; update People set {fieldParam} = @changeParam where ucn = @UCN;";
                    var cmd = new MySqlCommand(query, dbConn.Connection);
                    cmd.Parameters.AddWithValue("@changeParam", changeParam);
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
