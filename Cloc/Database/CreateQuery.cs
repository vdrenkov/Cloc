using Cloc.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Windows;
using static Cloc.Classes.FileHelper;
using static Cloc.Classes.Security;
using static Cloc.Database.DatabaseInfo;
using static Cloc.Database.InsertQuery;

namespace Cloc.Database
{
    internal class CreateQuery
    {
        internal static bool StartupQuery(DatabaseParameters db, Person person, string accessCode)
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

                if (SetSettings(db))
                {
                    User user = SelectQuery.SelectUserQuery(person.UCN);

                    if (user.UserUCN != null)
                    {
                        if (LogTableCreationQuery(db) && CheckTableCreationQuery(db) && ReportTableCreationQuery(db))
                        {
                            flag = true;

                            if ((!AddLogQuery(person.UCN, "Начална инициализация.")))
                            {
                                MessageBox.Show("Възникна грешка при записване на активността.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AddErrorLog(ex.ToString());
                return false;
            }

            return flag;
        }

        internal static bool LogTableCreationQuery(DatabaseParameters db)
        {
            string connectionString = $"server={db.Server};port={db.Port};user={db.UserID};password={db.Password};";

            try
            {
                var connection = new MySqlConnection(connectionString);
                var cmd = connection.CreateCommand();
                connection.Open();

                cmd.CommandText = "use clocdb; create table if not exists Logs(id int primary key auto_increment, userUcn varchar(255) not null," +
                    "action text, dt DateTime default Now()," +
                    "constraint foreign key(userUcn) references people(ucn) on delete cascade on update cascade);";

                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                ErrorLog.AddErrorLog(ex.ToString());
                return false;
            }

            return true;
        }

        internal static bool CheckTableCreationQuery(DatabaseParameters db)
        {
            string connectionString = $"server={db.Server};port={db.Port};user={db.UserID};password={db.Password};";

            try
            {
                var connection = new MySqlConnection(connectionString);
                var cmd = connection.CreateCommand();
                connection.Open();

                cmd.CommandText = "use clocdb; create table if not exists Checks(id int primary key auto_increment, userUcn varchar(255) not null," +
                    "checkIn DateTime default Now(), checkOut DateTime default Now()," +
                    "constraint foreign key(userUcn) references people(ucn) on delete cascade on update cascade);";

                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                ErrorLog.AddErrorLog(ex.ToString());
                return false;
            }

            return true;
        }

        internal static bool ReportTableCreationQuery(DatabaseParameters db)
        {
            string connectionString = $"server={db.Server};port={db.Port};user={db.UserID};password={db.Password};";

            try
            {
                var connection = new MySqlConnection(connectionString);
                var cmd = connection.CreateCommand();
                connection.Open();

                cmd.CommandText = "use clocdb; create table if not exists Reports(id int primary key auto_increment, userUcn varchar(255) not null," +
                    "names text, sum double(16,2) default 0,dt DateTime default Now()," +
                    "constraint foreign key(userUcn) references people(ucn) on delete cascade on update cascade);";

                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                ErrorLog.AddErrorLog(ex.ToString());
                return false;
            }

            return true;
        }
    }
}
