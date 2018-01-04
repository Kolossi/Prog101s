using System;

namespace Prog101s.ObjectOrientated_Simple
{
    public static class Simple
    {
        public static void Process()
        {
            var storage = new Storage();
            var people = storage.LoadPeople();
            storage.UpdateSpend(people);

            for (int i = 0; i < people.Length; i++)
            {
                people[i].SetDiscount();
            }
            var reporter = new Reporter(people);
            reporter.WriteDiscountReport();
        }


        public class Person
        {
            public int Id;
            public string Firstname;
            public string Lastname;
            public int Age;
            public decimal[] FiveYearSpend;
            public decimal DiscountPercent;

            public Person(int id, string firstname, string lastname, int age)
            {
                this.Id = id;
                this.Firstname = firstname;
                this.Lastname = lastname;
                this.Age = age;
                this.FiveYearSpend = new decimal[5];
                this.DiscountPercent = 0.0m;
            }

            public Person(string[] csvParts)
                : this(int.Parse(csvParts[0]), csvParts[1], csvParts[2], int.Parse(csvParts[3]))
            {
            }

            public void UpdateSpend(int year, decimal spend)
            {
                var yearIndex = 2017 - year;
                if (yearIndex < 5)
                {
                    this.FiveYearSpend[yearIndex] = spend;
                }
            }

            public void SetDiscount()
            {
                if (this.Age > 65)
                {
                    this.DiscountPercent = 10.0m;
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (this.FiveYearSpend[i] > 800.0m)
                        {
                            this.DiscountPercent += 2.0m;
                        }
                    }
                }
            }
        }

        public class Reporter
        {
            public Person[] People;

            public Reporter(Person[] people)
            {
                this.People = people;
            }

            public void WriteDiscountReport()
            {
                for (int i = 0; i < People.Length; i++)
                {
                    var person = People[i];
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
                        Console.WriteLine(string.Format("{0},{1},{2},{3:F2}%", person.Id, person.Firstname,
                            person.Lastname,
                            person.DiscountPercent));
                    }
                }
            }
        }


        public class Storage
        {
            private const string PERSON_DATA_FILENAME = "../PersonData.txt";
            private const string SPEND_DATA_FILENAME = "../SpendData.txt";

            public Person[] LoadPeople()
            {
                var lines = System.IO.File.ReadAllLines(PERSON_DATA_FILENAME);
                var personCount = int.Parse(lines[0]);
                var people = new Person[personCount];
                for (int i = 1; i <= personCount; i++)
                {
                    var csvParts = lines[i].Split(",");
                    people[i - 1] = new Person(csvParts);
                }
                return people;
            }

            public void UpdateSpend(Person[] people)
            {
                var lines = System.IO.File.ReadAllLines(SPEND_DATA_FILENAME);
                for (int i = 0; i < lines.Length; i++)
                {
                    var csvParts = lines[i].Split(",");
                    var personId = int.Parse(csvParts[0]);
                    var year = int.Parse(csvParts[1]);
                    var spend = decimal.Parse(csvParts[2]);

                    people[personId-1].UpdateSpend(year, spend);
                }
            }
        }
    }
}

