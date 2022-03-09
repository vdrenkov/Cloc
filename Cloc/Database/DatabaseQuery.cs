using Cloc.Classes;
using MySql.Data.MySqlClient;
using System;

namespace Cloc.Database
{
    internal class DatabaseQuery
    {

        public static bool StartupQuery(string server, string user, string password, string port)
        {
            string connectionString = $"server={server};user={user};password={password}; port={port};";
            var connection = new MySqlConnection(connectionString);
            var cmd = connection.CreateCommand();

            connection.Open();

            cmd.CommandText = "drop database if exists ClocDB; create database if not exists ClocDB; use ClocDB; create table if not exists People(ucn char(10) not null primary key unique," +
                "name varchar(50) not null,surname varchar(50) not null,email varchar(50) not null,phoneNumber varchar(20) not null,country varchar(50) not null,city varchar(50) not null," +
                "address varchar(50) not null, position varchar(10) not null); create table if not exists Users(userUcn char(10) not null unique primary key," +
                "accessCode char(5) not null, checkIn DateTime not null default Now(),checkOut DateTime not null default Now(), isCheckedIn boolean default false not null, totalHours double default 0 not null," +
                "hourPayment double not null default 0,percent double not null default 0, " +
                "constraint foreign key(userUcn) references people(ucn) on delete cascade on update cascade);" +
                " insert into People(ucn, name, surname, email, phoneNumber, country, city, address, position)" +
                " values('9902130044', 'Valentin', 'Drenkov', 'vdrenkov@tu-sofia.bg', '0888992278', 'Bulgaria', 'Razlog', 'Tsar Ivan Asen II 5', 'Boss');" +
                " insert into Users(userUcn, accessCode) values('9902130044', '77777'); ";
            cmd.ExecuteNonQuery();

            connection.Close();
            return true;
        }

        public static bool AddWorkerQuery(Person person, User user)
        {
            var DBConn = DatabaseConnection.Instance();

            if (DBConn.IsConnect())
            {
                string query = "use ClocDB; insert into People (ucn,name,surname,email,phoneNumber,country,city,address,position) values" +
                   "(@ucn,@name,@surname,@email,@phoneNumber,@country,@city,@address,@position)";
                var cmd = new MySqlCommand(query, DBConn.Connection);

                cmd.Parameters.AddWithValue("@ucn", person.UCN);
                cmd.Parameters.AddWithValue("@name", person.Name);
                cmd.Parameters.AddWithValue("@surname", person.Surname);
                cmd.Parameters.AddWithValue("@email", person.Email);
                cmd.Parameters.AddWithValue("@phoneNumber", person.PhoneNumber);
                cmd.Parameters.AddWithValue("@country", person.Country);
                cmd.Parameters.AddWithValue("@city", person.City);
                cmd.Parameters.AddWithValue("@address", person.Address);
                cmd.Parameters.AddWithValue("@position", person.Position.ToString());
                cmd.ExecuteNonQuery();

                string secondQuery = "use ClocDB; insert into Users (userUcn,accessCode,checkIn,checkOut,isCheckedIn,hourPayment,totalHours,percent) values" +
                    "(@userUcn,@accessCode,@checkIn,@checkOut,@isCheckedIn,@hourPayment,@totalHours,@percent)";
                var command = new MySqlCommand(secondQuery, DBConn.Connection);

                command.Parameters.AddWithValue("@userUcn", user.UserUCN);
                command.Parameters.AddWithValue("@accessCode", user.AccessCode);
                command.Parameters.AddWithValue("@checkIn", user.CheckIn);
                command.Parameters.AddWithValue("@checkOut", user.CheckOut);
                command.Parameters.AddWithValue("@isCheckedIn", user.IsCheckedIn);
                command.Parameters.AddWithValue("@hourPayment", user.HourPayment);
                command.Parameters.AddWithValue("@totalHours", user.TotalHours);
                command.Parameters.AddWithValue("@percent", user.Percent);
                command.ExecuteNonQuery();

                DBConn.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool DeleteWorkerQuery(string UCN)
        {
            var DBConn = DatabaseConnection.Instance();

            if (DBConn.IsConnect())
            {
                string query = $"use ClocDB; delete from People where ucn = {UCN};";
                var cmd = new MySqlCommand(query, DBConn.Connection);

                cmd.ExecuteNonQuery();

                string secondQuery = $"use ClocDB; delete from Users where userUcn = {UCN};";
                var command = new MySqlCommand(secondQuery, DBConn.Connection);

                command.ExecuteNonQuery();

                DBConn.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ChangeAccessCodeQuery(string UCN, string accessCode)
        {
            var DBConn = DatabaseConnection.Instance();

            if (DBConn.IsConnect())
            {
                string query = $"use ClocDB; update Users set accessCode = '{accessCode}' where userUcn ={UCN};";
                var cmd = new MySqlCommand(query, DBConn.Connection);

                cmd.ExecuteNonQuery();

                DBConn.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckInQuery(User user)
        {
            var DBConn = DatabaseConnection.Instance();

            if (DBConn.IsConnect())
            {
                string query = "use ClocDB; update Users set checkIn = @checkIn where userUcn = @userUcn;";
                var cmd = new MySqlCommand(query, DBConn.Connection);

                cmd.Parameters.AddWithValue("@checkIn", user.CheckIn);
                cmd.Parameters.AddWithValue("@userUcn", user.UserUCN);
                cmd.ExecuteNonQuery();

                string secondQuery = "use ClocDB; update Users set isCheckedIn = true where userUcn = @userUcn;";
                var command = new MySqlCommand(secondQuery, DBConn.Connection);

                command.Parameters.AddWithValue("@userUcn", user.UserUCN);
                command.ExecuteNonQuery();

                DBConn.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckOutQuery(User user)
        {
            var DBConn = DatabaseConnection.Instance();

            if (DBConn.IsConnect())
            {
                string query = "use ClocDB; update Users set checkOut = @checkOut where userUcn = @userUcn;";
                var cmd = new MySqlCommand(query, DBConn.Connection);

                cmd.Parameters.AddWithValue("@checkOut", user.CheckOut);
                cmd.Parameters.AddWithValue("@userUcn", user.UserUCN);
                cmd.ExecuteNonQuery();

                string secondQuery = "use ClocDB; update Users set isCheckedIn = false where userUcn = @userUcn;";
                var command = new MySqlCommand(secondQuery, DBConn.Connection);

                command.Parameters.AddWithValue("@userUcn", user.UserUCN);
                command.ExecuteNonQuery();

                DBConn.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ChangeTotalHoursQuery(User user)
        {
            double totalHours = (user.CheckOut - user.CheckIn).TotalHours;
            var DBConn = DatabaseConnection.Instance();

            if (DBConn.IsConnect())
            {
                string query = $"use ClocDB; update Users set totalHours = @totalHours where userUcn =@userUcn;";
                var cmd = new MySqlCommand(query, DBConn.Connection);

                cmd.Parameters.AddWithValue("@totalHours", totalHours);
                cmd.Parameters.AddWithValue("@userUcn", user.UserUCN);
                cmd.ExecuteNonQuery();

                DBConn.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ChangeHourPaymentQuery(string userUCN, double hourPayment)
        {
            var DBConn = DatabaseConnection.Instance();

            if (DBConn.IsConnect())
            {
                string query = $"use ClocDB; update Users set hourPayment = {hourPayment} where userUcn ={userUCN};";
                var cmd = new MySqlCommand(query, DBConn.Connection);

                cmd.ExecuteNonQuery();

                DBConn.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ChangePercentQuery(string userUCN, double percent)
        {
            var DBConn = DatabaseConnection.Instance();

            if (DBConn.IsConnect())
            {
                string query = $"use ClocDB; update Users set percent = {percent} where userUcn ={userUCN};";
                var cmd = new MySqlCommand(query, DBConn.Connection);

                cmd.ExecuteNonQuery();

                DBConn.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ChangePersonQuery(string UCN, string fieldParam, string changeParam)
        {
            var DBConn = DatabaseConnection.Instance();

            if (DBConn.IsConnect())
            {
                string query = $"use ClocDB; update People set {fieldParam} = '{changeParam}' where ucn = {UCN};";
                var cmd = new MySqlCommand(query, DBConn.Connection);

                cmd.ExecuteNonQuery();

                DBConn.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Person SelectPersonQuery(string UCN)
        {
            var DBConn = DatabaseConnection.Instance();
            Person person = new Person();

            if (DBConn.IsConnect())
            {
                string query = $"use ClocDB; select * from People where ucn={UCN}";
                var cmd = new MySqlCommand(query, DBConn.Connection);

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
                DBConn.Close();
            }
            return person;
        }

        public static User SelectUserQuery(string UserUCN)
        {
            var DBConn = DatabaseConnection.Instance();
            User user = new User();

            if (DBConn.IsConnect())
            {
                string query = $"use ClocDB; select * from Users where userUcn={UserUCN}";
                var cmd = new MySqlCommand(query, DBConn.Connection);

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
                DBConn.Close();
            }
            return user;
        }
    }
}
