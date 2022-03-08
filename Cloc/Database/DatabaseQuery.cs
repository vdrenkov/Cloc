using Cloc.Classes;
using MySql.Data.MySqlClient;
using System;

namespace Cloc.Database
{
    internal class DatabaseQuery
    {

        public static bool StartupQuery(string server, string user, string port)
        {
            string connectionString = $"server={server};user={user};port={port};";
            var connection = new MySqlConnection(connectionString);
            var cmd = connection.CreateCommand();

            connection.Open();

            cmd.CommandText = "create database if not exists ClocDB; use ClocDB; create table if not exists People(ucn char(10) not null primary key unique," +
                "name varchar(50) not null,surname varchar(50) not null,email varchar(50) default null,phoneNumber varchar(20) default null,country varchar(50) not null,city varchar(50) not null," +
                "address varchar(50) default null, position varchar(10) not null); create table if not exists UsersInfo(userUcn char(10) not null unique primary key," +
                "accessCode char(5) not null, checkIn DateTime default null,hourPayment double default null,totalHours double default 0,percent double default 0, " +
                "constraint foreign key(userUcn) references people(ucn) on delete cascade on update cascade);" +
                " insert into People(ucn, name, surname, email, phoneNumber, country, city, address, position)" +
                " values('9902130044', 'Valentin', 'Drenkov', 'vdrenkov@tu-sofia.bg', '0888992278', 'Bulgaria', 'Razlog', 'Tsar Ivan Asen II 5', 'Boss');" +
                " insert into UsersInfo(userUcn, accessCode) values('9902130044', '77777'); ";
            cmd.ExecuteNonQuery();

            connection.Close();
            return true;
        }

        public static bool AddWorkerQuery(Person person, UserInfo user)
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

                string secondQuery = "use ClocDB; insert into UsersInfo (userUcn,accessCode,checkIn,checkOut,hourPayment,totalHours,percent) values" +
                    "(@userUcn,@accessCode,@checkIn,@checkOut,@hourPayment,@totalHours,@percent)";
                var command = new MySqlCommand(secondQuery, DBConn.Connection);

                command.Parameters.AddWithValue("@userUcn", user.UserUCN);
                command.Parameters.AddWithValue("@accessCode", user.AccessCode);
                command.Parameters.AddWithValue("@checkIn", user.CheckIn);
                command.Parameters.AddWithValue("@checkOut", user.CheckOut);
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

        public static bool ChangeCodeQuery(UserInfo user) // Излишна
        {
            var DBConn = DatabaseConnection.Instance();

            if (DBConn.IsConnect())
            {
                string query = "use ClocDB; update UsersInfo set accessCode = @accessCode where userUcn = @userUcn;";
                var cmd = new MySqlCommand(query, DBConn.Connection);

                cmd.Parameters.AddWithValue("@accessCode", user.AccessCode);
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

        public static bool DeleteWorkerQuery(Person person)
        {
            var DBConn = DatabaseConnection.Instance();

            if (DBConn.IsConnect())
            {
                string query = "use ClocDB; delete from People where ucn = @UCN;";
                var cmd = new MySqlCommand(query, DBConn.Connection);

                cmd.Parameters.AddWithValue("@UCN", person.UCN);
                cmd.ExecuteNonQuery();

                string secondQuery = "use ClocDB; delete from UsersInfo where userUcn = @userUcn;";
                var command = new MySqlCommand(secondQuery, DBConn.Connection);

                command.Parameters.AddWithValue("@userUcn", person.UCN);
                command.ExecuteNonQuery();

                DBConn.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckInQuery(UserInfo user)
        {
            var DBConn = DatabaseConnection.Instance();

            if (DBConn.IsConnect())
            {
                string query = "use ClocDB; update UsersInfo set checkIn = @checkIn where userUcn = @userUcn;";
                var cmd = new MySqlCommand(query, DBConn.Connection);

                cmd.Parameters.AddWithValue("@checkIn", user.CheckIn);
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

        public static bool ChangePeopleQuery(string UCN, string fieldParam, string changeParam)
        {
            var DBConn = DatabaseConnection.Instance();

            if (DBConn.IsConnect())
            {
                string query = $"use ClocDB; update People set {fieldParam} = @change where ucn = @UCN;";
                var cmd = new MySqlCommand(query, DBConn.Connection);

                cmd.Parameters.AddWithValue("@change", changeParam);
                cmd.Parameters.AddWithValue("@UCN", UCN);
                cmd.ExecuteNonQuery();

                DBConn.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ChangeUsersInfoQuery(string userUCN, string fieldParam, string changeParam)
        {
            var DBConn = DatabaseConnection.Instance();

            if (DBConn.IsConnect())
            {
                string query = $"use ClocDB; update UsersInfo set {fieldParam} = @change where userUcn = @userUcn;";
                var cmd = new MySqlCommand(query, DBConn.Connection);

                cmd.Parameters.AddWithValue("@change", changeParam);
                cmd.Parameters.AddWithValue("@userUcn", userUCN);
                cmd.ExecuteNonQuery();

                DBConn.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
