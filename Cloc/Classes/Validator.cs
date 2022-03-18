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
            try
            {
                Regex rx = new Regex("^[0-9]{10}$");
                MatchCollection matches;

                matches = rx.Matches(UCN);
                if (matches.Count > 0)
                { return true; }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Грешка при валидация на ЕГН..");
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
                Console.WriteLine("Грешка при валидация на кода за достъп.");
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
                        Console.WriteLine("Промяната не беше успешна. Моля, въведете съществуващо ЕГН!");
                    }
                }
                else
                {
                    Console.WriteLine("Моля, въведете 5-цифрен код за достъп!");
                }
            }
            else
            {
                Console.WriteLine("Моля, въведете съществуващо ЕГН!");
            }
            return flag;
        }

        public static bool ValidateEntry(string accessCode)
        {
            bool flag = false;
            Person currentPerson = new Person();
            User currentUser = new User();

            if (ValidateAccessCode(accessCode))
            { currentUser = SelectUserByAccessCodeQuery(accessCode); }

            if (currentUser.UserUCN != null)
            { currentPerson = SelectPersonQuery(currentUser.UserUCN); }

            if (currentPerson.UCN != null)
            {
                flag = true;
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