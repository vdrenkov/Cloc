using System;

namespace Cloc.Classes
{
    internal class User
    {
        internal string UserUCN { get; set; }
        internal string AccessCode { get; set; }
        internal DateTime CheckIn { get; set; }
        internal DateTime CheckOut { get; set; }
        internal bool IsCheckedIn { get; set; }
        internal double HourPayment { get; set; }
        internal double TotalHours { get; set; }
        internal double Percent { get; set; }
    }
}
