using System;

namespace Prog101s.Procedural
{
    public static class Procedures
    {
        public class Person
        {
            public int Id;
            public string Firstname;
            public string Lastname;
            public int Age;
            public decimal[] FiveYearSpend;
            public decimal DiscountPercent;
        }
        
        public static void Process()
        {
            var people = LoadPeopleFromStorage();
            UpdateSpendFromStorage(people);

            for (int i = 0; i < people.Length; i++)
            {
                SetDiscount(people[i]);
            }
            WriteDiscountReport(people);
        }

        public static void SetDiscount(Person person)
        {
            if (person.Age > 65)
            {
                person.DiscountPercent = 10.0m;
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if (person.FiveYearSpend[i] > 800.0m)
                    {
                        person.DiscountPercent += 2.0m;
                    }
                }
            }
        }

        public static void WriteDiscountReport(Person[] people)
        {
            for (int i = 0; i < people.Length; i++)
            {
                var person = people[i];
                bool show = false;
                for (int j = 0; j < person.FiveYearSpend.Length; j++)
                {
                    if (person.FiveYearSpend[j] > 0)
                    {
                        show = true;
                        break;
                    }
                }
                if (show)
                {
                    Console.WriteLine(string.Format("{0},{1},{2},{3:F2}%", person.Id, person.Firstname, person.Lastname,
                        person.DiscountPercent));
                }
            }
        }

        #region storage access...

        public static Person[] LoadPeopleFromStorage()
        {
            var lines = System.IO.File.ReadAllLines("../PersonData.txt");
            var personCount = int.Parse(lines[0]);
            var people = new Person[personCount];
            for (int i = 1; i <= personCount; i++)
            {
                people[i-1] = MakePerson(lines[i]);
            }
            return people;
        }

        public static Person MakePerson(string fileLine)
        {
            var csvParts = fileLine.Split(",");
            return new Person() {
                Id = int.Parse(csvParts[0]),
                Firstname = csvParts[1],
                Lastname = csvParts[2],
                Age = int.Parse(csvParts[3]),
                FiveYearSpend = new decimal[5]
            };
        }

        public static void UpdateSpendFromStorage(Person[] people)
        {
            var lines = System.IO.File.ReadAllLines("../SpendData.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                UpdatePersonSpend(people, lines[i]);
            }
        }

        public static void UpdatePersonSpend(Person[] people, string fileLine)
        {
            var csvParts = fileLine.Split(",");
            var personId = int.Parse(csvParts[0]);
            var yearIndex = 2017 - int.Parse(csvParts[1]);
            if (yearIndex < 5)
            {
                people[personId-1].FiveYearSpend[yearIndex] = decimal.Parse(csvParts[2]);
            }
        }

        #endregion
    }
}
