using System.Collections.Generic;

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
        Owner,
        Admin
    }

    internal class Person
    {
        internal string UCN { get; set; }
        internal string Name { get; set; }
        internal string Surname { get; set; }
        internal string Email { get; set; }
        internal string PhoneNumber { get; set; }
        internal string Country { get; set; }
        internal string City { get; set; }
        internal string Address { get; set; }
        internal WorkPosition Position { get; set; }

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
                WorkPosition.Owner => "Собственик",
                WorkPosition.Admin => "Администратор",
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
                "Собственик" => WorkPosition.Owner,
                "Администратор" => WorkPosition.Admin,
                _ => WorkPosition.OnTrial
            };
        }

        internal static int AdminCount()
        {
            int adminCount = 0;

            List<Person> people = Database.DatabaseQuery.SelectAllPeopleQuery();

            foreach (Person person in people)
            {
                if (person.Position == WorkPosition.Admin)
                {
                    adminCount++;
                }
            }
            return adminCount;
        }
    }
}
