using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Cloc.Classes
{
    internal class Validator
    {
        public static bool ValidateUCN(string UCN)
        {
            Regex rx = new Regex("^[0-9]{10}$");
            MatchCollection matches=null;

            try
            { matches = rx.Matches(UCN); }
            catch (Exception)
            {
                MessageBox.Show("Възникна грешка при валидация на вашето ЕГН.");
            }

            if (matches.Count > 0)
            { return true; }
            else
            {
                return false;
            }
        }

        public static bool ValidateAccessCode(string accessCode)
        {
            bool flag = false;

            try
            {
                Regex rx = new Regex("^[0-9]{5}$");
                MatchCollection matches = rx.Matches(accessCode);
                if (matches.Count > 0)
                { flag= true; }
            }
            catch(Exception)
            {
                MessageBox.Show("Възникна грешка при валидация на вашия код за достъп.");
            }
            return flag;
        }
    }
}