using Cloc.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using static Cloc.Classes.Security;

namespace Cloc.Database
{
    internal class SelectQuery
    {
        internal static Person SelectPersonQuery(string UCN)
        {
            UCN = EncryptString(UCN);
            DatabaseConnection dbConn = new();
            Person person = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; select * from People where ucn=@UCN;";
                    var cmd = new MySqlCommand(query, dbConn.Connection);
                    cmd.Parameters.AddWithValue("@UCN", UCN);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        person.UCN = reader.GetString(0);
                        person.Name = reader.GetString(1);
                        person.Surname = reader.GetString(2);
                        person.Email = reader.GetString(3);
                        person.PhoneNumber = reader.GetString(4);
                        person.Country = reader.GetString(5);
                        person.City = reader.GetString(6);
                        person.Address = reader.GetString(7);
                        person.Position = (WorkPosition)Enum.Parse(typeof(WorkPosition), reader.GetString(8));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Възникна неочаквана грешка при обработка на заявката.");
                    ErrorLog.AddErrorLog(ex.ToString());
                    return new Person();
                }
                finally
                {
                    dbConn.Close();
                }
            }

            if (person.UCN != null)
            {
                person.UCN = DecryptString(person.UCN);
                return person;
            }
            else
            {
                return new Person();
            }
        }

        internal static List<Person> SelectAllPeopleQuery()
        {
            DatabaseConnection dbConn = new();
            Person person = new();
            List<Person> people = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; select * from People;";
                    var cmd = new MySqlCommand(query, dbConn.Connection);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        person.UCN = reader.GetString(0);
                        person.Name = reader.GetString(1);
                        person.Surname = reader.GetString(2);
                        person.Email = reader.GetString(3);
                        person.PhoneNumber = reader.GetString(4);
                        person.Country = reader.GetString(5);
                        person.City = reader.GetString(6);
                        person.Address = reader.GetString(7);
                        person.Position = (WorkPosition)Enum.Parse(typeof(WorkPosition), reader.GetString(8));

                        person.UCN = DecryptString(person.UCN);
                        people.Add(person);
                        person = new Person();
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Възникна неочаквана грешка при обработка на заявката.");
                    ErrorLog.AddErrorLog(ex.ToString());
                    return new List<Person>();
                }
                finally
                {
                    dbConn.Close();
                }
            }

            if (people.Count > 0)
            { return people; }
            else
            { return new List<Person>(); }
        }

        internal static User SelectUserQuery(string UserUCN)
        {
            UserUCN = EncryptString(UserUCN);
            DatabaseConnection dbConn = new();
            User user = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; select * from Users where userUcn=@UserUCN;";
                    var cmd = new MySqlCommand(query, dbConn.Connection);
                    cmd.Parameters.AddWithValue("@UserUCN", UserUCN);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        user.UserUCN = reader.GetString(0);
                        user.AccessCode = reader.GetString(1);
                        user.CheckIn = reader.GetDateTime(2);
                        user.CheckOut = reader.GetDateTime(3);
                        user.IsCheckedIn = reader.GetBoolean(4);
                        user.TotalHours = reader.GetDouble(5);
                        user.HourPayment = reader.GetDouble(6);
                        user.Percent = reader.GetDouble(7);

                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Възникна неочаквана грешка при обработка на заявката.");
                    ErrorLog.AddErrorLog(ex.ToString());
                    return new User();
                }
                finally
                {
                    dbConn.Close();
                }
            }

            if (user.UserUCN != null)
            {
                user.UserUCN = DecryptString(user.UserUCN);
                return user;
            }
            else
            { return new User(); }
        }

        internal static User SelectUserByAccessCodeQuery(string accessCode)
        {
            accessCode = HashString(accessCode);
            DatabaseConnection dbConn = new();
            User user = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; select * from Users where accessCode=@accessCode;";
                    var cmd = new MySqlCommand(query, dbConn.Connection);
                    cmd.Parameters.AddWithValue("@accessCode", accessCode);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        user.UserUCN = reader.GetString(0);
                        user.AccessCode = reader.GetString(1);
                        user.CheckIn = reader.GetDateTime(2);
                        user.CheckOut = reader.GetDateTime(3);
                        user.IsCheckedIn = reader.GetBoolean(4);
                        user.TotalHours = reader.GetDouble(5);
                        user.HourPayment = reader.GetDouble(6);
                        user.Percent = reader.GetDouble(7);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Възникна неочаквана грешка при обработка на заявката.");
                    ErrorLog.AddErrorLog(ex.ToString());
                    return new User();
                }
                finally
                {
                    dbConn.Close();
                }
            }

            if (user.UserUCN != null)
            {
                user.UserUCN = DecryptString(user.UserUCN);
                return user;
            }
            else
            {
                return new User();
            }
        }

        internal static bool SelectAccessCodeQuery(string accessCode)
        {
            bool flag = false;
            string DBAccessCode = string.Empty;
            accessCode = HashString(accessCode);
            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; select accessCode from Users where accessCode=@accessCode;";
                    var cmd = new MySqlCommand(query, dbConn.Connection);
                    cmd.Parameters.AddWithValue("@accessCode", accessCode);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        DBAccessCode = reader.GetString(0);
                    }

                    if (!string.IsNullOrEmpty(DBAccessCode))
                    {
                        flag = true;
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Възникна неочаквана грешка при обработка на заявката.");
                    ErrorLog.AddErrorLog(ex.ToString());
                    return false;
                }
                finally { dbConn.Close(); }
            }

            return flag;
        }

        internal static List<string> SelectAllLogsQuery(string UCN)
        {
            UCN = EncryptString(UCN);
            DatabaseConnection dbConn = new();
            List<string> allLogs = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; select * from Logs where userUcn=@UCN;";
                    var cmd = new MySqlCommand(query, dbConn.Connection);
                    cmd.Parameters.AddWithValue("@UCN", UCN);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        allLogs.Add(reader.GetString(1) + ";" + reader.GetString(2) + ";" + reader.GetDateTime(3));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Възникна неочаквана грешка при обработка на заявката.");
                    ErrorLog.AddErrorLog(ex.ToString());
                    return new List<string>();
                }
                finally
                {
                    dbConn.Close();
                }
            }
            if (allLogs.Count > 0)
            {
                return allLogs;
            }
            else
            {
                return new List<string>();
            }
        }

        internal static List<string> SelectAllChecksQuery(string UCN)
        {
            UCN = EncryptString(UCN);
            DatabaseConnection dbConn = new();
            List<string> allLogs = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; select * from Checks where userUcn=@UCN;";
                    var cmd = new MySqlCommand(query, dbConn.Connection);
                    cmd.Parameters.AddWithValue("@UCN", UCN);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        StringBuilder sb = new();

                        sb.Append(reader.GetString(1));
                        sb.Append(';');
                        sb.Append(reader.GetDateTime(2));
                        sb.Append(';');
                        sb.Append(reader.GetDateTime(3));

                        allLogs.Add(sb.ToString());
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Възникна неочаквана грешка при обработка на заявката.");
                    ErrorLog.AddErrorLog(ex.ToString());
                    return new List<string>();
                }
                finally
                {
                    dbConn.Close();
                }
            }
            if (allLogs.Count > 0)
            {
                return allLogs;
            }
            else
            {
                return new List<string>();
            }
        }

        internal static List<string> SelectAllReportsQuery(string UCN, bool isAll)
        {
            UCN = EncryptString(UCN);
            DatabaseConnection dbConn = new();
            List<string> allLogs = new();
            string query;

            if (dbConn.IsConnect())
            {
                try
                {
                    if (isAll)
                    {
                        query = $"use ClocDB; select * from Reports;";
                    }
                    else
                    {
                        query = $"use ClocDB; select * from Reports where userUcn=@UCN;";
                    }

                    var cmd = new MySqlCommand(query, dbConn.Connection);
                    cmd.Parameters.AddWithValue("@UCN", UCN);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        allLogs.Add(reader.GetString(1) + ";" + reader.GetString(2) + ";" + reader.GetDouble(3) + ";" + reader.GetDateTime(4));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Възникна неочаквана грешка при обработка на заявката.");
                    ErrorLog.AddErrorLog(ex.ToString());
                    return new List<string>();
                }
                finally
                {
                    dbConn.Close();
                }
            }
            if (allLogs.Count > 0)
            {
                return allLogs;
            }
            else
            {
                return new List<string>();
            }
        }
    }
}
