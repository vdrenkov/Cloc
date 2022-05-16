using Cloc.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using static Cloc.Classes.Security;
using static Cloc.Database.DatabaseInfo;
using static Cloc.Classes.FileHelper;

namespace Cloc.Database
{
    internal class DatabaseQuery
    {
        internal static bool StartupQuery(DBInfo db, Person person, string accessCode)
        {
            bool flag = false;
            string connectionString = $"server={db.Server};port={db.Port};user={db.UserID};password={db.Password};";

            try
            {
                var connection = new MySqlConnection(connectionString);
                var cmd = connection.CreateCommand();
                connection.Open();

                person.UCN = EncryptString(person.UCN);
                accessCode = HashString(accessCode);
                cmd.CommandText = "drop database if exists ClocDB; create database if not exists ClocDB; use ClocDB; " +
                    "create table if not exists People(ucn varchar(255) not null primary key unique," +
                    "name varchar(50),surname varchar(50),email varchar(50),phoneNumber varchar(20)," +
                    "country varchar(50),city varchar(50)," +
                    "address varchar(50), position varchar(20)); create table if not exists Users(userUcn varchar(255) not null unique primary key," +
                    "accessCode text, checkIn DateTime default Now(),checkOut DateTime default Now(), isCheckedIn boolean default false," +
                    "totalHours double(16,2) default 0, hourPayment double(16,2) default 0,percent double(16,2) default 0," +
                    "constraint foreign key(userUcn) references people(ucn) on delete cascade on update cascade); " +
                    "insert into People(ucn, name, surname, email, phoneNumber, country, city, address, position)" +
                    $" values('{person.UCN}', '{person.Name}', '{person.Surname}', '{person.Email}', '{person.PhoneNumber}', '{person.Country}'," +
                    $"'{person.City}', '{person.Address}', '{person.Position}'); insert into Users(userUcn, accessCode) values('{person.UCN}', '{accessCode}');";

                cmd.ExecuteNonQuery();
                connection.Close();

                FileCreator();

                person.UCN = DecryptString(person.UCN);

                User user = SelectUserQuery(person.UCN);

                if (user.UserUCN != null)
                {
                    flag = true;

                    if ((!Logger.AddLog(person.UCN, "Начална инициализация.")) || (!Checker.AddCheck(SelectUserQuery(person.UCN))))
                    {
                        MessageBox.Show("Възникна грешка при записване на активността.");
                    }

                    if (!SetSettings(db))
                    { MessageBox.Show("Възникна грешка при записване на данните за базата."); }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AddErrorLog(ex.ToString());
                return false;
            }

            return flag;
        }

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
                    User testUser = SelectUserQuery(testUCN);

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
    }
}