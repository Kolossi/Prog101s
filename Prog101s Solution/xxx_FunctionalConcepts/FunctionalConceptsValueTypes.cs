using System;
using System.Collections.Generic;
using System.Linq;

namespace Prog101s.FunctionalConceptsValueTypes
{
    public interface ISetoid<T>
    {
        Func<T, T, bool> IsEquivalent { get; set; }
    }

    public interface ISemigroup<T> : ISetoid<T>
    {
        Func<T, T, T> AssociativeOperation { get; set; }
    }

    public class Semigroup<T> : ISemigroup<T>
    {
        public Func<T, T, bool> IsEquivalent { get; set; }
        public Func<T, T, T> AssociativeOperation { get; set; }
        public ICollection<T> Items { get; set; }

        public Semigroup(Func<T, T, bool> isEquivalent, Func<T, T, T> associativeOperation, ICollection<T> items)
        {
            IsEquivalent = isEquivalent;
            AssociativeOperation = associativeOperation;
            Items = items;
        }

        // needs to have all (x.y).z == x.(y.z)
        public bool IsValidSemigroup()
        {
            foreach (var x in Items)
            {
                foreach (var y in Items)
                {
                    foreach (var z in Items)
                    {
                        // x,y,z are values from Items
                        if (!IsEquivalent(
                            AssociativeOperation(AssociativeOperation(x, y), z),
                            AssociativeOperation(x, AssociativeOperation(y, z))))
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

    public class Monoid<T> : Semigroup<T>
    {
        public Monoid(Func<T, T, bool> isEquivalent, Func<T, T, T> associativeOperation, ICollection<T> items)
            : base(isEquivalent, associativeOperation, items)
        {
        }

        // needs to have an "Empty" (aka "Identity") value
        public bool IsValidMonoid()
        {
            foreach (var x in Items)
            {
                if (Items.Where(i => !IsEquivalent(x, i)) // all Items that aren't x
                    .All(y => IsEquivalent(AssociativeOperation(x, y), y) // x is a left Identity
                              && IsEquivalent(AssociativeOperation(y, x), y) // x is a right Indentity
                        )
                   )
                {
                    // x is empty/identity value
                    return true;
                }

            }
            return false;
        }

    }

    public interface IFunctor<TA>
    {
        IFunctor<TB> Fmap<TB>(Func<TA, TB> function);
    }

    public interface IContext<TA>
    {
        void StoreInContext(TA a);
        TA RemoveFromContext();
    }

    //http://www.codefugue.com/haskell-in-c-sharp-functors/
    // do a stringybag which converts to string to store
    // do a "trinity" which always hold 3 of TA
    
    public class Bag<TA>:IContext<TA>,IFunctor<TA>
    {
        private TA _stored;

        public Bag()
        {
        }

        public void StoreInContext(TA a)
        {
            _stored = a;
        }

        public TA RemoveFromContext()
        {
            return _stored;
        }

        public IFunctor<TB> Fmap<TB>(Func<TA, TB> function)
        {
            var result = new Bag<TB>();
            result.StoreInContext(function(this.RemoveFromContext()));
            return result;
        }
    }

    public static class Concepts
    {
        public static bool IntEquivalent(int x, int y)
        {
            return x == y;
        }

        public static int IntAdd(int x, int y)
        {
            return x + y;
        }

        public static int IntSubtract(int x, int y)
        {
            return x - y;
        }

        public static void CheckIntAddSemigroup()
        {
            var positiveInts = Enumerable.Range(1, 100).ToArray();

            var intSemigroup = new Semigroup<int>(IntEquivalent, IntAdd, positiveInts);

            var semigroupValid = intSemigroup.IsValidSemigroup();

            Console.WriteLine(string.Format("Positive ints with add is {0}a semigroup",
                !semigroupValid ? "NOT " : ""));
        }

        public static void CheckIntSubtractSemigroup()
        {
            var positiveInts = Enumerable.Range(1, 100).ToArray();

            var intSemigroup = new Semigroup<int>(IntEquivalent, IntSubtract, positiveInts);

            var semigroupValid = intSemigroup.IsValidSemigroup();

            Console.WriteLine(string.Format("Positive ints with subtract is {0}a semigroup",
                !semigroupValid ? "NOT " : ""));
        }


        public static bool FloatEquivalent(float x, float y)
        {
            return x == y;
        }

        public static bool FloatEqualish(float x, float y)
        {
            return Math.Abs(x - y) < 0.00001;
        }

        public static float FloatAdd(float x, float y)
        {
            return x + y;
        }

        public static void CheckFloatAddSemigroup()
        {
            var positiveFloats = Enumerable.Range(1, 100)
                .Select(i => (float)i / 1000f).ToArray(); //0.001, 0.002, ...0.100

            var floatSemigroup = new Semigroup<float>(FloatEquivalent, FloatAdd, positiveFloats);
            
            var semigroupValid = floatSemigroup.IsValidSemigroup();

            Console.WriteLine(string.Format("Positive floats with add is {0}a semigroup",
                !semigroupValid ? "NOT " : ""));
        }

        public static void CheckFloatAddEqualishSemigroup()
        {
            var positiveFloats = Enumerable.Range(1, 100)
                .Select(i => (float)i / 1000f).ToArray(); //0.001, 0.002, ...0.100

            var floatSemigroup = new Semigroup<float>(FloatEqualish, FloatAdd, positiveFloats);

            var semigroupValid = floatSemigroup.IsValidSemigroup();

            Console.WriteLine(string.Format("Positive floats with add and equalish is {0}a semigroup",
                !semigroupValid ? "NOT " : ""));
        }

        public static void CheckPositiveIntAddMonoid()
        {
            var positiveInts = Enumerable.Range(1, 100).ToArray();

            var intMonoid = new Monoid<int>(IntEquivalent, IntAdd, positiveInts);

            var monoidValid = intMonoid.IsValidMonoid();

            Console.WriteLine(string.Format("Positive ints with add is {0}a monoid",
                !monoidValid ? "NOT " : ""));
        }

        public static void CheckNonNegativeIntAddMonoid()
        {
            var nonNegativeInts = Enumerable.Range(0, 100).ToArray();

            var intMonoid = new Monoid<int>(IntEquivalent, IntAdd, nonNegativeInts);

            var monoidValid = intMonoid.IsValidMonoid();

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
