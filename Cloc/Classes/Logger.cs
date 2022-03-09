using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloc.Classes
{
    public static class Logger
    {
        static public List<string> UserLogs(User user, int count)
        {
            List<string> logs = new List<string>();
            List<string> allLogs = new List<string>();
            string[] lines = File.ReadAllLines(".\\Logs.txt");

            foreach (string line in lines)
            {
                string[] results = line.Split(';', ';');

                if (results[1] == user.UserUCN)
                {
                    string temp = results[0] + ";" + results[1] + ";" + results[2];
                    allLogs.Add(temp);
                }
            }

            allLogs.Reverse();
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

        static public void AddLog(User user, string activity)
        {
            string activityLine = DateTime.Now + ";" + user.UserUCN + ";" + activity;

            if (File.Exists(".\\Logs.txt"))
            {
                File.AppendAllText(".\\Logs.txt", activityLine + Environment.NewLine);
            }
            else
            {
                File.Create(".\\Logs.txt");
            }
        }
    }
}
