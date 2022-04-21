using System;
using System.Text.RegularExpressions;
using System.Windows;
using static Cloc.Database.DatabaseQuery;

namespace Cloc.Classes
{
    internal static class Validator
    {
        internal static bool ValidateUCN(string UCN)
        {
            try
            {
                Regex rx = new("^[0-9]{10}$");
                MatchCollection matches;

                matches = rx.Matches(UCN);

                if (matches.Count > 0)
                { return true; }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Въведено е неправилно ЕГН.");
                ErrorLog.AddErrorLog(ex.ToString());
                return false;
            }
        }

        internal static bool ValidateAccessCode(string accessCode)
        {
            bool flag = false;

            try
            {
                Regex rx = new("^[0-9]{5}$");
                MatchCollection matches = rx.Matches(accessCode);
                if (matches.Count > 0)
                { flag = true; }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Въведен е неправилен код за достъп.");
                ErrorLog.AddErrorLog(ex.ToString());
            }
            return flag;
        }

        internal static bool ValidateAccessCodeChange(string ucn, string accessCode)
        {
            bool flag = false;

            if (ValidateUCN(ucn))
            {
                if (ValidateAccessCode(accessCode))
                {
                    if (!SelectAccessCodeQuery(accessCode))
                    {
                        if (ChangeAccessCodeQuery(ucn, accessCode))
                        { flag = true; }
                        else
                        {
                            MessageBox.Show("Въвели сте несъществуващо ЕГН.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Въведеният код за достъп вече се използва.");
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

        internal static bool ValidateEntry(string accessCode)
        {
            bool flag = false;
            Person currentPerson = new();
            User currentUser = new();

            if (ValidateAccessCode(accessCode))
            { currentUser = SelectUserByAccessCodeQuery(accessCode); }

            if (currentUser.UserUCN != null)
            { currentPerson = SelectPersonQuery(currentUser.UserUCN); }

            if (currentPerson.UCN != null)
            {
                Session.UserToken.SetLoginData(currentPerson.UCN);
                flag = true;
            }

            return flag;
        }

        internal static bool IsAdmin(Person person)
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