using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cloc.Classes
{
    internal class Validator
    {
        public static bool ValidateUCN(string UCN)
        {
            Regex rx = new Regex(@"[0-9]{10}");
            MatchCollection matches = rx.Matches(UCN);

            if (matches.Count > 0)
            { return true; }
            else
            {
                return false;
            }
        }

        public static bool ValidateAccessCode(string accessCode)
        {
            Regex rx = new Regex(@"[0-9]{5}");
            MatchCollection matches = rx.Matches(accessCode);

            if (matches.Count > 0)
            { return true; }
            else
            {
                return false;
            }
        }
    }
}