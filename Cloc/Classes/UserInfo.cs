using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloc.Classes
{
    internal class UserInfo
    {
        public string UserUCN { get; set; }
        public string AccessCode { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public double HourPayment { get; set; }
        public double TotalHours { get; set; }
        public double Percent { get; set; }

    }
}
