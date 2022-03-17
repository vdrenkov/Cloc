using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Cloc.Classes.Security;

namespace Cloc.Classes
{
    public static class Logger
    {
        static public void AddLog(string UCN, string activity)
        {
            UCN = EncryptString(UCN);
            string activityLine = DateTime.Now + ";" + UCN + ";" + activity;

            try
            {
                if (File.Exists(".\\Logs.txt"))
                {
                    File.AppendAllText(".\\Logs.txt", activityLine + Environment.NewLine);
                }
                else
                {
                    File.Create(".\\Logs.txt");
                }
            }
            catch (Exception)
            { MessageBox.Show("Възникна неочаквана грешка при записване на вашата активност."); }
        }
        static public List<string> UserLogs(string UCN, int count)
        {
            List<string> logs = new List<string>();
            List<string> allLogs = new List<string>();

            try
            {
                string[] lines = File.ReadAllLines(".\\Logs.txt");

                foreach (string line in lines)
                {
                    string[] results = line.Split(';', ';');
                    results[1] = DecryptString(results[1]);

                    if (results[1] == UCN)
                    {
                        string temp = results[0] + ";" + results[1] + ";" + results[2];
                        allLogs.Add(temp);
                    }
                }
                allLogs.Reverse();
            }
            catch (Exception)
            { MessageBox.Show("Възникна неочаквана грешка при извличане на вашата активност."); }

            for (int i = 0; i < allLogs.Count; i++)
            {
                logs.Add(allLogs[i]);
                if (i == (count - 1))
                {
                    break;
                }
            }
            return logs;
        }
    }
}
