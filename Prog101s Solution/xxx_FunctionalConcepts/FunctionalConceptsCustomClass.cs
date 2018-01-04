using System;
using System.Collections.Generic;
using System.Text;

namespace Prog101s.FunctionalConceptsCustomClass
{
    public interface ISetoid<T>
    {
        bool Equivalent(T other);
    }

    public interface ISemigroup<T> : ISetoid<T>
    {
        T AssociativeOperation(T x, T y);
    }

    public class Person : ISemigroup<Person>
    {

        public bool Equivalent(Person other)
        {
            throw new NotImplementedException();
        }

        public Person AssociativeOperation(Person x, Person y)
        {
            throw new NotImplementedException();
        }
    }
}

