using System.Collections.Generic;

namespace Cloc.Classes
{
    internal class Filter
    {
        internal static List<string> FilterItems(List<string> allItems, int count)
        {
            List<string> filteredItems = new();
            for (int index = 0; index < allItems.Count; index++)
            {
                filteredItems.Add(allItems[index]);
                if (index == (count - 1))
                {
                    break;
                }
            }

            if (filteredItems.Count > 0)
            { return filteredItems; }
            else
            { return new List<string>(); }
        }
    }
}
