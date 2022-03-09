using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloc.Classes
{
    public class User
    {
        public string UserUCN { get; set; }
        public string AccessCode { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }

        public bool IsCheckedIn { get; set; }
        public double HourPayment { get; set; }
        public double TotalHours { get; set; }
        public double Percent { get; set; }
    }
}
