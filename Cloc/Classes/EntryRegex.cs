using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cloc.Classes
{
    internal class EntryRegex
    {
        public static bool ValidateEntry(string entry)
        {
            Regex rx = new Regex(@"[0-9]{5}");
            MatchCollection matches = rx.Matches(entry);

            if (matches.Count > 0)
            { return true; }
            else
            {
                return false;
            }
        }
    }
}