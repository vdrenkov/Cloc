﻿using Cloc.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
using static Cloc.Classes.Security;
using static Cloc.Settings.SystemSetup;

namespace Cloc.Database
{
    internal class DatabaseQuery
    {
        public static bool StartupQuery(string server, string username, string password, string port, Person person, string accessCode)
        {
            bool flag = false;
            string connectionString = $"server={server};user={username};password={password}; port={port};";
            var connection = new MySqlConnection(connectionString);
            var cmd = connection.CreateCommand();

            try
            {
                connection.Open();
                person.UCN = EncryptString(person.UCN);
                accessCode = HashString(accessCode);
                cmd.CommandText = "drop database if exists ClocDB; create database if not exists ClocDB; use ClocDB; " +
                    "create table if not exists People(ucn varchar(255) not null primary key unique," +
                    "name varchar(50),surname varchar(50),email varchar(50),phoneNumber varchar(20)," +
                    "country varchar(50),city varchar(50)," +
                    "address varchar(50), position varchar(20)); create table if not exists Users(userUcn varchar(255) not null unique primary key," +
                    "accessCode text, checkIn DateTime default Now(),checkOut DateTime default Now(), isCheckedIn boolean default false," +
                    "totalHours double default 0, hourPayment double default 0,percent double default 0," +
                    "constraint foreign key(userUcn) references people(ucn) on delete cascade on update cascade); " +
                    "insert into People(ucn, name, surname, email, phoneNumber, country, city, address, position)" +
                    $" values('{person.UCN}', '{person.Name}', '{person.Surname}', '{person.Email}', '{person.PhoneNumber}', '{person.Country}'," +
                    $"'{person.City}', '{person.Address}', '{person.Position}'); insert into Users(userUcn, accessCode) values('{person.UCN}', '{accessCode}');";

                cmd.ExecuteNonQuery();
                connection.Close();

                person.UCN = DecryptString(person.UCN);
                User user = SelectUserQuery(person.UCN);

                if (user.UserUCN != null && SetSettings(server, username, password, port) && Logger.AddLog(person.UCN, "Начална инициализация.") && Checker.AddCheck(SelectUserQuery(person.UCN)))
                {
                    flag = true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return flag;
        }

        public static bool AddWorkerQuery(Person person, User user)
        {
            bool flag = false;
            person.UCN = EncryptString(person.UCN);
            user.UserUCN = person.UCN;
            user.AccessCode = HashString(user.AccessCode);

            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    User testUser = SelectUserQuery(DecryptString(person.UCN));
                    if (testUser.UserUCN != null)
                    { return false; }

                    string query = "use ClocDB; insert into People (ucn,name,surname,email,phoneNumber,country,city,address,position) values" +
                       $"('{person.UCN}','{person.Name}','{person.Surname}','{person.Email}','{person.PhoneNumber}','{person.Country}','{person.City}','{person.Address}','{person.Position}');";
                    var cmd = new MySqlCommand(query, dbConn.Connection);

                    string secondQuery = "use ClocDB; insert into Users (userUcn,accessCode,checkIn,checkOut,isCheckedIn,hourPayment,totalHours,percent) values" +
                        $"('{user.UserUCN}','{user.AccessCode}','{user.CheckIn:yyyy-MM-dd HH:mm:ss}','{user.CheckOut:yyyy-MM-dd HH:mm:ss}',{user.IsCheckedIn},{user.HourPayment},{user.TotalHours},{user.Percent});";
                    var command = new MySqlCommand(secondQuery, dbConn.Connection);

                    if (cmd.ExecuteNonQuery() > 0 && command.ExecuteNonQuery() > 0)
                    { flag = true; }
                    dbConn.Close();
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    person.UCN = DecryptString(person.UCN);
                    user.UserUCN = person.UCN;
                }
            }
            return flag;
        }

        public static bool DeleteWorkerQuery(string UCN)
        {
            bool flag = false;
            UCN = EncryptString(UCN);
            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; delete from People where ucn = '{UCN}';";
                    var cmd = new MySqlCommand(query, dbConn.Connection);

                    if (cmd.ExecuteNonQuery() > 0)
                    { flag = true; }
                    dbConn.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return flag;
        }

        public static bool ChangeAccessCodeQuery(string UCN, string accessCode)
        {
            bool flag = false;
            UCN = EncryptString(UCN);
            accessCode = HashString(accessCode);
            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; update Users set accessCode = '{accessCode}' where userUcn ='{UCN}';";
                    var cmd = new MySqlCommand(query, dbConn.Connection);
                    if (cmd.ExecuteNonQuery() > 0)
                    { flag = true; }
                    dbConn.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Промяната на кода за достъп не беше успешна.");
                }
            }
            return flag;
        }

        public static bool CheckInQuery(User user)
        {
            bool flag = false;
            user.UserUCN = EncryptString(user.UserUCN);
            user.AccessCode = HashString(user.AccessCode);
            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; update Users set checkIn = '{user.CheckIn:yyyy-MM-dd HH:mm:ss}' where userUcn = '{user.UserUCN}';";
                    var cmd = new MySqlCommand(query, dbConn.Connection);

                    string secondQuery = $"use ClocDB; update Users set isCheckedIn = true where userUcn = '{user.UserUCN}';";
                    var command = new MySqlCommand(secondQuery, dbConn.Connection);

                    if (cmd.ExecuteNonQuery() > 0 && command.ExecuteNonQuery() > 0)
                    { flag = true; }
                    dbConn.Close();
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    user.UserUCN = DecryptString(user.UserUCN);
                }
            }
            return flag;
        }

        public static bool CheckOutQuery(User user)
        {
            bool flag = false;
            user.UserUCN = EncryptString(user.UserUCN);
            user.AccessCode = HashString(user.AccessCode);
            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {

                try
                {
                    string query = $"use ClocDB; update Users set checkOut = '{user.CheckOut:yyyy-MM-dd HH:mm:ss}' where userUcn = '{user.UserUCN}';";
                    var cmd = new MySqlCommand(query, dbConn.Connection);

                    string secondQuery = $"use ClocDB; update Users set isCheckedIn = false where userUcn = '{user.UserUCN}';";
                    var command = new MySqlCommand(secondQuery, dbConn.Connection);

                    if (cmd.ExecuteNonQuery() > 0 && command.ExecuteNonQuery() > 0)
                    { flag = true; }
                    dbConn.Close();
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    user.UserUCN = DecryptString(user.UserUCN);
                }
            }
            return flag;
        }

        public static bool ChangeTotalHoursQuery(User user)
        {
            bool flag = false;
            user.UserUCN = EncryptString(user.UserUCN);
            user.AccessCode = HashString(user.AccessCode);
            TimeSpan span = user.CheckOut.Subtract(user.CheckIn);
            double totalHours = Math.Round((span.Hours + user.TotalHours), 4);

            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; update Users set totalHours = '{totalHours}' where userUcn = '{user.UserUCN}';";
                    var cmd = new MySqlCommand(query, dbConn.Connection);

                    if (cmd.ExecuteNonQuery() > 0)
                    { flag = true; }
                    dbConn.Close();
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    user.UserUCN = DecryptString(user.UserUCN);
                }
            }
            return flag;
        }

        public static bool ChangeHourPaymentQuery(string userUCN, double hourPayment)
        {
            bool flag = false;
            userUCN = EncryptString(userUCN);
            hourPayment = Math.Round(hourPayment, 4);
            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; update Users set hourPayment = {hourPayment} where userUcn = '{userUCN}';";
                    var cmd = new MySqlCommand(query, dbConn.Connection);

                    if (cmd.ExecuteNonQuery() > 0)
                    { flag = true; }
                    dbConn.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return flag;
        }

        public static bool ChangePercentQuery(string userUCN, double percent)
        {
            bool flag = false;
            userUCN = EncryptString(userUCN);
            percent = Math.Round(percent, 4);
            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; update Users set percent = {percent} where userUcn ='{userUCN}';";
                    var cmd = new MySqlCommand(query, dbConn.Connection);

                    if (cmd.ExecuteNonQuery() > 0)
                    { flag = true; }
                    dbConn.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return flag;
        }

        public static bool ChangePersonQuery(string UCN, string fieldParam, string changeParam)
        {
            bool flag = false;
            UCN = EncryptString(UCN);
            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; update People set {fieldParam} = '{changeParam}' where ucn = '{UCN}';";
                    var cmd = new MySqlCommand(query, dbConn.Connection);

                    if (cmd.ExecuteNonQuery() > 0)
                    { flag = true; }
                    dbConn.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return flag;
        }

        public static Person SelectPersonQuery(string UCN)
        {
            UCN = EncryptString(UCN);
            DatabaseConnection dbConn = new();
            Person person = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; select * from People where ucn='{UCN}';";
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
                    }

                    reader.Close();
                    dbConn.Close();
                    person.UCN = DecryptString(person.UCN);
                }
                catch (Exception)
                {
                    MessageBox.Show("Възникна неочаквана грешка при обработка на заявката.");
                }
            }
            return person;
        }

        public static List<Person> SelectAllPeopleQuery()
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
                    dbConn.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Възникна неочаквана грешка при обработка на заявката.");
                }
            }
            return people;
        }

        public static User SelectUserQuery(string UserUCN)
        {
            UserUCN = EncryptString(UserUCN);
            DatabaseConnection dbConn = new();
            User user = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; select * from Users where userUcn='{UserUCN}';";
                    var cmd = new MySqlCommand(query, dbConn.Connection);

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
                    dbConn.Close();
                    user.UserUCN = DecryptString(user.UserUCN);
                }
                catch (Exception)
                {
                    MessageBox.Show("Възникна неочаквана грешка при обработка на заявката.");
                }
            }
            return user;
        }

        public static User SelectUserByAccessCodeQuery(string accessCode)
        {
            accessCode = HashString(accessCode);
            DatabaseConnection dbConn = new();
            User user = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; select * from Users where accessCode='{accessCode}';";
                    var cmd = new MySqlCommand(query, dbConn.Connection);

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
                    dbConn.Close();
                    user.UserUCN = DecryptString(user.UserUCN);
                }
                catch (Exception)
                {
                    MessageBox.Show("Възникна неочаквана грешка при обработка на заявката.");
                }
            }
            return user;
        }

        public static bool SelectAccessCodeQuery(string accessCode)
        {
            bool flag = false;
            string DBAccessCode = null;
            accessCode = HashString(accessCode);
            DatabaseConnection dbConn = new();

            if (dbConn.IsConnect())
            {
                try
                {
                    string query = $"use ClocDB; select accessCode from Users where accessCode='{accessCode}';";
                    var cmd = new MySqlCommand(query, dbConn.Connection);

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
                    dbConn.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Възникна неочаквана грешка при обработка на заявката.");
                }
            }

            return flag;
        }
    }
}