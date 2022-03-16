using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloc.Classes
{
    enum WorkPosition
    {
        Guest,
        Bartender,
        Barmaid,
        Barista,
        Waiter,
        Waitress,
        Busser,
        Host,
        Hostess,
        Manager,
        Boss,
        Admin
    }
    internal class Person
    {
        public string UCN { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; } 
        public string PhoneNumber { get; set; }
        public string Country {get; set; }
        public string City {get; set; }
        public string Address { get; set; }
        public WorkPosition Position { get; set; }
    }
}
