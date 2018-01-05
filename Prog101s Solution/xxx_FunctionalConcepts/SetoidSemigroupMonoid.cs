using System;
using System.Collections.Generic;
using System.Linq;

namespace Prog101s.SetoidSemigroupMonoid
{
    //public interface ISetoid<T>
    //{
    //    bool Equals(T other);
    //}

    public interface ISemigroup<T> // where T : ISetoid<T>
    {
        Func<T, T, T> AssociativeOperation { get; set; }
    }

    public interface IMonoid<T> : ISemigroup<T> //where T : ISetoid<T>
    {
        T Identity { get; set; }
    }

    public class Semigroup<T> : ISemigroup<T> //where T : ISetoid<T>
    {
        public Func<T, T, T> AssociativeOperation { get; set; }


        public Semigroup(Func<T, T, T> associativeOperation)
        {
            AssociativeOperation = associativeOperation;
        }

        // needs to have all (x.y).z == x.(y.z)
        public bool IsValidSemigroup(ICollection<T> itemsToTestWith)
        {
            foreach (var x in itemsToTestWith)
            {
                foreach (var y in itemsToTestWith)
                {
                    foreach (var z in itemsToTestWith)
                    {
                        // x,y,z are values from itemsToTestWith
                        if (!AssociativeOperation(AssociativeOperation(x, y), z)
                            .Equals(AssociativeOperation(x, AssociativeOperation(y, z))))
                        {
                            // the operation is not associative for this x,y,z
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }

    public class Monoid<T> : Semigroup<T> //where T : ISetoid<T>
    {
        public T Identity { get; set; }

        public Monoid(Func<T, T, T> associativeOperation, T identity)
            : base(associativeOperation)
        {
            Identity = identity;
        }

        // needs to have an "Empty" (aka "Identity") value
        public bool IsValidMonoid(ICollection<T> itemsToTestWith)
        {
            if (!itemsToTestWith.Contains(Identity)) return false;

            foreach (var x in itemsToTestWith)
            {
                if (!AssociativeOperation(Identity, x).Equals(x) // Identity is not a left identity with x
                    || !AssociativeOperation(x, Identity).Equals(x)) // Identity is not a right Indentity with x
                {
                    // Identity is not a two-sided identity with x
                    return false;
                }

            }
            return IsValidSemigroup(itemsToTestWith);
        }

    }

    public class Floatish // : ISetoid<Floatish>
    {
        public float Value;

        public Floatish(float value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            // c# : null checks x2 ...
            var other = obj as Floatish;
            return Math.Abs(Value - other.Value) < 0.00001;
        }

        // functor?
        public static Floatish FloatishAdd(Floatish x, Floatish y)
        {
            return new Floatish(x.Value + y.Value);
        }
    }

    public static class Concepts
    {
        public static int IntAdd(int x, int y)
        {
            return x + y;
        }

        public static int IntSubtract(int x, int y)
        {
            return x - y;
        }

        public static float FloatAdd(float x, float y)
        {
            return x + y;
        }

        public static void CheckIntAddSemigroup()
        {
            var positiveInts = Enumerable.Range(1, 100).ToArray();

            var intSemigroup = new Semigroup<int>(IntAdd);

            var semigroupValid = intSemigroup.IsValidSemigroup(positiveInts);

            Console.WriteLine(string.Format("Positive ints with add is {0}a semigroup",
                !semigroupValid ? "NOT " : ""));
        }

        public static void CheckIntSubtractSemigroup()
        {
            var positiveInts = Enumerable.Range(1, 100).ToArray();

            var intSemigroup = new Semigroup<int>(IntSubtract);

            var semigroupValid = intSemigroup.IsValidSemigroup(positiveInts);

            Console.WriteLine(string.Format("Positive ints with subtract is {0}a semigroup",
                !semigroupValid ? "NOT " : ""));
        }

        public static void CheckFloatAddSemigroup()
        {
            var positiveFloats = Enumerable.Range(1, 100)
                .Select(i => (float) i / 1000f).ToArray(); //0.001, 0.002, ...0.100

            var floatSemigroup = new Semigroup<float>(FloatAdd);

            var semigroupValid = floatSemigroup.IsValidSemigroup(positiveFloats);

            Console.WriteLine(string.Format("Positive floats with add is {0}a semigroup",
                !semigroupValid ? "NOT " : ""));
        }

        public static void CheckFloatAddEqualishSemigroup()
        {
            var positiveFloatishs = Enumerable.Range(1, 100)
                .Select(i => (float) i / 1000f) //0.001, 0.002, ...0.100
                .Select(f => new Floatish(f))
                .ToArray();

            var floatSemigroup = new Semigroup<Floatish>(Floatish.FloatishAdd);

            var semigroupValid = floatSemigroup.IsValidSemigroup(positiveFloatishs);

            Console.WriteLine(string.Format("Positive floats with add and equalish is {0}a semigroup",
                !semigroupValid ? "NOT " : ""));
        }

        public static void CheckPositiveIntAddMonoid()
        {
            var positiveInts = Enumerable.Range(1, 100).ToArray();

            var intMonoid = new Monoid<int>(IntAdd, 0);

            var monoidValid = intMonoid.IsValidMonoid(positiveInts);

            Console.WriteLine(string.Format("Positive ints with add is {0}a monoid",
                !monoidValid ? "NOT " : ""));
        }

        public static void CheckNonNegativeIntAddMonoid()
        {
            var nonNegativeInts = Enumerable.Range(0, 100).ToArray();

            var intMonoid = new Monoid<int>(IntAdd, 0);

            var monoidValid = intMonoid.IsValidMonoid(nonNegativeInts);

            Console.WriteLine(string.Format("Non negative ints with add is {0}a monoid",
                !monoidValid ? "NOT " : ""));
        }

        public static void Process()
        {
            CheckIntAddSemigroup();

            CheckIntSubtractSemigroup();

            CheckFloatAddSemigroup();

            CheckFloatAddEqualishSemigroup();

            CheckPositiveIntAddMonoid();

            CheckNonNegativeIntAddMonoid();
        }
    }
}

