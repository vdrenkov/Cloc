using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloc.Classes
{
    enum WorkPosition
    {
        OnTrial,
        Hygienist,
        PrepCook,
        LineCook,
        Cook,
        SousChef,
        Chef,
        Bartender,
        Barmaid,
        Barista,
        Waiter,
        Waitress,
        Busser,
        Host,
        Hostess,
        FnB,
        Manager,
        Admin
    }

    internal class Person
    {
        public string UCN { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public WorkPosition Position { get; set; }

        internal static string TranslateFromWorkPosition(WorkPosition wp)
        {
            return wp switch
            {
                WorkPosition.Hygienist => "Хигиенист/ка",
                WorkPosition.PrepCook => "Заготовщик",
                WorkPosition.LineCook => "Готвач поточна линия",
                WorkPosition.Cook => "Готвач",
                WorkPosition.SousChef => "Помощник главен готвач",
                WorkPosition.Chef => "Главен готвач",
                WorkPosition.Bartender => "Барман",
                WorkPosition.Barmaid => "Барманка",
                WorkPosition.Barista => "Бариста",
                WorkPosition.Waiter => "Сервитьор",
                WorkPosition.Waitress => "Сервитьорка",
                WorkPosition.Busser => "Помощник-сервитьор/ка",
                WorkPosition.Host => "Хост",
                WorkPosition.Hostess => "Хостеса",
                WorkPosition.FnB => "Специалист храни и напитки",
                WorkPosition.Manager => "Мениджър",
                WorkPosition.Admin => "Администратор / Собственик",
                _ => "На изпитателен срок",
            };
        }

        internal static WorkPosition TranslateToWorkPosition(string wp)
        {
            return wp switch
            {
                "Хигиенист/ка" => WorkPosition.Hygienist,
                "Заготовщик" => WorkPosition.PrepCook,
                "Готвач поточна линия" => WorkPosition.LineCook,
                "Готвач" => WorkPosition.Cook,
                "Помощник главен готвач" => WorkPosition.SousChef,
                "Главен готвач" => WorkPosition.Chef,
                "Барман" => WorkPosition.Bartender,
                "Барманка" => WorkPosition.Barmaid,
                "Бариста" => WorkPosition.Barista,
                "Сервитьор" => WorkPosition.Waiter,
                "Сервитьорка" => WorkPosition.Waitress,
                "Помощник-сервитьор/ка" => WorkPosition.Busser,
                "Хост" => WorkPosition.Host,
                "Хостеса" => WorkPosition.Hostess,
                "Специалист храни и напитки" => WorkPosition.FnB,
                "Мениджър" => WorkPosition.Manager,
                "Администратор / Собственик" => WorkPosition.Admin,
                _ => WorkPosition.OnTrial
            };
        }
    }
}
