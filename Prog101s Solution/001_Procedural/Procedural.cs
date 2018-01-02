using System;

namespace Prog101s
{
    public class Procedural
    {
        public class Person
        {
            public int Id;
            public string Firstname;
            public string Lastname;
            public int Age;
            public decimal[] SpentInYear;
            public decimal DiscountPercent;
            
            #region more Person fields...
            public string CustomerNumber;
            public string EmployeeNumber;
            public EmployeeType EmployeeType;
            public decimal[] PaidInYear;
            #endregion

        }

        #region more data types...
        public enum EmployeeType
        {
            None,
            Worker,
            Manager
        }
        #endregion

        public static void Process()
        {
            var people = LoadPeopleFromStorage();
            UpdateSpendFromStorage(people);
            for (int i = 0; i < people.Length; i++)
            {
                UpdateDiscount(people[i]);
            }
            WriteDiscountReport(people);
        }

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
            var parts = fileLine.Split(",");
            return new Person() {
                Id = int.Parse(parts[0]),
                Firstname = parts[1],
                Lastname = parts[2],
                Age = int.Parse(parts[3]),
                SpentInYear = new decimal[5]
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
            var parts = fileLine.Split(",");
            var personId = int.Parse(parts[0]);
            var yearIndex = 2017 - int.Parse(parts[1]);
            if (yearIndex < 5)
            {
                people[personId-1].SpentInYear[yearIndex] = decimal.Parse(parts[2]);
            }
        }

        public static void UpdateDiscount(Person person)
        {
            if (person.Age > 65)
            {
                person.DiscountPercent = 10.0m;
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if (person.SpentInYear[i] > 800.0m)
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
                Console.WriteLine(string.Format("{0},{1},{2},{3:F2}%", person.Id, person.Firstname, person.Lastname,
                    person.DiscountPercent));
            }
        }
    }
}
