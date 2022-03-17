using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using static Cloc.Database.DatabaseQuery;

namespace Cloc.Classes
{
    internal class Validator
    {
        public static bool ValidateUCN(string UCN)
        {
            Regex rx = new Regex("^[0-9]{10}$");
            MatchCollection matches = null;

            try
            { matches = rx.Matches(UCN); }
            catch (Exception)
            {
                MessageBox.Show("Грешка при валидация на ЕГН..");
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
                { flag = true; }
            }
            catch (Exception)
            {
                MessageBox.Show("Грешка при валидация на кода за достъп.");
            }
            return flag;
        }

        public static bool ValidateAccessCodeChange(string ucn, string accessCode)
        {
            bool flag = false;

            if (ValidateUCN(ucn))
            {
                if (ValidateAccessCode(accessCode))
                {
                    if (ChangeAccessCodeQuery(ucn, accessCode))
                    {
                        flag = true;
                    }
                    else
                    {
                        MessageBox.Show("Промяната не беше успешна. Моля, въведете съществуващо ЕГН!");
                    }
                }
                else
                {
                    MessageBox.Show("Моля, въведете 5-цифрен код за достъп!");
                }
            }
            else
            {
                MessageBox.Show("Моля, въведете съществуващо ЕГН!");
            }
            return flag;
        }

        public static bool ValidateEntry(string accessCode)
        {
            bool flag = false;
            Person currentPerson = null;
            User currentUser = null;

            if (ValidateAccessCode(accessCode))
            { currentUser = GetUserByAccessCodeQuery(accessCode); }

            if (object.ReferenceEquals(null, currentUser)) { }else
            { currentPerson = SelectPersonQuery(currentUser.UserUCN); }

            if (object.ReferenceEquals(null, currentPerson)) { }else
            {
                flag = true;
                MessageBox.Show(currentPerson.UCN+currentUser.AccessCode);
            }

            return flag;
        }

        public static bool isAdmin(Person person)
        {
            bool flag = false;

            if (person.Position == WorkPosition.Admin)
            {
                flag = true;
            }

            return flag;
        }
    }
}