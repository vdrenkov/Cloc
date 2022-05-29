using Cloc.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Cloc.Database
{
    internal class DatabaseInfo
    {

        private readonly static int PARAMETERS_COUNT = 4;
        private readonly static string path = ".\\DBInfo.txt";

        internal static bool WriteInfoToFile(DatabaseParameters db)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.WriteAllText(path, Security.EncryptString(db.Server) + Environment.NewLine + Security.EncryptString(db.Port) + Environment.NewLine + Security.EncryptString(db.UserID) + Environment.NewLine + Security.EncryptString(db.Password));
                }
                else
                {
                    File.Create(path).Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Възникна неочаквана грешка по време на работа.");
                ErrorLog.AddErrorLog(ex.ToString());
                return false;
            }
        }

        static internal bool SetSettings(DatabaseParameters db)
        {
            bool isSaved = WriteInfoToFile(db);

            if (isSaved)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static internal DatabaseParameters RetrieveInfo()
        {
            List<string> data = new();
            DatabaseParameters db = new();

            using (StreamReader reader = new(path))
            {
                var line = reader.ReadLine();

                while (line != null)
                {
                    data.Add(line);
                    line = reader.ReadLine();
                }
            }

            if (data != null && data.Count >= PARAMETERS_COUNT)
            {
                db.Server = Security.DecryptString(data[0]);
                db.Port = Security.DecryptString(data[1]);
                db.UserID = Security.DecryptString(data[2]);
                db.Password = Security.DecryptString(data[3]);
            }

            return db;
        }

        static internal string GetServer()
        {
            DatabaseParameters db = RetrieveInfo();

            if (db.Server != null)
                return db.Server;
            else
                return string.Empty;
        }

        static internal string GetPort()
        {
            DatabaseParameters db = RetrieveInfo();

            if (db.Port != null)
                return db.Port;
            else
                return string.Empty;
        }

        static internal string GetUserID()
        {
            DatabaseParameters db = RetrieveInfo();

            if (db.UserID != null)
                return db.UserID;
            else
                return string.Empty;
        }

        static internal string GetPassword()
        {
            DatabaseParameters db = RetrieveInfo();

            if (db.Password != null)
                return db.Password;
            else
                return string.Empty;
        }
    }
}