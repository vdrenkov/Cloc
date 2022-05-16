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

        internal static bool WriteInfoToFile(DBInfo db)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.WriteAllText(path, db.Server + Environment.NewLine + db.Port + Environment.NewLine + db.UserID + Environment.NewLine + db.Password);
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

        static internal bool SetSettings(DBInfo db)
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

        static internal DBInfo RetrieveInfo()
        {
            List<string> data = new();
            DBInfo db = new();

            using (StreamReader reader = new(path))
            {
                var line = reader.ReadLine();

                while (line != null)
                {
                    data.Add(line);
                    line = reader.ReadLine();
                }
            }

            if (data != null&&data.Count>=PARAMETERS_COUNT)
            {
                db.Server = data[0];
                db.Port = data[1];
                db.UserID = data[2];
                db.Password = data[3];
            }

            return db;
        }

        static internal string GetServer()
        {
            DBInfo db = RetrieveInfo();

            if (db.Server!=null)
                return db.Server;
            else
                return string.Empty;
        }

        static internal string GetPort()
        {
            DBInfo db = RetrieveInfo();

            if (db.Port != null)
                return db.Port;
            else
                return string.Empty;
        }

        static internal string GetUserID()
        {
            DBInfo db = RetrieveInfo();

            if (db.UserID != null)
                return db.UserID;
            else
                return string.Empty;
        }

        static internal string GetPassword()
        {
            DBInfo db = RetrieveInfo();

            if (db.Password != null)
                return db.Password;
            else
                return string.Empty;
        }
    }
}
